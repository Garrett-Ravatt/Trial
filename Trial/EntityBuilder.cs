using Arch.Core;
using Catalyster.Components;
using Catalyster.Interfaces;
using RogueSharp.DiceNotation;

namespace Trial
{
    //NOTE: Color values are ABGR
    public static class EntityBuilder
    {
        public static Entity Goblin(World world)
        {
            var hp = Math.Max(Dice.Roll("2d4-2"), 1);
            return world.Create(
                new Position { X=8, Y=8 },
                new Token { Char='g', Name="goblin", Color=0xff00e300},
                new Health { Max = hp, Points = hp },
                new Defense { Class = 10 },
                new Energy { Max = 1000, Points = 1000, Regen = 1000 },
                new MeleeAttack { AttackFormula = Dice.Parse("1d20+2"), DamageFormula = Dice.Parse("1d4") }
                //(IDirector) new MonoBehavior { Directive = new RightMover { Cost = 1000 } }
                );
        }

        public static Entity Player( World world )
        {
            return world.Create(
                new Player { },
                new Position { X = 5, Y = 5 },
                new Token { Char='@', Name="Alchymer", Color=0xffff70ff},
                new Sense { Range = 20 },
                new Health { Max = 10, Points = 10 },
                new Defense { Class = 0 },
                new Energy { Max = 1000, Points = 1000, Regen = 1000 },
                new MeleeAttack { AttackFormula = Dice.Parse("1d20+3"), DamageFormula = Dice.Parse("1d3+1") }
                );
        }
    }
}
