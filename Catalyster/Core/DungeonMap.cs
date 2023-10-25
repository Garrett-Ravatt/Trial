using RogueSharp;
using Arch.Core;
using Catalyster.Components;

namespace Catalyster.Core
{
    public class DungeonMap : Map
    {
        public List<Rectangle> Rooms = new List<Rectangle>();
        //TODO: refactor 'world' parameter using static world or event system.
        public void UpdateFieldOfView(World world)
        {
            world.Query(in new QueryDescription().WithAll<Player, Position, Sense>(), (ref Position position, ref Sense sense) =>
            {
                foreach (Cell cell in GetAllCells())
                {
                    ComputeFov(position.X, position.Y, sense.Range, true );
                    if (IsInFov(cell.X, cell.Y))
                    {
                        SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
                    }
                }
            });
        }

        // Used for testing walking around.
        // TODO: as model step.
        public void SetAllWalkable()
        {
            for(int x = 0; x < Width; x++)
            {
                for(int y = 0; y < Height; y++)
                {
                    SetCellProperties(x, y, true, true );
                }
            }
        }
    }
}
