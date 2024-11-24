using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Components;
using Catalyster.RAW;

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
    }
}
