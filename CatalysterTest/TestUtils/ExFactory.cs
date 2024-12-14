using Arch.Core;
using Catalyster.Components;
using Catalyster.Components.Directives;
using Catalyster.Components.Directors;
using Catalyster.Interfaces;
using Inventory = Catalyster.Items.Inventory;
using RogueSharp.DiceNotation;
using Arch.Core.Extensions;
using Catalyster.Helpers;
using Catalyster.Acts.Interactive;

namespace CatalysterTest.TestUtils
{
    public class ExFactory
    {
        // Simple Creature that loves walking right
        public static Entity SimpleCreature(World world)
        {
            var c = world.Create(
                new Token { RID = "SIMPLE_CREATURE", Char = 'c', Name = "Simple Creature", Color = 0xffffffff },
                new Position { X = 0, Y = 0 },
                new Stats { Body = 2, HP = 30, Blood = 30, Breath = 10, Energy = 1000 },
                new MeleeAttack { AttackFormula = Dice.Parse("1d3"), DamageFormula = Dice.Parse("1d4") },
                new Monster { },
                new Faction { HostileDesc = new QueryDescription().WithAll<Player>() },
                (IDirector) new MonoBehavior { Directive = new RightMover { } }
                );
            return c;
        }

        public static Entity Player(World world)
        {
            return world.Create(
                new Token { RID = "PLAYER", Char = '@', Name = "Player", Color = 0xffffffff },
                new Position { X = 0, Y = 0 },
                new Stats { Body = 1, HP = 30, Blood = 30, Breath = 10, Energy = 1000 },
                new MeleeAttack { AttackFormula = Dice.Parse("1d3"), DamageFormula = Dice.Parse("1d4") },
                new Sense { Range=5 },
                new Inventory() { WeightCapacity = 6f, FillCapacity = 6f },
                new Player { },
                (IDirector) new PlayerDirector { }
                );
        }

        public static Entity BlackPowder(World world)
        {
            return world.Create(
                new Position { },
                new Token { RID = "BLACK_POWDER", Char = 'X', Name = "Black Powder", Color = 0xff101010 },
                new Item { Fill = 0.3f, Weight = 1f },
                new Explosive
                {
                    Resistance = new Catalyster.Hunks.IntHunk(new int[] { 0, 1 }),
                    Potential = new Catalyster.Hunks.IntHunk(new int[] { 1, 1 })
                }
                );
        }

        public static Entity BasicBomb(World world)
        {
            // Bomb has no RID because it's a container and thus can't be fabricated.
            var bomb = world.Create(
                new Position { },
                new Token { Name = "Bomb" },
                new Item { Fill = 0.3f, Weight = 1f },
                new Container { FillCap = 10f }
            );
            var powder = BlackPowder(world);
            powder.Remove<Position>();
            if (!ItemPropHelper.Contain(bomb, powder))
                Console.WriteLine("Exfactory.BasicBomb is experiencing issues");
            return bomb;
        }

        public static Entity Door(World world)
        {
            return world.Create(
                new Token { RID = "DOOR", Char = '+', Name = "Door", Color = 0xa0522dff },
                new Position { },
                new Door { state = DoorState.CLOSED },
                new InterAct { act = new UseDoorAct(null, null) }
                );
        }
    }
}
