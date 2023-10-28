using Arch.Core;
using Catalyster;
using Catalyster.Components;
using Catalyster.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalysterTest
{
    [TestClass]
    public class SpatialTest
    {
        [TestMethod]
        public void SpatialHelperTest1()
        {
            var pos1 = new Position { X = 0, Y = 0 };
            var pos2 = new Position { X = 1, Y = 1 };
            var pos3 = new Position { X = 1, Y = 0 };

            var dist1 = SpatialHelper.LazyDist(pos1, pos2);
            var dist2 = SpatialHelper.LazyDist(pos1, pos3);

            Assert.IsTrue(dist1 <= 1);
            Assert.IsTrue(dist2 <= 1);
        }

        [TestMethod]
        public void SpatialHelperTest2()
        {
            new GameMaster();
            var world = GameMaster.World;

            var position = new Position { X=0, Y=0 };

            // is clear before being added
            Assert.IsTrue(SpatialHelper.IsClear(0,0));

            // isn't clear after being added
            world.Create(position);
            Assert.IsFalse(SpatialHelper.IsClear(0, 0));

            GameMaster.World.Dispose();
        }

        [TestMethod]
        public void SpatialHelperTest3()
        {
            new GameMaster();
            var world = GameMaster.World;

            var position = new Position { X = 0, Y = 0 };

            Entity? holder = null;

            // can't find anything before being added
            Assert.IsTrue(SpatialHelper.ClearOrAssign(0,0,ref holder));

            // found it and returned it once added
            world.Create(position);
            Assert.IsFalse(SpatialHelper.ClearOrAssign(0,0, ref holder));

            GameMaster.World.Dispose();
        }
    }
}
