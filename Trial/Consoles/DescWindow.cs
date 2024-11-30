using Catalyster;
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
        public DescWindow(int x, int y, MapConsole mapConsole) : base(30, 8)
        {
            _mapConsole = mapConsole;
            IsFocused = true;

            // Put Description window wherever it will fit on screen
            var (p, q) = (x, y);
            if (p + Width > GameSettings.Width)
                p -= Width - 1;
            else
                p += 1;

            if (q + Height > GameSettings.Height)
                q -= Height;
            else
                q += 1;
            
            Position = new Point(p, q);

            var def = GameMaster.Instance().Command.Describe(x, y);
            if (def != null)
            {
                // This is what I am
                Title = def.Name;

                var descConsole = new Console(Width - 2, Height - 2)
                {
                    Position = new Point(1, 1),
                };

                descConsole.Print(0, 0, def.Description);

                Children.Add(descConsole);
            }
            else
            {
                // This is what I am
                Title = "Desc";

                this.Print(1, 1, "Empty");
            }

            Show();
        }

        public void Transition(int X, int Y)
        {
            new DescWindow(X, Y, _mapConsole);
            Children.Clear();
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
                Children.Clear();
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
