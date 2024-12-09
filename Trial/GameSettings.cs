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
        public const int Height = MapHeight + HeaderHeight;

        public const int HeaderWidth = Width;
        public const int HeaderHeight = 3;

        public const int MapWidth = 80;
        public const int MapHeight = 25;

        public const int MessageLogWidth = 40;
        public const int MessageLogHeight = MapHeight;
    }
}
