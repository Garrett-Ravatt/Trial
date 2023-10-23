using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Components;
using Catalyster.Interfaces;

namespace Catalyster.Core
{
    public class TurnOrder
    {
        private World _world;
        private static readonly QueryDescription _desc = new QueryDescription()
            .WithAll<Energy>()
            .WithAny<Player, IDirector>();

        private DungeonMap _map;

        public bool PlayerLock;

        private Queue<Entity> _entities;
        public TurnOrder(World world, DungeonMap map)
        {
            _world = world;
            _map = map;
            PlayerLock = false;
        }

        public void Update(World world)
        {
            if (!PlayerLock)
            {
                _entities = QueryEntities();
                Entity entity;
                while (_entities.TryDequeue(out entity))
                {
                    if (entity.Has<Player>())
                    {
                        // TODO: Consider returning Player entity
                        PlayerLock = true;
                        break;
                    }
                    else
                    {
                        entity.Get<IDirector>().Direct(entity, world);
                    }
                }
            }
            else
            {
                //handle player's continuing turn
            }
        }

        // We CAN query entities but
        public Queue<Entity> QueryEntities()
        {
            // TODO: Maybe should be Entity references.
            var queue = new Queue<Entity>();
            _world.Query(in _desc, (Entity entity) =>
            {
                queue.Enqueue(entity);
            });
            return queue;
        }
    }
}
