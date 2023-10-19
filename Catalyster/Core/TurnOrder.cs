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

        private List<Entity> _entities;
        public TurnOrder(World world, DungeonMap map)
        {
            _world = world;
            _map = map;
            PlayerLock = false;
        }

        public void Update()
        {
            foreach(var entity in _entities)
            {
                if ( entity.Has<Player>() )
                {
                    PlayerLock = true;
                    break;
                }
                else
                {
                    entity.Get<Energy>().Points -= 1000;
                }
            }
        }

        // We CAN query entities but
        public List<Entity> QueryEntities()
        {
            // TODO: Maybe should be Entity references.
            var list = new List<Entity>();
            _world.Query(in _desc, (Entity entity) =>
            {
                list.Add(entity);
            });
            return list;
        }
    }
}
