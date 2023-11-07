using SadConsole.Components;
using SadConsole.Effects;
using SadConsole.Input;
using SadConsole.UI.Controls;
using Trial.InputStates;

namespace Trial.Consoles
{
    public class MapConsole : Console
    {
        private MapFocusState _state;
        public int ItemIndex = 0;
        public MapConsole(int width, int height) : base(width, height)
        {
            Cursor.CursorRenderEffect = new Blink() { BlinkCount = -1 };
            Cursor.ApplyCursorEffect = true;

            _state = MapFocusState.Map;
        }

        public override bool ProcessMouse(MouseScreenObjectState state)
        {
            if (Cursor.IsEnabled && state.IsOnScreenObject)
            {
                if (state.Mouse.LeftButtonDown)
                {
                    // NOTE:This Pretends the x,y is definitely accurate and there is an inventory item.
                    Program.GameMaster.Command.Throw(state.CellPosition.X, state.CellPosition.Y, 0);
                }

                Cursor.Position = state.CellPosition;
            }

            return base.ProcessMouse(state);
        }

        public override bool ProcessKeyboard(Keyboard keyboard)
        {
            bool handled = false;

            if (keyboard.IsKeyPressed(Keys.T))
            {
                // enable cursor (if item index is known)


                // TODO: Open better popup

                IsFocused = false;

                var window = new ThrowingWindow(40, 30);
                window.Parent = this;
                window.Show();

                _state = MapFocusState.Throwing;
                Cursor.IsVisible = true;
                Cursor.IsEnabled = true;
            }

            handled = MapFocus.ProcessKeyboard(_state, handled, keyboard, this);

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
