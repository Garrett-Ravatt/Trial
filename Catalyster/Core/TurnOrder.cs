using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Components;
using Catalyster.Interfaces;

namespace Catalyster.Core
{
    public class TurnOrder
    {
        private static readonly QueryDescription _desc = new QueryDescription()
            .WithAll<Energy>()
            .WithAny<Player, IDirector>();

        public bool PlayerLock = false;

        private Queue<Entity> _entities;
        public TurnOrder() { }

        public Entity? Update(World world)
        {
            if (!PlayerLock)
            {
                if (_entities == null || _entities.Count == 0)
                    _entities = QueryEntities(world);

                Entity entity;
                while (_entities.TryDequeue(out entity))
                {
                    if (entity.Has<Player>())
                    {
                        // TODO: Consider returning Player entity
                        PlayerLock = true;
                        return entity;
                    }
                    else
                    {
                        Console.WriteLine($"Calling director {entity}");
                        ref var director = ref entity.Get<IDirector>();
                        director.Direct(entity, world);
                    }
                }
            }
            return null;
        }

        // We CAN query entities but
        public Queue<Entity> QueryEntities(World world)
        {
            // TODO: Maybe should be Entity references.
            var queue = new Queue<Entity>();
            world.Query(in _desc, (Entity entity, ref Energy energy) =>
            {
                queue.Enqueue(entity);
                energy.Points = Math.Min(energy.Max, energy.Points + energy.Regen);
            });
            return queue;
        }
    }
}
