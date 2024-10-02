using Arch.Core;
using Catalyster.Interfaces;
using System.Transactions;

namespace Catalyster.Components
{
    public class MoveAct : IAct
    {
        public int Cost { get; set; } = 1000;
        
        public int X;
        public int Y;

        public MoveAct(int x = 0, int y = 0)
        {
            X = x;
            Y = y;
        }

        public bool Enter(Entity entity, World world)
        {
            return true;
        }
    }
}
