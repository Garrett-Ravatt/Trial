using SadConsole.Input;

namespace Trial.InputStates
{
    public static class MapFocus
    {
        // This is just a long chunk of the key map stuffed into a method.
        public static bool HandleMove(bool handled, Keyboard keyboard)
        {
            if (handled)
                return handled;

            // North
            else if (keyboard.IsKeyPressed(Keys.Up) || keyboard.IsKeyPressed(Keys.NumPad8))
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
            else if (keyboard.IsKeyPressed(Keys.NumPad5))
            {
                Program.GameMaster.Command.Wait();
                handled = true;
            }

            return handled;
        }
    }
}
