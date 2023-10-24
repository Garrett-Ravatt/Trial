using Arch.Core;
using Catalyster.Components;
using Catalyster.Interfaces;
using RogueSharp.DiceNotation;

namespace CatalysterTest.TestUtils
{
    public class ExFactory
    {
        public static Entity SimpleCreature(World world)
        {
            return world.Create(
                new Token { Char = 'c', Name = "Simple Creature", Color = 0xffffffff },
                new Position { X = 0, Y = 0 },
                new Health { Points = 5, Max = 5 },
                new Defense { Class = 0 },
                new Energy { Max = 1000, Points = 1000, Regen = 1000 },
                new MeleeAttack { AttackFormula = Dice.Parse("1d20+2"), DamageFormula = Dice.Parse("1d4") },
                (IDirector)new MonoBehavior { Directive = new RightMover { } }
                );
        }

        public static Entity Player(World world)
        {
            return world.Create(
                new Token { Char = '@', Name = "Player", Color = 0xffffffff },
                new Position { X = 0, Y = 0 },
                new Health { Points = 5, Max = 5 },
                new Defense { Class = 0 },
                new Energy { Max = 1000, Points = 1000, Regen = 1000 },
                new MeleeAttack { AttackFormula = Dice.Parse("1d20+2"), DamageFormula = Dice.Parse("1d4") },
                new Player { }
                );
        }
    }
}
