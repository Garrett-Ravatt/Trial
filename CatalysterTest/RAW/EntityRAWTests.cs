using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Components;
using Catalyster.RAW;

namespace CatalysterTest.RAW
{
    [TestClass]
    public class EntityRAWTests
    {
        private World world;
        [TestInitialize]
        public void Initialize()
        {
            world = World.Create();
        }
        [TestCleanup]
        public void Cleanup()
        {
            world.Dispose();
        }

        [TestMethod]
        public void RAWTest1()
        {
            var def = new EntityDefinition("JANICE", "very cool lady from PTA",
                world.Create(
                    new Token { Char = '@'},
                    new Stats { Blood = 1, Body = 1, Breath = 1, Energy = 100, HP = 1 }
                    ).Reference()
                );

            var e = def.EntityReference.Entity;

            var archetype = e.GetArchetype();
            var copiedEntity = world.Create(archetype.Types);
            foreach (var c in e.GetAllComponents())
            {
                copiedEntity.Set(c);
            }
            Console.WriteLine(copiedEntity);
        }
    }
}
