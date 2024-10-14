using Arch.Core;
using TinyMessenger;

namespace Catalyster.Messages
{
    public class WallBumpMessage : TinyMessageBase
    {
        public int X, Y;
        public EntityReference Ref;
        public WallBumpMessage(object sender, int x, int y, EntityReference @ref) : base(sender)
        {
            X = x;
            Y = y;
            Ref = @ref;
        }
    }
}
