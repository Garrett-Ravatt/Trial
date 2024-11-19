using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Components;
using Catalyster.Interfaces;

namespace Catalyster.Acts
{
    public class WaitAct : IAct
    {
        public int Cost {get;} = 0;
        public bool Resolved { get; set; } = false;
        public bool Suspended { get; set; } = false;
        public EntityReference? EntityRef;
        public WaitAct(EntityReference? entityRef = null)
        {
            EntityRef = entityRef;
        }

        public IAct Execute()
        {
            if (EntityRef == null)
                throw new Exception($"{GetType()} ran into null entity reference");

            var entity = EntityRef.Value.Entity;

            entity.Get<Stats>().Energy = 0;
            Resolved = true;
            return this;
        }
    }
}
