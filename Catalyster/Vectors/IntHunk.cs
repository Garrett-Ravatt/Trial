
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
    }
}
