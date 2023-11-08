
using Arch.Core;

namespace Catalyster.Hunks
{
    public struct IntHunk : IEquatable<IntHunk>
    {
        public int[] Array;

        public IntHunk(IEnumerable<int> array = null)
        {
            Array = array.ToArray() ?? new int[]{ };
        }

        public bool Equals(IntHunk other)
        {
            var otherArr = other.Array;
            for (int i = 0; i < Array.Length; i++)
            {
                if (Array[i] != otherArr[i])
                    return false;
            }
            return true;
        }

        public IntHunk Add(IntHunk hunk)
        {
            var build = new int[Array.Length];

            var arr = hunk.Array;
            for(int i = 0; i < Array.Length; i++)
            {
                build[i] = Math.Max(0, Array[i] + arr[i]);
            }

            return new IntHunk(build);
        }

        public static IntHunk Max(IntHunk first, IntHunk second)
        {
            var build = new int[first.Array.Length];

            var arr1 = first.Array;
            var arr2 = second.Array;

            for (int i = 0; i < arr1.Length; i++)
            {
                build[i] = Math.Max(arr1[i], arr2[i]);
            }

            return new IntHunk(build);
        }

        public bool AnyLess(IntHunk hunk)
        {
            var arr = hunk.Array;
            for (int i = 0; i < Array.Length; i++)
            {
                if (Array[i] < arr[i])
                    return true;
            }
            return false;
        }

        public bool AnyGreater(IntHunk hunk)
        {
            return hunk.AnyLess(this);
        }

        public bool AnyLessOrEqual(IntHunk hunk)
        {
            var arr = hunk.Array;
            for (int i = 0; i < Array.Length; i++)
            {
                if (Array[i] <= arr[i])
                    return true;
            }
            return false;
        }

        public bool AnyGreaterOrEqual(IntHunk hunk)
        {
            return hunk.AnyLessOrEqual(this);
        }
    }
}
