using Catalyster.Core;
using RogueSharp;
using Arch.Core;
using Arch.Core.Extensions;
using SadConsole.UI;
using Catalyster.Components;
using Catalyster.Models;

namespace Trial
{
    class Program
    {
        public const int Width = 80;
        public const int Height = 25;

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
            // TODO: Refactor to use Catalyster's GameMaster.

            //Map
            var model = new Model<DungeonMap>()
                .Step(new InitializeMap(Width, Height))
                .Step(new RoomGen(10, 15, 7))
                .Step(new CorridorGen())
                .Seed(0xfab); // necessary until player is better placed

            var map = new ConsoleMap();
            model.Process(map);
            
            var world = World.Create(); // Catalyster should do this for us as well.

            //Enemy
            var gob = EntityBuilder.Goblin(world);
            gob.Set<Position>(new Position { X = 10, Y = 15 });

            //Player
            var player = EntityBuilder.Player(world);
            player.Set<Position>(new Position { X = 5, Y = 14 });

            var startingConsole = (Console)GameHost.Instance.Screen;

            map.UpdateFieldOfView(world);
            map.DrawTo(startingConsole);

            world.Query(in new QueryDescription().WithAll<Position, Token>(), (ref Position position, ref Token token) =>
            {
                startingConsole.SetGlyph(position.X, position.Y, token.Char);
                startingConsole.SetForeground(position.X, position.Y, new Color(token.Color));
            });   
        }
        
    }
    
}

