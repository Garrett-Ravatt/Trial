using Catalyster.Core;
using RogueSharp;
using SadConsole.UI;
using Rectangle = RogueSharp.Rectangle;

namespace Trial
{
    // An extension of DungeonMap able to render itself to a Console
    public class DrawingMap : DungeonMap
    {
        public DrawingMap() : base() { }

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
                if (cell.IsTransparent)
                    console.SetGlyph(cell.X, cell.Y, '.');
                else
                    console.SetGlyph(cell.X, cell.Y, '+');
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
