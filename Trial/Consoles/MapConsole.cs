using SadConsole.Components;
using SadConsole.Input;
using Trial.InputStates;

namespace Trial.Consoles
{
    public class MapConsole : Console
    {
        public MapConsole(int width, int height) : base(width, height) { }

        public override bool ProcessMouse(MouseScreenObjectState state)
        {
            Cursor.Position = state.CellPosition;
            return base.ProcessMouse(state);
        }

        public override bool ProcessKeyboard(Keyboard keyboard)
        {
            bool handled = false;

            if (keyboard.IsKeyPressed(Keys.T))
            {
                // enable cursor (if item index is known)
                Cursor.IsVisible = true;
                Cursor.IsEnabled = true;
                
                // TODO: Open popup
            }

            handled = MapFocus.HandleMove(handled, keyboard);

            if (handled)
            {
                // this is hacky (but I am at peace)
                Program.GameMaster.Update();
                Program.GameMaster.Update();
                Program.Draw();
            }

            return handled;
        }
    }
}
