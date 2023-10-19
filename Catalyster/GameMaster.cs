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
        private World _world;
        public DungeonMap DungeonMap { get; private set; }

        public int Turn {  get; private set; }
        private TurnOrder _turnOrder;

        public GameMaster()
        {
            DungeonMap = new DungeonMap();
            DungeonMap.Initialize(10, 10); // TODO: you know... make bigger

            _world = World.Create();

            Turn = 0;
        }

        public void Update()
        {
            
        }

    }
}
