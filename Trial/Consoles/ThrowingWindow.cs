using SadConsole.DrawCalls;
using SadConsole.UI;
using SadConsole.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trial.Consoles
{
    public class ThrowingWindow : Window
    {
        public ThrowingWindow(int width, int height) : base(width, height)
        {
            //this.Print(1, 1, "Select an item to throw");
            Title = "Select an item to throw";

            var listwidth = width-2;
            var listheight = height-3;

            var listbox = new ListBox(listwidth, listheight)
            {
                Position = new Point(1, 2),
                IsFocused = true,
                UseKeyboard = true,
                UseMouse = true,
            };

            listbox.SelectedItemExecuted += (sender, args) =>
            {
                // TODO: collect the item index
                Controls.Clear();
                Hide(); // TODO: teardown
                Parent.IsFocused = true;
            };

            foreach (string s in Program.GameMaster.Command.Inventory())
                listbox.Items.Add(s);
            Controls.Add(listbox);
        }
    }
}
