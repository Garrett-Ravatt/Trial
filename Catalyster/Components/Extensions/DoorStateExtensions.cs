using Arch.Core;
using Arch.Core.Extensions;

namespace Catalyster.Components.Extensions
{
    public static class DoorStateExtensions
    {
        public static void SetUpdate(ref this DoorState state, DoorState value, Position p)
        {
            state = value;
            state.UpdateMap(p);
        }

        public static void UpdateMap(this DoorState state, Position p)
        {
            var gm = GameMaster.Instance();
            
            switch (state)
            {
                case DoorState.OPEN:
                    gm.DungeonMap.SetCellProperties(p.X, p.Y, true, true);
                    break;
                case DoorState.CLOSED:
                    gm.DungeonMap.SetCellProperties(p.X, p.Y, false, false);
                    break;
            }
        }
    }
}
