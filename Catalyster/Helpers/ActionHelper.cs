using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Components;
using Catalyster.Interfaces;

namespace Catalyster.Helpers
{
    public class ActionHelper
    {
        public static bool ResolveAttack(Entity attacker, Entity defender)
        {
            Console.WriteLine($"{attacker} attacks {defender}...");
            if ( attacker.Has<MeleeAttack>() )
            {
                return ResolveMelee(attacker.Get<MeleeAttack>(), defender);
            }

            return false;
        }

        public static bool ResolveMelee(MeleeAttack attack, Entity defender)
        {
            //NOTE: implementation assumes defender has Defense and Health components
            if ( attack.AttackFormula.Roll().Value > defender.Get<Defense>().Class )
            {
                Console.WriteLine($"{defender} is hurt.");
                ref var health = ref defender.Get<Health>();
                health.Points -= attack.DamageFormula.Roll().Value;
                if (health.Points <=0)
                {
                    Console.WriteLine($"{defender} dies!");
                    //TODO: safely destroy defender entity
                }
                return true;
            }
            // TODO: replace with message log
            Console.WriteLine($"{defender} successfully defends.");
            return false;
        }
    }
}
