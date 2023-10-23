using Catalyster.Core;
using RogueSharp;
using SadConsole.UI;
using Rectangle = RogueSharp.Rectangle;

namespace Trial
{
    // An extension of DungeonMap able to render itself to a Console
    public class ConsoleMap : DungeonMap
    {
        public ConsoleMap() : base() { }
        public ConsoleMap(int Width, int Height)
        {
            Initialize(Width, Height);
            var room = new Rectangle(2, 2, Width-4, Height-4);
            for (int x = room.Left + 1; x <= room.Right; x++)
            {
                for (int y = room.Top + 1; y < room.Bottom; y++)
                {
                    SetCellProperties(x, y, true, true, true);
                }
            }
        }

        public void DrawTo(Console console)
        {
            foreach (Cell cell in GetAllCells())
            {
                SetConsoleSymbolForCell(console, cell);
            }
        }

        private void SetConsoleSymbolForCell(Console console, Cell cell)
        {
            if (!cell.IsExplored)
            {
                return;
            }

            if (cell.IsWalkable)
            {
                console.SetGlyph(cell.X, cell.Y, '.');
            }
            else
            {
                console.SetGlyph(cell.X, cell.Y, '#');
            }

            if (IsInFov(cell.X, cell.Y))
            {
                console.SetForeground(cell.X, cell.Y, new Color(0xffffffff));
                console.SetBackground(cell.X, cell.Y, new Color(0xff303048));
            }
            else
            {
                console.SetForeground(cell.X, cell.Y, new Color(0xffaaaaaa));
                console.SetBackground(cell.X, cell.Y, new Color(0xff101015));
            }
        }
    }
}
