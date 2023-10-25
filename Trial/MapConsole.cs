using Arch.System;
using Catalyster.Components;
using SadConsole.Input;
using System.Runtime.CompilerServices;

namespace Trial
{
    public partial class MapConsole : Console
    {
        public MapConsole(int width, int height) : base(width, height) { }
        public override bool ProcessKeyboard(Keyboard keyboard)
        {
            bool handled = false;

            if (keyboard.IsKeyPressed(Keys.Up))
            {
                Program.GameMaster.Command.Move(0, -1);
                handled = true;
            }
            else if (keyboard.IsKeyPressed(Keys.Down))
            {
                Program.GameMaster.Command.Move(0, 1);
                handled = true;
            }

            if (keyboard.IsKeyPressed(Keys.Left))
            {
                Program.GameMaster.Command.Move(-1, 0);
                handled = true;
            }
            else if (keyboard.IsKeyPressed(Keys.Right))
            {
                Program.GameMaster.Command.Move(1, 0);
                handled = true;
            }

            // the lag from this is terrible.
            if (handled)
            {
                // This is hacky
                Program.Draw();
                Program.GameMaster.Update();
                Program.GameMaster.Update();
            }

            return handled;
        }
    }
}
