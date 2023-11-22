using Arch.Core;
using Arch.Core.Extensions;
using Arch.Relationships;
using Catalyster.Components;
using RogueSharp.DiceNotation;

namespace Catalyster.Helpers
{
    public static class ItemPropHelper
    {
        // TODO: Calculation for container's 

        // try to put something in a container.
        // returns true if it could put the content in the container
        public static bool Contain(Entity container, Entity content)
        {
            if (container.Has<Container, Item>() && content.Has<Item>())
            {
                var item = content.Get<Item>();

                ref var c = ref container.Get<Container>();
                ref var i = ref container.Get<Item>();

                // If it can't fit, don't
                if ((c.FillCap - c.Filled) < item.Fill)
                    return false;

                // update internal Filled and weight
                c.Filled += item.Fill;
                // TODO: update the weight of parent containers
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
            if (Contain(dest, content))
                if (Detain(source, content))
                    return true;
            return false;
        }

        // For thrown weapons. Explosions are a different item property.
        public static RangedAttack ThrownAttack(Entity entity)
        {
            if (entity.Has<RangedAttack>())
                return entity.Get<RangedAttack>();

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

        // Represent an entity's item properties as a string
        public static string StringifyItem(Entity entity)
        {
            var bits = new List<string>();

            if (entity.Has<Token>())
                bits.Add($"{entity.Get<Token>().Name},");
            else
                bits.Add($"Chunk,"); // A chunk is the most basic form of matter

            if (entity.Has<Item>())
            {
                var item = entity.Get<Item>();
                bits.Add($"[ Fill: {item.Fill}, " +
                    $"Weight: {item.Weight} ]");
            }

            if (entity.Has<Container>())
            {
                var con = entity.Get<Container>();
                bits.Add($"[ Space: {con.FillCap - con.Filled} ]");
            }

            // TODO: Show Contained items

            return string.Join(' ', bits);
        }

        // Get explosive properties of item (damage from the explosives inside)
        public static DiceExpression BombOf(Entity container)
        {
            var explosives = new List<Explosive>();
            foreach (var (content, relationship) in container.GetRelationships<Contains>())
            {
                if (content.Has<Explosive>())
                    explosives.Add(content.Get<Explosive>());
            }

            return DetonationHelper.DamageDice(explosives);
        }
    }
}
