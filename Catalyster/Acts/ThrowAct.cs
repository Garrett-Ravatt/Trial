using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Components;
using Catalyster.Helpers;
using Catalyster.Interfaces;
using Catalyster.Items;

namespace Catalyster.Acts
{
    public class ThrowAct : IAct
    {
        public int Cost { get; set; } = 1000;

        public EntityReference? EntityRef;
        int? X, Y, I;
        public ThrowAct(EntityReference? e = null, int? x = null, int? y = null, int? i = null)
        {
            EntityRef = e;
            X = x; Y = y; I = i;
        }

        public bool Execute()
        {
            if (!EntityRef.HasValue || !X.HasValue || !Y.HasValue || !I.HasValue)
                return false;
            var (entity, x, y, i) = (EntityRef.Value.Entity, X.Value, Y.Value, I.Value);

            if (entity.Has<Player>() && !GameMaster.DungeonMap.IsInFov(x, y))
                return false;

            ref var energy = ref entity.Get<Energy>();

            var item = entity.Get<Inventory>().Items[i];
            if (item == null)
            {
                Console.WriteLine($"Invalid item index {i} was selected");
                return false;
            }

            // Check for a target.
            {
                Entity? bumped = null;
                if (!SpatialHelper.ClearOrAssign(x, y, ref bumped))
                {
                    // Resolve an attack attempt
                    ActionHelper.ResolveRanged(ItemPropHelper.ThrownAttack(item), bumped.Value, entity.Reference());
                }
                item.Entity.Add(new Position { X = x, Y = y });
            }

            // Resolve an explosion
            // TODO: SpatialHash refactor point
            // TODO: refactor explosion resolution into helper somewhere
            var bomb = ItemPropHelper.BombOf(item);
            var bombFormula = bomb.DamageFormula;
            if (bombFormula.MinRoll().Value > 0)
            {
                var radius = bomb.Range;
                for (var _x = x - radius; _x <= x + radius; _x++)
                {
                    for (var _y = y - radius; _y <= y + radius; _y++)
                    {
                        // Check for a target.
                        Entity? bumped = null;
                        if (!SpatialHelper.ClearOrAssign(_x, _y, ref bumped))
                        {
                            // TODO: Some other method than this
                            var rangedAttack = new RangedAttack
                            {
                                Range = 6,
                                AttackFormula = RogueSharp.DiceNotation.Dice.Parse("20"),
                                DamageFormula = bombFormula
                            };
                            // Resolve an attack attempt
                            ActionHelper.ResolveRanged(rangedAttack, bumped.Value, attacker: item);
                        }
                    }
                }
            }

            energy.Points -= WiggleHelper.Wiggle(1000, .1);
            // TODO: Dispose of the thrown item entity and contents
            entity.Get<Inventory>().Items.RemoveAt(i);

            return true;
        }
    }
}
