using Catalyster;
using Catalyster.RAW;
using SadConsole.Ansi;
using SadConsole.Input;
using SadConsole.UI;
using SadConsole.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Trial.InputStates;

namespace Trial.Consoles
{
    public class CatalogueWindow : Window
    {
        private MapConsole _mapConsole;
        private Console _descBody;
        private List<string> RIDs;
        const int SUBWIDTH = 20;
        const int SUBHEIGHT = 10;
        private List<Window> subWindows = new List<Window>();
        public CatalogueWindow(int width, int height, MapConsole mapConsole, int startIndex = 0): base(width, height)
        {
            _mapConsole = mapConsole;
            Center();

            // Take control from the map console because I am a popup menu
            IsFocused = true;
            mapConsole.IsFocused = false;

            // This is what I am
            Title = "Catalogue";

            // X BUTTON
            var xButton = new Button(1, 1)
            {
                Position = new Point(width - 1, 0),
                Text = "X"
            };

            xButton.MouseButtonClicked += (sender, args) =>
            {
                mapConsole.IsFocused = true;
                mapConsole.SetState(MapInputState.Map);
                Children.Clear();
                Dispose();
            };

            Controls.Add(xButton);

            // Header
            var head = new Console(width - 2, 2) { Position = new Point(1, 1) };
            head.Cursor.Print("Select a creature or item to see its catalogue entry");
            Children.Add(head);

            RIDs = GameMaster.Instance().Stats.Stats.Keys.ToList();
            var listbox = new ListBox(width - 2, height - 2)
            {
                Position = new Point(1, 4),
                IsFocused = true,
                UseKeyboard = true,
                UseMouse = true,
                SingleClickItemExecute = true,
            };

            listbox.SelectedItemExecuted += (sender, args) =>
            {
                subWindows.Add(new EntityDefinitionWindow(SUBWIDTH, SUBHEIGHT,
                    GameMaster.Instance().Stats.Get(RIDs[listbox.SelectedIndex]),
                    this
                    ));
            };

            foreach (var rid in RIDs)
                listbox.Items.Add(rid);

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

        public override bool ProcessMouse(MouseScreenObjectState state)
        {
            var handled = base.ProcessMouse(state);
            
            if (!handled && IsFocused)
            {
                Cursor.Position = state.CellPosition;

                if (state.Mouse.LeftClicked)
                {
                    var adj = state.WorldCellPosition.Subtract(_mapConsole.Position);
                    var def = GameMaster.Instance().Command.Describe(adj.X, adj.Y);
                    if (def != null)
                        subWindows.Add(new EntityDefinitionWindow(SUBWIDTH, SUBHEIGHT, def, this));
                }
                handled = true;
            }
             
            return handled;
        }
    }
}
