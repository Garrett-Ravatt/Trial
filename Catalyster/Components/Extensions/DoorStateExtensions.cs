using Arch.Core;
using Arch.Core.Extensions;

namespace Catalyster.Components.Extensions
{
    public static class DoorStateExtensions
    {
        public static void UpdateMap(this DoorState state, Entity entity)
        {
            var gm = GameMaster.Instance();
            Position p;

            if (!entity.TryGet(out p))
                return;

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
