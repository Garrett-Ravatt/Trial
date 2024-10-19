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

        private Queue<EntityReference> _entities;
        public TurnOrder() { }

        public Entity? Update(World world)
        {
            if (!PlayerLock)
            {
                if (_entities == null || _entities.Count == 0)
                    _entities = QueryEntities(world);

                EntityReference entityRef;
                while (_entities.TryDequeue(out entityRef))
                {
                    if (entityRef.IsAlive())
                    {
                        var entity = entityRef.Entity;
                        if (entity.Has<Player>())
                        {
                            PlayerLock = true;
                            return entity;
                        }
                        else
                        {
                            ref var director = ref entity.Get<IDirector>();
                            ref var energy = ref entity.Get<Energy>();
                            while (energy.Points > 0)
                            {
                                var act = director.Direct(entity, world);
                                if (act != null)
                                    act.Execute();
                                else
                                    break;
                            }
                        }
                    }
                }
            }
            return null;
        }

        // We CAN query entities but
        public Queue<EntityReference> QueryEntities(World world)
        {
            var queue = new Queue<EntityReference>();
            world.Query(in _desc, (Entity entity, ref Energy energy) =>
            {
                queue.Enqueue(entity.Reference());
                energy.Points = Math.Min(energy.Max, energy.Points + energy.Regen);
            });
            return queue;
        }
    }
}
