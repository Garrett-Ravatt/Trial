using Catalyster.Core;
using RogueSharp;
using Arch.Core;
using SadConsole.UI;
using Catalyster.Components;

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
            var map = new ConsoleMap(Width, Height); //won't be full dimensions forever.
            //More detailed map gen steps from Catalyster later on.
            
            var world = World.Create(); // Catalyster should do this for us as well.

            //Enemy
            EntityBuilder.Goblin(world);

            //Player
            EntityBuilder.Player(world);

            var startingConsole = (Console)GameHost.Instance.Screen;

            map.UpdateFieldOfView(world);
            map.DrawTo(startingConsole);

            world.Query(in new QueryDescription().WithAll<Position, Token>(), (ref Position position, ref Token token) =>
            {
                startingConsole.SetGlyph(position.X, position.Y, token.Char);
                startingConsole.SetForeground(position.X, position.Y, new Color(token.Color));
                //TODO: add the color :)
            });   
        }
        
    }
    
}

