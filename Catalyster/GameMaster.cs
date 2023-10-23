using Arch.Core;
using Arch.Core.Extensions;
using Arch.System;
using Catalyster.Components;
using Catalyster.Core;
using Catalyster.Systems;
using System.Runtime.CompilerServices;

namespace Catalyster
{
    public class GameMaster
    {
        public static World World { get; private set; }
        public static DungeonMap DungeonMap { get; private set; }

        public Command Control;
        private TurnOrder _turnOrder;

        public GameMaster(DungeonMap dungeonMap)
        {
            DungeonMap = dungeonMap;
            World = World.Create();
            Control = new Command();
            _turnOrder = new TurnOrder();
        }

        public GameMaster()
        {
            DungeonMap = new DungeonMap();
            World = World.Create();
            Control = new Command();
            _turnOrder = new TurnOrder();
        }

        public void Update()
        {
            // NOTE: May be refactored to use Arch.Extended's Systems
            // once more than TurnOrder needs to be updated.
            // TODO: use event bus instead.
            Control.Entity = _turnOrder.Update(World);
        }
    }
}
