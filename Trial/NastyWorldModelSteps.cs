using Arch.CommandBuffer;
using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Components;
using Catalyster.Interfaces;
using Catalyster.Models;
using RogueSharp.DiceNotation;

namespace Trial
{
    // TODO: This very much should not be structured like this

    public class POIGoblin : POIOverwrite
    {
        public POIGoblin(double p = 1) : base(p) { }
        public override void AddOn(CommandBuffer buffer, Entity entity)
        {
            var hp = Math.Max(Dice.Roll("2d4-2"), 1);
            buffer.Add( in entity,
                new Token { Char = 'g', Name = "goblin", Color = 0xff00e300 },
                new Health { Max = hp, Points = hp },
                new Defense { Class = 10 },
                new Energy { Max = 1000, Points = 1000, Regen = 1000 },
                new Monster { },
                new Faction { HostileDesc = new QueryDescription().WithAll<Player>() },
                new MeleeAttack { AttackFormula = Dice.Parse("1d20+2"), DamageFormula = Dice.Parse("1d4") },
                (IDirector)new CrazedHunter { }
                );
        }
    }

    public class POIPlant : POIOverwrite
    {
        public override void AddOn(CommandBuffer buffer, Entity entity)
        {
            buffer.Add( in entity,
                new Token { Char = 'p', Name = "Plant", Color = 0xff00d000 });
        }
    }
}
