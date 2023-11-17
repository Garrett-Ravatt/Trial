using Arch.Core;
using Arch.Core.Extensions;
using Arch.Relationships;
using Catalyster.Components;
using RogueSharp.DiceNotation;

namespace Catalyster.Helpers
{
    public static class ItemPropHelper
    {
        // try to put something in a container.
        // returns true if it could put the content in the container
        public static bool Contain(Entity container, Entity content)
        {
            if (container.Has<Container, Item>() && content.Has<Item>())
            {
                ref var item = ref content.Get<Item>();

                //TODO: Care about capacity
                ref var c = ref container.Get<Container>();
                ref var i = ref container.Get<Item>();

                // update internal Filled
                c.Filled += item.Fill;

                // update weight
                i.Weight += item.Weight;

                container.AddRelationship<Contains>(content);
                return true;
            }
            return false;
        }

        // Returns true if content is removed from container
        public static bool Detain(Entity container, Entity content)
        {
            if (container.Has<Container, Item>() && content.Has<Item>()
                && container.HasRelationship<Contains>(content))
            {
                ref var item = ref content.Get<Item>();

                ref var c = ref container.Get<Container>();
                ref var i = ref container.Get<Item>();

                // update internal Filled
                c.Filled -= item.Fill;

                // update weight
                i.Weight -= item.Weight;

                container.RemoveRelationship<Contains>(content);
                return true;
            }
            return false;
        }

        // moving one item from its first container to another
        public static bool ReContain(Entity source, Entity dest, Entity content)
        {
            if (Detain(source, content))
                if (Contain(dest, content))
                    return true;
            return false;
        }

        public static RangedAttack ThrownAttack(Entity entity)
        {
            // TODO: Explosives?
            if (entity.Has<Item>())
            {
                var item = entity.Get<Item>();

                var range = Math.Max(6 - (int)item.Fill, 1);
                var attackFormula = new DiceExpression().Die(20).Constant(-(int)item.Fill);
                var damageFormula = new DiceExpression().Die(Math.Max((int)item.Weight * 2, 1));

                return new RangedAttack { AttackFormula = attackFormula, DamageFormula = damageFormula, Range = range };
            }

            return new RangedAttack { };
        }
    }
}
