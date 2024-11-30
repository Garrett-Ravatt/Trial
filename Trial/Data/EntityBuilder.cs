using Arch.Core;
using Catalyster.Components;
using Catalyster.Components.Directors;
using Catalyster.Interfaces;
using Inventory = Catalyster.Items.Inventory;
using RogueSharp.DiceNotation;
using Arch.Core.Extensions;
using Catalyster;
using Catalyster.RAW;

namespace Trial.Data
{
    //NOTE: Color values are ABGR
    public static class EntityBuilder
    {
        public static Entity Player(World world)
        {
            var rid = "ALCHYMER";
            var gm = GameMaster.Instance();
            var stats = gm.Stats;

            if (!stats.Has(rid))
            {
                var desc = "You";

                var e = stats.World.Create(
                    new Position { },
                    new Token { RID = rid, Char = '@', Name = "Alchymer", Color = 0xffff70ff },
                    new Stats { Body = 6, HP = 6, Blood = 6, Breath = 6 },
                    new MeleeAttack { AttackFormula = Dice.Parse("1d4-2"), DamageFormula = Dice.Parse("1") },
                    new Player { },
                    (IDirector)new PlayerDirector { },
                    new Sense { Range = 20 },
                    new Inventory(new List<EntityReference> { })
                    );

                var def = new EntityDefinition(rid, desc, e.Reference());
                stats.Define(def);
            }

            return stats.CreateIn(rid, world);
        }

        // Creatures
        public static Entity Goblin(World world)
        {
            var rid = "GOBLIN";
            var gm = GameMaster.Instance();
            var stats = gm.Stats;

            if (!stats.Has(rid))
            {
                var desc = "Named for its gob, its jaw draws wide and deep like a saucepan.";

                var e = stats.World.Create(
                    new Position { },
                    new Token { RID = rid, Char = 'g', Name = "goblin", Color = 0xff00e300 },
                    new Stats { Body = 4, HP = 3, Blood = 3, Breath = 5 },
                    new Monster { },
                    new Faction { HostileDesc = new QueryDescription().WithAll<Player>() },
                    new MeleeAttack { AttackFormula = Dice.Parse("1d8-5"), DamageFormula = Dice.Parse("1d2") },
                    (IDirector)new CrazedHunter { }
                    );

                var def = new EntityDefinition(rid, desc, e.Reference());
                stats.Define(def);
            }

            return stats.CreateIn(rid, world);
        }

        // Items
        public static Entity BlackPowder(World world)
        {
            return world.Create(
                new Position { },
                new Token { Char = 'X', Name = "Black Powder", Color = 0xff101010 },
                new Item { Fill = 0.3f, Weight = 1f },
                new Explosive {
                    Resistance = new Catalyster.Hunks.IntHunk(new int[] { 0, 1 }),
                    Potential = new Catalyster.Hunks.IntHunk(new int[] { 1, 1 })
                }
                );
        }
    }
}
