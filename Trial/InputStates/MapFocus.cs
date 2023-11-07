using SadConsole.Input;

namespace Trial.InputStates
{
    public enum MapFocusState
    {
        Map,
        Throwing,
        // etc, expand as needed.
    }

    public static class MapFocus
    {
        public static bool ProcessKeyboard(MapFocusState state, bool handled, Keyboard keyboard, Console mapConsole)
        {
            if (handled)
                return handled;

            switch (state)
            {
                case MapFocusState.Map:
                    return HandleMove(handled, keyboard);

                case MapFocusState.Throwing:
                    return HandleTargeting(handled, keyboard, mapConsole);
            }
            // Placeholder for unimplemented states
            return handled;
        }

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

        public static bool HandleTargeting(bool handled, Keyboard keyboard, Console mapConsole)
        {
            if (handled)
                return handled;

            // North
            else if (keyboard.IsKeyPressed(Keys.Up) || keyboard.IsKeyPressed(Keys.NumPad8))
            {
                mapConsole.Cursor.Position = mapConsole.Cursor.Position.Add(new Point(0, -1));
                handled = true;
            }
            // South
            else if (keyboard.IsKeyPressed(Keys.Down) || keyboard.IsKeyPressed(Keys.NumPad2))
            {
                mapConsole.Cursor.Position = mapConsole.Cursor.Position.Add(new Point(0, 1));
                handled = true;
            }
            // West
            else if (keyboard.IsKeyPressed(Keys.Left) || keyboard.IsKeyPressed(Keys.NumPad4))
            {
                mapConsole.Cursor.Position = mapConsole.Cursor.Position.Add(new Point(-1, 0));
                handled = true;
            }
            // East
            else if (keyboard.IsKeyPressed(Keys.Right) || keyboard.IsKeyPressed(Keys.NumPad6))
            {
                mapConsole.Cursor.Position = mapConsole.Cursor.Position.Add(new Point(1, 0));
                handled = true;
            }
            // NorthEast
            else if (keyboard.IsKeyPressed(Keys.NumPad9))
            {
                mapConsole.Cursor.Position = mapConsole.Cursor.Position.Add(new Point(1, -1));
                handled = true;
            }
            // SouthEast
            else if (keyboard.IsKeyPressed(Keys.NumPad3))
            {
                mapConsole.Cursor.Position = mapConsole.Cursor.Position.Add(new Point(1, 1));
                handled = true;
            }
            // SouthWest
            else if (keyboard.IsKeyPressed(Keys.NumPad1))
            {
                mapConsole.Cursor.Position = mapConsole.Cursor.Position.Add(new Point(-1, 1));
                handled = true;
            }
            // NorthWest
            else if (keyboard.IsKeyPressed(Keys.NumPad7))
            {
                mapConsole.Cursor.Position = mapConsole.Cursor.Position.Add(new Point(-1, -1));
                handled = true;
            }

            return handled;
        }
    }
}
