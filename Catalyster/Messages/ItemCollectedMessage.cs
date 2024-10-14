using Arch.Core;
using Catalyster.Components;
using TinyMessenger;

namespace Catalyster.Messages
{
    public class ItemCollectedMessage : TinyMessageBase
    {
        public EntityReference Collector, ItemEntity;
        public Item Item;
        public ItemCollectedMessage(object sender,
            EntityReference collector, EntityReference itemEntity, Item item) : base(sender)
        {
            Collector = collector;
            ItemEntity = itemEntity;
            Item = item;
        }
    }
}
