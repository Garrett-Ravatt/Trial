using Catalyster;
using Catalyster.RAW;
using SadConsole.UI;
using SadConsole.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trial.Consoles
{
    public class EntityDefinitionWindow : Window
    {
        static List<string> open = new List<string>();
        
        CatalogueWindow _cat;

        public EntityDefinitionWindow(int width, int height, EntityDefinition def, CatalogueWindow cat): base(width, height)
        {
            _cat = cat;
            if (open.Contains(def.Name))
            {
                _cat.IsFocused = true;
                Dispose();
            }
            else
            {
                open.Add(def.Name);

                // This is what I am
                Title = def.Name;

                // X BUTTON
                var xButton = new Button(1, 1)
                {
                    Position = new Point(width - 1, 0),
                    Text = "X"
                };

                xButton.MouseButtonClicked += (sender, args) =>
                {
                    IsFocused = false;
                    _cat.IsFocused = true;
                    open.Remove(def.Name);
                    Children.Clear();
                    Dispose();
                };

                Controls.Add(xButton);

                // DESCRIPTION BODY
                var descBody = new Console(width - 2, height - 2)
                {
                    Position = new Point(1, 1),
                    //DefaultBackground = new Color(0xffffffff),
                };
                Children.Add(descBody);

                // Print content
                descBody.Cursor
                    .Print($"{def.Name}\n\n")
                    .CarriageReturn()
                    .Print($"{def.Description}");

                Show();
            }
        }
    }
}
