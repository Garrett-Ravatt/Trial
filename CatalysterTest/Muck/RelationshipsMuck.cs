using Arch.Core;
using Arch.Relationships;

namespace CatalysterTest.Muck
{
    [TestClass]
    internal class RelationshipsMuck
    {
        internal struct Mama { }

        [TestMethod]
        public void TestParenthood()
        {
            var world = World.Create();

            var ma = world.Create();
            var bab = world.Create();
            var cab = world.Create();

            ma.AddRelationship<Mama>(bab);
            ma.AddRelationship<Mama>(cab);

            Assert.IsTrue(ma.HasRelationship<Mama>(bab));

            ref var parentOfRelation = ref ma.GetRelationships<Mama>();
            foreach (var (child, relation) in parentOfRelation)
            {
                Assert.IsTrue(child.Equals(cab) || child.Equals(bab));
            }

            world.Dispose();
        }
    }
}
