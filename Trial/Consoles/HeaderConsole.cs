using Catalyster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trial.Consoles
{
    public class HeaderConsole : Console
    {
        public HeaderConsole(): base(GameSettings.HeaderWidth, GameSettings.HeaderHeight) { }

        public void Draw()
        {
            // Fetch
            var nstats = GameMaster.Instance().Command.Stats();
            if (!nstats.HasValue)
                return;
            var stats = nstats.Value;

            // Print
            this.Clear();
            this.Print(0, 0, new string('-',GameSettings.HeaderWidth));
            this.Print(0, 2, new string('-', GameSettings.HeaderWidth));
            this.Print(2, 1, $"BL:{stats.HP}/{stats.Blood}");
            this.Print(10, 1, $"BO:{stats.Body}");
            this.Print(16, 1, $"BR:{stats.Breath}");
        }
    }
}
