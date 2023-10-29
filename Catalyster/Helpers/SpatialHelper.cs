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

        // returns true if tile is clear of entities
        public static bool IsClear(int x, int y)
        {
            var isClear = true;
            GameMaster.World.Query(in new QueryDescription().WithAll<Position>(), (ref Position position) =>
            {
                if (position.X == x && position.Y == y) isClear = false;
            });
            return isClear;
        }

        // tries to find something. Isn't particular about what.
        public static bool ClearOrAssign(int x, int y, ref Entity? foundEntity)
        {
            var isClear = true;
            Entity? temp = foundEntity;
            GameMaster.World.Query(in new QueryDescription().WithAll<Position>(), (Entity entity, ref Position position) =>
            {
                if (position.X == x && position.Y == y)
                {
                    temp = entity;
                    isClear = false;
                }
            });

            foundEntity = temp;
            return isClear;
        }
    }
}
