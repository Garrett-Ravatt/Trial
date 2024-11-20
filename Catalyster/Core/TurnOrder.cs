using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Components;
using Catalyster.Interfaces;

namespace Catalyster.Core
{
    public class TurnOrder
    {
        private static readonly QueryDescription _desc = new QueryDescription()
            .WithAll<Stats, IDirector>();

        // InjectedAct to be resolved before returning to the turn order
        public IAct? SuspendedAct;

        private LinkedList<EntityReference> _entities;
        public TurnOrder() { }

        public bool Resolve()
        {
            if (SuspendedAct == null)
                return true;
            SuspendedAct = SuspendedAct.Consume();
            var s = SuspendedAct.Suspended;
            // TODO: Consider nullifying the CommandInjectionAct as well as the suspended act
            if (SuspendedAct.Resolved)
                SuspendedAct = null;
            return !s;
        }

        public Entity? Update(World world)
        {
            
            if (_entities == null || _entities.Count == 0)
                _entities = QueryEntities(world);

            EntityReference entityRef;
            while (_entities.Count > 0)
            {
                entityRef = _entities.First();
                if (entityRef.IsAlive())
                {
                    var entity = entityRef.Entity;
                    ref var director = ref entity.Get<IDirector>();
                    ref var stats = ref entity.Get<Stats>();
                    while (stats.Energy > 0)
                    {
                        var act = director.Direct(entity, world);
                                
                        if (act == null)
                        {
                            Console.Error.WriteLine($"Director output null: {director}");
                            break;
                        }

                        var result = act.Consume();
                        if (result == null)
                        {
                            Console.Error.WriteLine($"Act output null: {act}");
                        }
                        else if (result.Suspended)
                        {
                            SuspendedAct = result;
                            if (entity.Has<Player>())
                            {
                                // push player onto the front of the queue
                                _entities.AddFirst(entityRef);
                                return entity;
                            }
                            return null;
                        }
                    }
                }
                _entities.RemoveFirst();
            }
            return null;
        }

        const int BREATH_ENERGY_MULT = 100;
        // We CAN query entities but
        public LinkedList<EntityReference> QueryEntities(World world)
        {
            var queue = new LinkedList<EntityReference>();
            world.Query(in _desc, (Entity entity, ref Stats stats) =>
            {
                queue.AddLast(entity.Reference());
                stats.Energy = Math.Min(stats.Breath * BREATH_ENERGY_MULT, stats.Energy + stats.Breath * BREATH_ENERGY_MULT);
            });
            return queue;
        }
    }
}
