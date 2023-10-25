using SadConsole.Input;

namespace Trial
{
    public class MapConsole : Console
    {
        public MapConsole(int width, int height) : base(width, height) { }
        public override bool ProcessKeyboard(Keyboard keyboard)
        {
            bool handled = false;

            // This is hacky
            Program.GameMaster.Update();
            Program.GameMaster.Update();

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

            // the performance of this is terrible
            if (handled)
            {
                Program.Draw();
            }

            return handled;
        }
    }
}
