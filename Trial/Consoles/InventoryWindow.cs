using SadConsole.UI;
using SadConsole.UI.Controls;
using SadConsole.Input;
using Trial.InputStates;

namespace Trial.Consoles
{
    public class InventoryWindow : Window
    {
        private MapConsole _mapConsole;

        // we may very well overload this constructor with other consoles that may call this popup.
        public InventoryWindow(int width, int height, MapConsole mapConsole) : base(width, height)
        {
            _mapConsole = mapConsole;
            Center();

            // Take control from the map console because I am a popup menu
            IsFocused = true;
            mapConsole.IsFocused = false;

            // This is what I am
            Title = "Inventory";

            // X BUTTON
            var xButton = new Button(1, 1)
            {
                Position = new Point(width-1, 0),
                Text = "X"
            };

            xButton.MouseButtonClicked += (sender, args) =>
            {
                mapConsole.IsFocused = true;
                mapConsole.SetState(MapInputState.Map);
                Dispose();
            };

            Controls.Add(xButton);

            // INVENTORY LIST

            // Fit list within the margin
            var listwidth = width - 2;
            var listheight = height - 3;

            var listbox = new ListBox(listwidth, listheight)
            {
                Position = new Point(1, 2),
                IsFocused = true,
                UseKeyboard = true,
                UseMouse = true,
            };

            listbox.SelectedItemChanged += (sender, args) =>
            {
                // TODO: populate a description of this item: Options, contents, description
            };

            listbox.SelectedItemExecuted += (sender, args) =>
            {
                //CommandBobber.Throw(index: 0);

                mapConsole.IsFocused = true;
                mapConsole.SetState(MapInputState.Map);
                Dispose();
            };

            foreach (string s in Program.GameMaster.Command.Inventory())
                listbox.Items.Add(s);
            Controls.Add(listbox);

            Show();
        }

        public override bool ProcessKeyboard(Keyboard keyboard)
        {
            bool handled = false;

            if (keyboard.IsKeyPressed(Keys.Escape) || keyboard.IsKeyPressed(Keys.Back))
            {
                IsFocused = false;
                _mapConsole.IsFocused = true;
                _mapConsole.IsFocused = true;
                _mapConsole.SetState(MapInputState.Map);
                Dispose();
                handled = true;
            }

            if (handled)
            {
                // this is hacky (but I am at peace)
                Program.GameMaster.Update();
                Program.GameMaster.Update();
                Program.Draw();
            }
            else
                handled = base.ProcessKeyboard(keyboard);

            return handled;
        }
    }
}
