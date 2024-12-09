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
            // offset below header
            Position = new Point(0, GameSettings.HeaderHeight);

            State = MapInputState.Map;
        }

        // TODO: Put into MapFocus
        public override bool ProcessMouse(MouseScreenObjectState state)
        {
            var handled = false;
            switch (State)
            {
                case MapInputState.Throwing:
                    if (Cursor.IsEnabled && state.IsOnScreenObject && IsFocused)
                    {
                        Cursor.Position = state.CellPosition;

                        if (state.Mouse.LeftClicked)
                        {
                            CommandBobber.Throw(x: state.CellPosition.X, y: state.CellPosition.Y);
                            SetState(MapInputState.Map);
                        }
                        handled = true;
                    }
                    break;

                case MapInputState.Map:
                    if (state.IsOnScreenObject && IsFocused)
                    {
                        if (state.Mouse.LeftClicked)
                        {
                            new DescWindow(state.CellPosition.X, state.CellPosition.Y, this);
                        }
                        handled = true;
                    }
                    break;
            }

            return handled;
        }

        public override bool ProcessKeyboard(Keyboard keyboard)
        {
            bool handled = false;

            if (keyboard.IsKeyPressed(Keys.T))
            {
                IsFocused = false;
                new ThrowingWindow(40, 20, this);
            }
            else if (keyboard.IsKeyReleased(Keys.Tab))
            {
                IsFocused = false;
                new InventoryWindow(40, 20, this);
            }
            else if (keyboard.IsKeyReleased(Keys.C))
            {
                IsFocused = false;
                new CatalogueWindow(40, 20, this);
            }

            handled = MapFocus.ProcessKeyboard(State, handled, keyboard, this);

            if (handled)
            {
                Program.GameMaster.Resolve();
                Program.Draw();
            }

            return handled;
        }
    }
}
