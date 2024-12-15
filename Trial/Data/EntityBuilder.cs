using Arch.Core;
using Catalyster.Components;
using Catalyster.Components.Directors;
using Catalyster.Interfaces;
using Inventory = Catalyster.Items.Inventory;
using RogueSharp.DiceNotation;
using Arch.Core.Extensions;
using Catalyster;
using Catalyster.RAW;
using Catalyster.Helpers;
using Catalyster.Acts.Interactive;

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

            //return stats.CreateIn(rid, world);
            var player = stats.CreateIn(rid, world);

            // DEBUG
            // TODO: delete section
            var jug = world.Create(
                new Token { Name = "Jug" },
                new Item { Fill = 8f, Weight = 2f },
                new Container { FillCap = 8f }
                );

            var bottle = world.Create(
                new Token { Name = "Bottle" },
                new Item { Fill = 2f, Weight = 1f },
                new Container { FillCap = 2f }
                );

            var phial = world.Create(
                new Token { Name = "Phial" },
                new Item { Fill = .5f, Weight = .5f },
                new Container { FillCap = 0.5f }
                );

            var pebble = world.Create(
                new Token { Name = "Pebble" },
                new Item { Fill = .1f, Weight = .1f }
                );

            ItemPropHelper.Contain(jug, bottle);
            ItemPropHelper.Contain(bottle, phial);
            ItemPropHelper.Contain(phial, pebble);

            player.Get<Inventory>().Items.Add(jug.Reference());
            player.Get<Inventory>().CalculateCapacity();

            return player;
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

        // Interactives
        public static Entity Door(World world)
        {
            var rid = "DOOR";
            var gm = GameMaster.Instance();
            var stats = gm.Stats;

            if (!stats.Has(rid))
            {
                var desc = "A plain wooden door.";

                var e = stats.World.Create(
                new Token { RID = "DOOR", Char = '+', Name = "Door", Color = 0xa0522dff },
                new Position { },
                new Door { state = DoorState.CLOSED },
                new InterAct { act = new UseDoorAct() }
                );

                var def = new EntityDefinition(rid, desc, e.Reference());
                stats.Define(def);
            }

            return stats.CreateIn(rid, world);
        }
    }
}
