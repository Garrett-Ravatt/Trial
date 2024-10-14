using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Components;
using Catalyster.Messages;
using TinyMessenger;
using static System.Net.Mime.MediaTypeNames;

namespace Catalyster.Helpers
{
    public class ActionHelper
    {
        public static bool ResolveAttack(Entity attacker, Entity defender)
        {
            if ( attacker.Has<MeleeAttack>() )
            {
                return ResolveMelee(attacker.Get<MeleeAttack>(), defender, attacker);
            }

            return false;
        }

        public static bool ResolveMelee(MeleeAttack attack, Entity defender, Entity attacker)
        {
            if (!defender.Has<Defense>())
                return false;
            //NOTE: implementation assumes defender has Defense and Health components
            var toHit = attack.AttackFormula.Roll().Value;
            var ac = defender.Get<Defense>().Class;

            var msg = new MeleeAttackMessage(attack, attacker.Reference(), defender.Reference(), toHit);

            if ( toHit > ac )
            {
                msg.Hit = true;

                //Console.WriteLine($"{defender} is hurt.");
                ref var health = ref defender.Get<Health>();

                var damage = attack.DamageFormula.Roll().Value;
                msg.Damage = damage;
                
                GameMaster.MessageLog.Hub.Publish(msg);

                health.Points -= damage;
                if (health.Points <=0)
                {
                    GameMaster.MessageLog.Hub.Publish(new DeathMessage(attack, defender.Reference()));

                    //TODO: needs test coverage.
                    GameMaster.World.Destroy(defender);
                }
                return true;
            }
            
            else
            {
                msg.Hit = false;
                //GameMaster.MessageLog.IDAdd(attacker, $"misses [{toHit}]");
                // TODO: Publish
            }

            GameMaster.MessageLog.Hub.Publish(msg);
            // TODO: Publish
            return false;
        }

        public static bool ResolveRanged(RangedAttack attack, Entity defender, Entity attacker)
        {
            //NOTE: implementation assumes defender has Defense and Health components
            var toHit = attack.AttackFormula.Roll().Value;
            Defense def;
            if (!defender.TryGet(out def))
                return false;
            var ac = def.Class;

            if (toHit > ac)
            {
                Console.WriteLine($"{defender} is hurt.");
                ref var health = ref defender.Get<Health>();

                var damage = attack.DamageFormula.Roll().Value;
                // TODO: Message
                GameMaster.MessageLog.Hub.Publish(
                    new RangedAttackMessage(attack, attacker.Reference(), defender.Reference(), toHit, damage
                    ));

                health.Points -= damage;
                if (health.Points <= 0)
                {
                    GameMaster.MessageLog.Hub.Publish(new DeathMessage(attack, defender.Reference()));
                    Console.WriteLine($"{defender} dies!");
                    //TODO: needs test coverage.
                    GameMaster.World.Destroy(defender);
                }
                return true;
            }

            GameMaster.MessageLog.Hub.Publish(
                    new RangedAttackMessage(attack, attacker.Reference(), defender.Reference(), toHit, hit:false
                    ));

            return false;
        }
    }
}
