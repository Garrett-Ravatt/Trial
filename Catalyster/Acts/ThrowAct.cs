using Arch.Core;
using Arch.Core.Extensions;
using Arch.Relationships;
using Catalyster.Components;
using Catalyster.Helpers;
using Catalyster.Interfaces;
using Catalyster.Items;

namespace Catalyster.Acts
{
    public class ThrowAct : IInterAct
    {
        public int Cost { get; set; } = 1000;
        public bool Resolved { get; set; } = false;
        public bool Suspended { get; set; } = false;

        public EntityReference? Acting { get; set; }
        public EntityReference? Subject { get; set; }
        int? X, Y;
        // TODO: refactor i into Entity Reference
        public ThrowAct(EntityReference? e = null, EntityReference? subject = null, int? x = null, int? y = null)
        {
            Acting = e;
            X = x; Y = y;
            Subject = subject;
        }

        public IAct Execute()
        {
            if (!Acting.HasValue || !X.HasValue || !Y.HasValue || !Subject.HasValue)
            {
                Console.Error.WriteLine($"Throw Act executed with invalid state: {this}");
                // TODO: throw error
                return this;
            }
            var (entity, x, y, item) = (Acting.Value.Entity, X.Value, Y.Value, Subject.Value);

            if (entity.Has<Player>() && !GameMaster.Instance().DungeonMap.IsInFov(x, y))
            {
                // TODO: warn player
                return this;
            }

            ref var stats = ref entity.Get<Stats>();

            // Check for a target.
            {
                Entity? bumped = null;
                if (!SpatialHelper.ClearOrAssign(x, y, ref bumped))
                {
                    // Resolve an attack attempt
                    ActionHelper.ResolveRanged(ItemPropHelper.ThrownAttack(item), bumped.Value, entity.Reference());
                }
                //TODO: Message publication
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

            stats.Energy -= WiggleHelper.Wiggle(1000, .1);
            Resolved = true;
            // TODO: Dispose of the thrown item entity and contents

            // remove from inventory
            ref var inv = ref entity.Get<Inventory>();
            if (inv.Items.Contains(item))
                inv.Items.Remove(item);
            // remove from container
            // TODO: coverage
            else if (item.Entity.HasRelationship<Contained>())
            {
                foreach ((var container, var contained) in item.Entity.GetRelationships<Contained>())
                {
                    ItemPropHelper.Detain(container, item.Entity);
                }
            }
            entity.Get<Inventory>().Items.Remove(item);

            return this;
        }

        public IInterAct Clone()
        {
            return (IInterAct) MemberwiseClone();
        }
    }
}
