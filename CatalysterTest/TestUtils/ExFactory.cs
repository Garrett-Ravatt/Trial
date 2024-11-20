using Arch.Core;
using Catalyster.Components;
using Catalyster.Components.Directives;
using Catalyster.Components.Directors;
using Catalyster.Interfaces;
using Inventory = Catalyster.Items.Inventory;
using RogueSharp.DiceNotation;
using Arch.Relationships;
using Arch.Core.Extensions;
using Catalyster.Helpers;

namespace CatalysterTest.TestUtils
{
    public class ExFactory
    {
        // NOTE: no director by default
        public static Entity SimpleCreature(World world)
        {
            return world.Create(
                new Token { Char = 'c', Name = "Simple Creature", Color = 0xffffffff },
                new Position { X = 0, Y = 0 },
                new Stats { Body = 2, HP = 30, Blood = 30, Breath = 10, Energy = 1000 },
                new MeleeAttack { AttackFormula = Dice.Parse("1d3"), DamageFormula = Dice.Parse("1d4") },
                new Monster { },
                new Faction { HostileDesc = new QueryDescription().WithAll<Player>() },
                (IDirector) new MonoBehavior { Directive = new RightMover { } }
                );
        }

        public static Entity Player(World world)
        {
            return world.Create(
                new Token { Char = '@', Name = "Player", Color = 0xffffffff },
                new Position { X = 0, Y = 0 },
                new Stats { Body = 1, HP = 30, Blood = 30, Breath = 10, Energy = 1000 },
                new MeleeAttack { AttackFormula = Dice.Parse("1d3"), DamageFormula = Dice.Parse("1d4") },
                new Sense { Range=5 },
                new Inventory(),
                new Player { },
                (IDirector) new PlayerDirector { }
                );
        }

        public static Entity BlackPowder(World world)
        {
            return world.Create(
                new Position { },
                new Token { Char = 'X', Name = "Black Powder", Color = 0xff101010 },
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
    }
}
