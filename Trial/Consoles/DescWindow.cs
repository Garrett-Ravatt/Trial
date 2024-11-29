using SadConsole.Ansi;
using SadConsole.Input;
using SadConsole.UI;
using SadConsole.UI.Controls;
using Trial.InputStates;

namespace Trial.Consoles
{
    public class DescWindow : Window
    {
        private MapConsole _mapConsole;

        // we may very well overload this constructor with other consoles that may call this popup.
        public DescWindow(int x, int y, MapConsole mapConsole) : base(20, 5)
        {
            _mapConsole = mapConsole;
            IsFocused = true;

            Position = new Point(x + 1, y - Height);

            // This is what I am
            Title = "Desc";

            this.Print(1, 1, "Empty Description");

            // X BUTTON
            var xButton = new Button(1, 1)
            {
                Position = new Point(Width, 0),
                Text = "X"
            };

            xButton.MouseButtonClicked += (sender, args) =>
            {
                mapConsole.IsFocused = true;
                mapConsole.SetState(MapInputState.Map);
                Dispose();
            };

            Controls.Add(xButton);

            Show();
        }

        public void Transition(int X, int Y)
        {
            new DescWindow(X, Y, _mapConsole);
            Dispose();
        }

        public override bool ProcessMouse(MouseScreenObjectState state)
        {
            var handled = false;
            if (!state.IsOnScreenObject && IsFocused)
            {
                if (state.Mouse.LeftClicked)
                    Transition(state.WorldCellPosition.X, state.WorldCellPosition.Y);
                handled = true;
            }
            return handled;
        }

        public override bool ProcessKeyboard(Keyboard keyboard)
        {
            bool handled = false;

            if (keyboard.IsKeyPressed(Keys.Escape) || keyboard.IsKeyPressed(Keys.Back))
            {
                IsFocused = false;
                _mapConsole.IsFocused = true;
                _mapConsole.SetState(MapInputState.Map);
                Dispose();
                handled = true;
            }

            if (handled)
            {

            }

            // TODO: nums & move it around.
            return base.ProcessKeyboard(keyboard);
        }
    }
}
