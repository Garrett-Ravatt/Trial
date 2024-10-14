using Arch.Core;
using TinyMessenger;

namespace Catalyster.Messages
{
    public class DeathMessage : TinyMessageBase
    {
        public EntityReference Ref;
        public DeathMessage(object sender, EntityReference entityref) : base(sender)
        {
            Ref = entityref;
        }
    }
}