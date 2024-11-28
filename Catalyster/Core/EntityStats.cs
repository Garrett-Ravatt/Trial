using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Components;
using Catalyster.RAW;
using CommunityToolkit.HighPerformance;

namespace Catalyster.Core
{
    public class EntityStats
    {
        public World World;
        public Dictionary<string, EntityDefinition> Stats = new Dictionary<string, EntityDefinition>();
        
        public EntityStats()
        { 
            World = World.Create();
        }

        public bool Has(string rid)
        {
            return Stats.ContainsKey(rid);
        }
        
        public void Define(EntityDefinition def)
        {
            var e = def.EntityReference.Entity;
            if (!e.Has<Token>() || e.Get<Token>().RID == null)
                throw new Exception("Tried to define an entity without token RID");
            var rid = e.Get<Token>().RID;
            if (Stats.ContainsKey(rid))
                throw new Exception("Tried to write to an entity definition twice");
            Stats[rid] = def;
        }

        public EntityDefinition Get(string rid)
        {
            return Stats[rid];
        }

        public Entity CreateIn(string rid, World world)
        {
            var def = Stats[rid];
            var e = def.EntityReference.Entity;

            var archetype = e.GetArchetype();
            var copiedEntity = world.Create(archetype.Types);
            // foreach ((var number, var word) in numbers.Zip(words, (n, w) => (n, w))) { ... }
            foreach ((var type, var component) in archetype.Types.Zip(e.GetAllComponents(), (t, c) => (t, c)))
            {
                copiedEntity.Set(Convert.ChangeType(component, type));
            }

            return copiedEntity;
        }
    }
}
