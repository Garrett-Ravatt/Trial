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
        public EntityReference? Acting { get; set; }
        public WaitAct(EntityReference? entityRef = null)
        {
            Acting = entityRef;
        }

        public IAct Execute()
        {
            if (Acting == null)
                throw new Exception($"{GetType()} ran into null entity reference");

            var entity = Acting.Value.Entity;

            entity.Get<Stats>().Energy = 0;
            Resolved = true;
            return this;
        }
    }
}
