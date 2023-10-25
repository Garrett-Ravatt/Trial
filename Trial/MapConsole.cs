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

            // North
            if (keyboard.IsKeyPressed(Keys.Up) || keyboard.IsKeyPressed(Keys.NumPad8))
            {
                Program.GameMaster.Command.Move(0, -1);
                handled = true;
            }
            // South
            else if (keyboard.IsKeyPressed(Keys.Down) || keyboard.IsKeyPressed(Keys.NumPad2))
            {
                Program.GameMaster.Command.Move(0, 1);
                handled = true;
            }
            // West
            else if (keyboard.IsKeyPressed(Keys.Left) || keyboard.IsKeyPressed(Keys.NumPad4))
            {
                Program.GameMaster.Command.Move(-1, 0);
                handled = true;
            }
            // East
            else if (keyboard.IsKeyPressed(Keys.Right) || keyboard.IsKeyPressed(Keys.NumPad6))
            {
                Program.GameMaster.Command.Move(1, 0);
                handled = true;
            }
            // NorthEast
            else if (keyboard.IsKeyPressed(Keys.NumPad9))
            {
                Program.GameMaster.Command.Move(1, -1);
                handled = true;
            }
            // SouthEast
            else if (keyboard.IsKeyPressed(Keys.NumPad3))
            {
                Program.GameMaster.Command.Move(1, 1);
                handled = true;
            }
            // SouthWest
            else if (keyboard.IsKeyPressed(Keys.NumPad1))
            {
                Program.GameMaster.Command.Move(-1, 1);
                handled = true;
            }
            // NorthWest
            else if (keyboard.IsKeyPressed(Keys.NumPad7))
            {
                Program.GameMaster.Command.Move(-1, -1);
                handled = true;
            }

            if (handled)
            {
                // this is hacky
                Program.GameMaster.Update();
                Program.GameMaster.Update();
                Program.Draw();
            }

            return handled;
        }
    }
}
