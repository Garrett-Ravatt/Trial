using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trial
{
    static class GameSettings
    {
        public const int Width = MapWidth + MessageLogWidth;
        public const int Height = 25;

        public const int MapWidth = 80;
        public const int MapHeight = Height;

        public const int MessageLogWidth = 40;
        public const int MessageLogHeight = Height;
    }
}
