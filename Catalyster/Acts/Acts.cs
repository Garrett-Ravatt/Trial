using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Components;
using Catalyster.Helpers;
using Catalyster.Interfaces;
using Catalyster.Items;

namespace Catalyster.Acts
{
    public class WalkAct : IAct
    {
        public int Cost { get; set; } = 1000;

        public int? X;
        public int? Y;

        public WalkAct(int? x = null, int? y = 0)
        {
            X = x;
            Y = y;
        }

        public bool Enter(Entity entity, World world)
        {
            if (!X.HasValue || !Y.HasValue) return false;

            var (x, y) = (X.Value, Y.Value);

            ref var energy = ref entity.Get<Energy>();

            ref var position = ref entity.Get<Position>();
            var newPos = new Position { X = position.X + x, Y = position.Y + y };

            if (GameMaster.DungeonMap.IsWalkable(newPos.X, newPos.Y))
            {
                Entity? bumped = null;
                if (SpatialHelper.ClearOrAssign(position.X + x, position.Y + y, ref bumped))
                {
                    position = newPos;
                    // TODO: refer to movement speed
                    energy.Points -= WiggleHelper.Wiggle(1000, .1);
                }

                else // ran into a creature
                {
                    ActionHelper.ResolveAttack(entity, bumped.Value);
                    // TODO: refer to attack cost
                    energy.Points -= WiggleHelper.Wiggle(1000, .1);
                }
            }

            else if (entity.Has<Player>())
            {
                GameMaster.MessageLog.Add("You bump into the wall. You fool.");
            }

            return true;
        }
    }

    public class ThrowAct : IAct
    {
        public int Cost { get; set; } = 1000;

        int? X, Y, I;
        public ThrowAct(int? x = null, int? y = null, int? i = null)
        {
            X = x; Y = y; I = i;
        }

        public bool Enter(Entity entity, World world)
        {
            if (!X.HasValue || !Y.HasValue || !I.HasValue) return false;
            var (x, y, i) = (X.Value, Y.Value, I.Value);

            var didThrow = false; // if true by the end, use up energy

            if (!GameMaster.DungeonMap.IsInFov(x, y))
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
                    didThrow = true;

                    // Resolve an attack attempt
                    ActionHelper.ResolveRanged(ItemPropHelper.ThrownAttack(item), bumped.Value);
                }
                //TODO: throw item at empty space
            }

            // Resolve an explosion
            // TODO: SpatialHash refactor point
            // TODO: refactor explosion resolution into helper somewhere
            var bomb = ItemPropHelper.BombOf(item);
            var bombFormula = bomb.DamageFormula;
            if (bombFormula.MinRoll().Value > 0)
            {
                didThrow = true;
                var radius = bomb.Range;
                for (var _x = x - radius; _x <= x + radius; _x++)
                {
                    for (var _y = y - radius; _y <= y + radius; _y++)
                    {
                        // Check for a target.
                        Entity? bumped = null;
                        if (!SpatialHelper.ClearOrAssign(_x, _y, ref bumped))
                        {
                            energy.Points -= WiggleHelper.Wiggle(1000, .1);

                            // TODO: Some other method than this
                            var rangedAttack = new RangedAttack
                            {
                                Range = 6,
                                AttackFormula = RogueSharp.DiceNotation.Dice.Parse("20"),
                                DamageFormula = bombFormula
                            };
                            // Resolve an attack attempt
                            ActionHelper.ResolveRanged(rangedAttack, bumped.Value, attacker: "Bomb");
                        }
                    }
                }
            }

            if (didThrow)
            {
                energy.Points -= WiggleHelper.Wiggle(1000, .1);
                // TODO: Dispose of the thrown item entity and contents
                entity.Get<Inventory>().Items.RemoveAt(i);
            }

            return didThrow;
        }
    }
}
