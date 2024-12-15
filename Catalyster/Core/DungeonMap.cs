using RogueSharp;
using Arch.Core;
using Catalyster.Components;
using Catalyster.Components.Extensions;

namespace Catalyster.Core
{
    public class DungeonMap : Map
    {
        public List<Rectangle> Rooms = new List<Rectangle>();

        //TODO: refactor 'world' parameter using static world or event system.
        public void UpdateFieldOfView(World world)
        {
            // Block view through doors
            world.Query(in new QueryDescription().WithAll<Position, Door>(), (ref Position pos, ref Door door) =>
            {
                var cell = GetCell(pos.X, pos.Y);
                door.state.UpdateMap(pos);
            });

            // Print visible tokens
            world.Query(in new QueryDescription().WithAll<Player, Position, Sense>(), (ref Position position, ref Sense sense) =>
            {
                // TODO: compute fov cumulatively instead of destructively .
                ComputeFov(position.X, position.Y, sense.Range, true);
                foreach (Cell cell in GetAllCells())
                {
                    if (IsInFov(cell.X, cell.Y))
                    {
                        SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
                    }
                }
            });
        }

        // Used for testing walking around.
        // TODO: just replace with Clear(). Method is redundant.
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
