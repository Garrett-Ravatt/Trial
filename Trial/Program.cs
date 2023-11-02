using Catalyster.Core;
using RogueSharp;
using Arch.Core;
using Arch.Core.Extensions;
using SadConsole.UI;
using Catalyster.Components;
using Catalyster.Models;
using Catalyster;
using System.Text.RegularExpressions;
using Trial.Consoles;

namespace Trial
{
    class Program
    {

        public static GameMaster GameMaster;
        public static DrawingMap DrawingMap;

        public static MapConsole MapConsole;
        public static MessageConsole MessageConsole;

        public static void Main(string[] args)
        {
            Settings.WindowTitle = "Trial of the Alchymer";

            Game.Create(GameSettings.Width, GameSettings.Height);
            Game.Instance.OnStart = Startup;

            Game.Instance.Run();
            Game.Instance.Dispose();
        }

        static void Startup()
        {
            // Map
            var mapModel = new Model<DungeonMap>()
                .Step(new InitializeMap(GameSettings.MapWidth, GameSettings.MapHeight))
                .Step(new RoomGen(10, 7, 15))
                .Step(new CorridorGen())
                .Seed(0xfab); // necessary until player is better placed

            var map = new DrawingMap();
            mapModel.Process(map);
            
            GameMaster = new GameMaster(map);
            DrawingMap = map;

            // TODO: World Model
            var worldModel = new Model<World>()
                .Step(new POIGen(map))
                .Step(new POIGoblin(0.5))
                .Step(new POIPlant());
            worldModel.Process(GameMaster.World);

            // Enemy
            //var gob = EntityBuilder.Goblin(GameMaster.World);
            //gob.Set<Position>(new Position { X = 10, Y = 15 });

            // Player
            var player = EntityBuilder.Player(GameMaster.World);
            player.Set<Position>(new Position { X = 5, Y = 14 });

            // SadConsole
            ScreenObject container = new ScreenObject();
            Game.Instance.Screen = container;
            MapConsole = new MapConsole(GameSettings.MapWidth, GameSettings.MapHeight);
            
            
            // Focused to handle input.
            MapConsole.IsFocused = true;
            container.Children.Add(MapConsole);

            MessageConsole = new MessageConsole();
            container.Children.Add(MessageConsole);

            Game.Instance.DestroyDefaultStartingConsole();

            GameMaster.Update();
            GameMaster.Update();
            Draw();
        }
        
        public static void Draw()
        {
            DrawingMap.UpdateFieldOfView(GameMaster.World);
            DrawingMap.DrawTo(MapConsole);
            GameMaster.World.InlineQuery<TokenUpdate, Position, Token>(in new QueryDescription().WithAll<Token, Position>());
        }
    }
}

