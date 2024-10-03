using Arch.Core;
using Arch.Core.Extensions;
using Catalyster;
using Catalyster.Components;
using Catalyster.Helpers;
using Inventory = Catalyster.Items.Inventory;

namespace Catalyster.Core
{
    public class Command
    {
        // The Entity being controlled
        public Entity? Entity;
        public Command() { }

        // yield control away.
        public void Wait()
        {
            Entity = null;
        }

        // try to go somewhere.
        public bool Move(int X, int Y)
        {
            // TODO: Perform as Directive, or share code with a movement Directive using a Helper
            if (Entity != null)
            {
                var e = Entity.Value;
                var walkAct = new WalkAct(X, Y);
                var b = walkAct.Enter(Entity.Value, GameMaster.World);
                CheckEnergy(e.Get<Energy>().Points);
                return b;
            }
            else
            {
                Console.WriteLine("Command.Entity is null");
                return false;
            }
        }

        // Collects items at the feet or interacts with something
        public void Interact()
        {
            if (Entity == null)
                return;
            var player = Entity.Value;
            if (!player.Has<Inventory, Position>())
                return;

            var position = player.Get<Position>();
            var inv = player.Get<Inventory>();

            // Look for items, collect them
            // TODO: Move somewhere else?
            GameMaster.World.Query(
                in new QueryDescription().WithAll<Token, Position, Item>(),
                (Entity entity, ref Token token, ref Position pos, ref Item item) =>
                {
                    // TODO: SpatialHash refactor point
                    // TODO: Check inventory capacity
                    if (SpatialHelper.LazyDist(position, pos) <= 1)
                    {
                        entity.Remove<Position>(); //NOTE: potentially unsafe.
                        inv.Items.Add(entity.Reference());
                        GameMaster.MessageLog.Add($"{token.Name} Collected.");
                    }
                });
        }

        // Attempt to throw an item from inventory at a tile
        public bool Throw(int x, int y, int i)
        {
            var didThrow = false; // if true by the end, use up energy

            if (Entity == null || !GameMaster.DungeonMap.IsInFov(x, y))
                return false;

            var entity = Entity.Value;

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
                            ActionHelper.ResolveRanged(rangedAttack, bumped.Value, attacker : "Bomb");
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

            CheckEnergy(energy.Points);
            return didThrow;
        }

        // Check if the player's turn is now over.
        private void CheckEnergy(int points)
        {
            if (points <= 0)
                Entity = null;
        }

        // A method used by UI
        public List<string> Inventory()
        {
            if (Entity == null)
                return new List<string>();

            var entity = Entity.Value;
            if (!entity.Has<Inventory>())
                return new List<string>();

            var list = new List<string>();
            foreach (EntityReference item in entity.Get<Inventory>().Items)
            {
                if (item.IsAlive())
                    list.Add(ItemPropHelper.StringifyItem(item.Entity));
            }
            return list;
        }
    }
}
