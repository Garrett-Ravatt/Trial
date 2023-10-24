using Catalyster.Components;
using Arch.Core;

namespace Catalyster.Helpers
{
    public class SpatialHelper
    {
        // Very innacurate distance calculation.
        public static int LazyDist(Position pos1,  Position pos2)
        {
            var x = Math.Abs(pos1.X - pos2.X);
            var y = Math.Abs(pos1.Y - pos2.Y);
            return Math.Max(x, y);
        }

        public static bool IsClear(Position pos)
        {
            var isClear = true;
            GameMaster.World.Query(in new QueryDescription().WithAll<Position>(), (ref Position position) =>
            {
                if (position.Equals(pos)) isClear = false;
            });
            return isClear;
        }
    }
}
