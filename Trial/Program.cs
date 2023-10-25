using Catalyster.Core;
using RogueSharp;
using Arch.Core;
using Arch.Core.Extensions;
using SadConsole.UI;
using Catalyster.Components;
using Catalyster.Models;
using Catalyster;

namespace Trial
{
    class Program
    {
        public const int Width = 80;
        public const int Height = 25;

        public static GameMaster GameMaster;
        public static DrawingMap DrawingMap;

        public static MapConsole MapConsole;

        public static void Main(string[] args)
        {
            Settings.WindowTitle = "Trial of the Alchymer";

            Game.Create(90, 30);
            Game.Instance.OnStart = Startup;

            Game.Instance.Run();
            Game.Instance.Dispose();
        }

        static void Startup()
        {
            // Map
            var model = new Model<DungeonMap>()
                .Step(new InitializeMap(Width, Height))
                .Step(new RoomGen(10, 15, 7))
                .Step(new CorridorGen())
                .Seed(0xfab); // necessary until player is better placed

            var map = new DrawingMap();
            model.Process(map);
            
            GameMaster = new GameMaster(map);
            DrawingMap = map;

            // Enemy
            var gob = EntityBuilder.Goblin(GameMaster.World);
            gob.Set<Position>(new Position { X = 10, Y = 15 });

            // Player
            var player = EntityBuilder.Player(GameMaster.World);
            player.Set<Position>(new Position { X = 5, Y = 14 });

            // SadConsole
            ScreenObject container = new ScreenObject();
            Game.Instance.Screen = container;
            MapConsole = new MapConsole(Width, Height);
            
            // Focused to handle input.
            MapConsole.IsFocused = true;
            container.Children.Add(MapConsole);

            Game.Instance.DestroyDefaultStartingConsole();

            GameMaster.Update();
            GameMaster.Update();
            Draw();
        }
        
        public static void Draw()
        {
            DrawingMap.UpdateFieldOfView(GameMaster.World);
            DrawingMap.DrawTo(MapConsole);

            GameMaster.World.Query(in new QueryDescription().WithAll<Position, Token>(), (ref Position position, ref Token token) =>
            {
                MapConsole.SetGlyph(position.X, position.Y, token.Char);
                MapConsole.SetForeground(position.X, position.Y, new Color(token.Color));
            });
        }
    }
}

