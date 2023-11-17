using Catalyster.Core;
using Catalyster.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalyster.Models
{
    // TODO: Test Coverage
    public class ClearMap : IStep<DungeonMap>
    {
        public DungeonMap Step(DungeonMap subject, int _seed)
        {
            for (int x = 0; x < subject.Width; x++)
            {
                for (int y = 0; y < subject.Height; y++)
                {
                    subject.SetCellProperties(x, y, true, true);
                }
            }
            return subject;
        }
    }
}
