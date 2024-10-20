using Arch.Core;
using Catalyster.Core;

namespace Catalyster
{
    public class GameMaster
    {
        public static World World { get; private set; }
        public static DungeonMap DungeonMap { get; private set; }

        public Command Command;
        private TurnOrder _turnOrder;

        public static MessageLog MessageLog { get; private set; }

        // TODO: Consolidate constructors
        public GameMaster(DungeonMap dungeonMap)
        {
            DungeonMap = dungeonMap;
            World = World.Create();
            Command = new Command();
            _turnOrder = new TurnOrder();
            MessageLog = new MessageLog();
        }

        public GameMaster()
        {
            DungeonMap = new DungeonMap();
            World = World.Create();
            Command = new Command();
            _turnOrder = new TurnOrder();
            MessageLog = new MessageLog();
        }

        public void Update()
        {
            // NOTE: May be refactored to use Arch.Extended's Helpers
            // once more than TurnOrder needs to be updated.
            if (_turnOrder.Resolve())
            {
                if (!_turnOrder.PlayerLock)
                {
                    Command.Entity = _turnOrder.Update(World);
                }
                else if (Command.Entity == null)
                {
                    _turnOrder.PlayerLock = false;
                    Command.Entity = _turnOrder.Update(World);
                }
            }
        }
    }
}
