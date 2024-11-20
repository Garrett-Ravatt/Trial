using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Components;
using Catalyster.Messages;

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
            // TODO: Find appropriate conditions for exception
            if (!defender.Has<Stats>() || !attacker.Has<Stats>())
            {
                Console.WriteLine($"Invalid Melee attack attempted");
                return false;
            }

            var toHit = attack.AttackFormula.Roll().Value + attacker.Get<Stats>().Body;
            var ac = defender.Get<Stats>().Body;
            int hits = toHit / ac;

            var msg = new MeleeAttackMessage(attack, attacker.Reference(), defender.Reference(), toHit);

            if ( hits > 0 )
            {
                msg.Hit = true;

                ref var stat = ref defender.Get<Stats>();

                //var damage = attack.DamageFormula.Roll().Value;
                int damage = 0;
                for ( int i = 0; i<hits; i++) { damage += attack.DamageFormula.Roll().Value; }
                msg.Damage = damage;
                
                GameMaster.Instance().MessageLog.Hub.Publish(msg);

                stat.HP -= damage;
                if (stat.HP <=0)
                {
                    GameMaster.Instance().MessageLog.Hub.Publish(new DeathMessage(attack, defender.Reference()));

                    //TODO: needs test coverage.
                    GameMaster.Instance().World.Destroy(defender);
                }
                return true;
            }
            
            else
            {
                msg.Hit = false;
            }

            GameMaster.Instance().MessageLog.Hub.Publish(msg);
            return true;
        }

        public static bool ResolveRanged(RangedAttack attack, Entity defender, Entity attacker)
        {
            //NOTE: implementation assumes defender has Body and Health components
            var toHit = attack.AttackFormula.Roll().Value;

            if (!defender.Has<Stats>())
                return false;
            ref Stats stats = ref defender.Get<Stats>();

            var ac = stats.Body;

            if (toHit > ac)
            {
                Console.WriteLine($"{defender} is hurt.");

                var damage = attack.DamageFormula.Roll().Value;
                GameMaster.Instance().MessageLog.Hub.Publish(
                    new RangedAttackMessage(attack, attacker.Reference(), defender.Reference(), toHit, damage
                    ));

                stats.HP -= damage;
                if (stats.HP <= 0)
                {
                    GameMaster.Instance().MessageLog.Hub.Publish(new DeathMessage(attack, defender.Reference()));
                    Console.WriteLine($"{defender} dies!");
                    //TODO: needs test coverage.
                    GameMaster.Instance().World.Destroy(defender);
                }
                return true;
            }

            GameMaster.Instance().MessageLog.Hub.Publish(
                    new RangedAttackMessage(attack, attacker.Reference(), defender.Reference(), toHit, hit:false
                    ));

            return false;
        }
    }
}
