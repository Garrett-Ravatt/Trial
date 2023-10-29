using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Components;
using Catalyster.Core;
using Catalyster.Interfaces;

namespace Catalyster.Helpers
{
    public class ActionHelper
    {
        public static bool ResolveAttack(Entity attacker, Entity defender)
        {
            var attackerName = "";
            if (attacker.Has<Token>())
            {
                attackerName = attacker.Get<Token>().Name;
            }

            if ( attacker.Has<MeleeAttack>() )
            {
                return ResolveMelee(attacker.Get<MeleeAttack>(), defender, attackerName);
            }

            return false;
        }

        public static bool ResolveMelee(MeleeAttack attack, Entity defender, string attacker = "")
        {
            //NOTE: implementation assumes defender has Defense and Health components
            var toHit = attack.AttackFormula.Roll().Value;
            var ac = defender.Get<Defense>().Class;

            if ( toHit > ac )
            {
                Console.WriteLine($"{defender} is hurt.");
                ref var health = ref defender.Get<Health>();

                var damage = attack.DamageFormula.Roll().Value;
                GameMaster.MessageLog.IDAdd(attacker, $"hits [{toHit}] for {damage} damage");
                
                health.Points -= damage;
                if (health.Points <=0)
                {
                    GameMaster.MessageLog.IDAdd(defender, "dies!");
                    Console.WriteLine($"{defender} dies!");
                    //TODO: safely destroy defender entity
                }
                return true;
            }
            
            else
            {
                GameMaster.MessageLog.IDAdd(attacker, $"misses [{toHit}]");
            }

            // TODO: replace with message log
            Console.WriteLine($"{defender} successfully defends.");
            return false;
        }
    }
}
