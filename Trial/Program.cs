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

        public static Console StartingConsole;
        public static ConsoleMap Map;

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
            //Map
            var model = new Model<DungeonMap>()
                .Step(new InitializeMap(Width, Height))
                .Step(new RoomGen(10, 15, 7))
                .Step(new CorridorGen())
                .Seed(0xfab); // necessary until player is better placed

            Map = new ConsoleMap();
            model.Process(Map);
            
            var gm = new GameMaster(Map);

            //Enemy
            var gob = EntityBuilder.Goblin(GameMaster.World);
            gob.Set<Position>(new Position { X = 10, Y = 15 });

            //Player
            var player = EntityBuilder.Player(GameMaster.World);
            player.Set<Position>(new Position { X = 5, Y = 14 });

            StartingConsole = (Console)GameHost.Instance.Screen;

            Draw();
        }
        
        public static void Draw()
        {
            Map.UpdateFieldOfView(GameMaster.World);
            Map.DrawTo(StartingConsole);

            GameMaster.World.Query(in new QueryDescription().WithAll<Position, Token>(), (ref Position position, ref Token token) =>
            {
                StartingConsole.SetGlyph(position.X, position.Y, token.Char);
                StartingConsole.SetForeground(position.X, position.Y, new Color(token.Color));
            });
        }
    }
    
}

