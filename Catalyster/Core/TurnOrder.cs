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
                    ref var energy = ref entity.Get<Energy>();
                    while (energy.Points > 0)
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
                    _entities.RemoveFirst();
                }
            }
            return null;
        }

        // We CAN query entities but
        public LinkedList<EntityReference> QueryEntities(World world)
        {
            var queue = new LinkedList<EntityReference>();
            world.Query(in _desc, (Entity entity, ref Energy energy) =>
            {
                queue.AddLast(entity.Reference());
                energy.Points = Math.Min(energy.Max, energy.Points + energy.Regen);
            });
            return queue;
        }
    }
}
