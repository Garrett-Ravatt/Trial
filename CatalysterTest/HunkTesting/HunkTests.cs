using Catalyster.Hunks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalysterTest.HunkTesting
{
    [TestClass]
    public class HunkTests
    {
        [TestMethod]
        public void HunkTest1()
        {
            var hunk1 = new IntHunk(new int[] { 1, 2 });
            var hunk2 = new IntHunk(new int[] { 2, 3 });

            var hunk3 = hunk1.Add(hunk2);

            var check = new int[] { 3, 5 };
            for (int i = 0; i < hunk3.Array.Length; i++)
                Assert.AreEqual(hunk3.Array[i], check[i]);
        }

        [TestMethod]
        public void HunkTest2()
        {
            var hunk1 = new IntHunk(new int[] { 1, 2 });
            var hunk2 = new IntHunk(new int[] { 2, 3 });

            Assert.IsTrue(hunk1.AnyLess(hunk2));
            Assert.IsFalse(hunk1.AnyGreater(hunk2));
            Assert.IsTrue(hunk2.AnyGreater(hunk1));
        }

        [TestMethod]
        public void HunkTest3()
        {
            var hunk1 = new IntHunk(new int[] { 1, 2 });
            var hunk2 = new IntHunk(new int[] { 1, 2 });

            Assert.IsTrue(hunk1.Equals(hunk2));
        }

        [TestMethod]
        public void HunkTest4()
        {
            var hunk1 = new IntHunk(new int[] { 1, 2 });
            var hunk2 = new IntHunk(new int[] { 1, 2 });
            var hunk3 = new IntHunk(new int[] { 2, 3 });

            Assert.IsTrue(hunk1.AnyGreaterOrEqual(hunk2));
            Assert.IsTrue(hunk1.AnyLessOrEqual(hunk2));
            Assert.IsFalse(hunk1.AnyGreaterOrEqual(hunk3));
        }

        [TestMethod]
        public void HunkTest5()
        {
            var hunk1 = new IntHunk(new int[] { 1, 2 });
            var hunk2 = new IntHunk(new int[] { 2, 1 });
            
            var expected = new IntHunk(new int[] { 2, 2 });

            Assert.IsTrue(IntHunk.Max(hunk1, hunk2).Equals(expected));
        }
    }
}
