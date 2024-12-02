using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Components;
using Inventory = Catalyster.Items.Inventory;
using Catalyster.Helpers;
using Catalyster.Interfaces;
using Catalyster.Messages;

namespace Catalyster.Acts.ItemManipulation
{
    public class ItemCollectAct : IAct
    {

        public int Cost { get; set; }

        public EntityReference? Acting { get; set; }

        public bool Resolved { get; private set; } = false;

        public bool Suspended { get; private set; } = false;

        public ItemCollectAct(EntityReference acting)
        {
            Acting = acting;
        }

        public IAct Execute()
        {
            if (Acting == null || !Acting.Value.IsAlive())
                throw new Exception($"{Acting} entity reference invalid in {this}");

            var e = Acting.Value.Entity;

            if (!e.Has<Inventory, Position>())
            {
                if (e.Has<Player>())
                {
                    Resolved = true;
                    return this;
                }

                throw new Exception($"{e} tried to pick up items without Inventory and/or Position");
            }

            var position = e.Get<Position>();
            var inv = e.Get<Inventory>();

            // Look for items, collect them
            var foundSomething = false;
            GameMaster.Instance().World.Query(
                in new QueryDescription().WithAll<Token, Position, Item>(),
                (Entity entity, ref Token token, ref Position pos, ref Item item) =>
                {
                    // TODO: SpatialHash refactor point
                    // TODO: Check inventory capacity
                    // TODO: Use Command Buffer inside query
                    if (SpatialHelper.LazyDist(position, pos) <= 1)
                    {
                        var entityRef = entity.Reference();
                        entity.Remove<Position>();
                        inv.Items.Add(entityRef);
                        GameMaster.Instance().MessageLog.Hub.Publish(new ItemCollectedMessage(e, e.Reference(), entityRef, item));
                        foundSomething = true;
                    }
                });

            if (foundSomething)
            {
                ref var stats = ref e.Get<Stats>();
                stats.Energy -= WiggleHelper.Wiggle(Cost, 0.1);
            }

            Resolved = true;
            return this;
        }
    }
}
