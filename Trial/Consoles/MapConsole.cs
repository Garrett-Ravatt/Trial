using SadConsole.Components;
using SadConsole.Effects;
using SadConsole.Input;
using Trial.InputStates;

namespace Trial.Consoles
{
    public class MapConsole : Console
    {
        public MapInputState State { get; private set; }
        public void SetState(MapInputState value)
        {
            if (State == value) return;

            switch (State)
            {
                case MapInputState.Throwing:
                    Cursor.IsEnabled = false;
                    Cursor.IsVisible = false;
                    Cursor.ApplyCursorEffect = false;
                    break;
            }

            switch (value)
            {
                case MapInputState.Throwing:
                    Cursor.IsEnabled = true;
                    Cursor.IsVisible = true;
                    Cursor.CursorRenderEffect = new Blink() { BlinkCount = -1 };
                    Cursor.ApplyCursorEffect = true;
                    break;
            }

            State = value;
        }

        public int ItemIndex = 0;
        public MapConsole(int width, int height) : base(width, height)
        {
            

            State = MapInputState.Map;
        }

        public override bool ProcessMouse(MouseScreenObjectState state)
        {
            var handled = false;

            if (Cursor.IsEnabled && state.IsOnScreenObject && IsFocused)
            {
                Cursor.Position = state.CellPosition;

                if (state.Mouse.LeftButtonDown)
                {
                    CommandBobber.Throw(x: state.CellPosition.X, y: state.CellPosition.Y);
                    SetState(MapInputState.Map);
                }
                handled = true;
            }

            return handled;
        }

        public override bool ProcessKeyboard(Keyboard keyboard)
        {
            bool handled = false;

            if (keyboard.IsKeyPressed(Keys.T))
            {
                IsFocused = false;

                // We don't need to store this
                new ThrowingWindow(40, 20, this);
            }

            handled = MapFocus.ProcessKeyboard(State, handled, keyboard, this);

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
