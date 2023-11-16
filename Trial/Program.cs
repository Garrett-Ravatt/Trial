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
using Trial.Data;

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

            // Entity generation
            var worldModel = new Model<World>()
                .Step(new POIGen(map))
                .Step(new POIPlayer())
                .Step(new POIGoblin(1.0))
                .Step(new BlackPowderWrite(map))
                .Seed(0xfab);
            worldModel.Process(GameMaster.World);

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

