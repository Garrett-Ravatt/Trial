using Arch.Core;
using Catalyster.Acts;
using Catalyster.Core;
using TinyMessenger;

namespace Catalyster
{
    public sealed class GameMaster
    {
        public World World { get; private set; }
        public EntityStats Stats { get; private set; }
        public DungeonMap DungeonMap { get; set; }

        public Command Command;
        private TurnOrder _turnOrder;

        public MessageLog MessageLog { get; private set; }

        public static GameMaster _gameMaster;
        public static GameMaster Instance()
        {
            if (_gameMaster == null)
                _gameMaster = new GameMaster();
            return _gameMaster;
        }
        public void Reset()
        {
            World.Dispose();
            Stats.World.Dispose();
            // TODO: Wonder about if this should be here
            CommandInjectionAct.InjectedAct = null;
            _gameMaster = new GameMaster();
        }

        private GameMaster()
        {
            DungeonMap = new DungeonMap();
            World = World.Create();
            Stats = new EntityStats();
            Command = new Command();
            _turnOrder = new TurnOrder();
            MessageLog = new MessageLog();
        }

        public void Update()
        {
            // NOTE: May be refactored to use Arch.Extended's Helpers
            // once more than TurnOrder needs to be updated.
            Command.CheckEnergy();
            if (_turnOrder.Resolve())
            {
                Command.Entity = _turnOrder.Update(World);
            }
        }

        // Maximum updates done to GameMaster before exiting
        const int UPDATE_LIMIT = 15;
        public void Resolve()
        {
            // TODO: if finished updating, stop updating
            for (var i = 0; i < UPDATE_LIMIT; i++)
            {
                Update();
            }
        }
    }
}
