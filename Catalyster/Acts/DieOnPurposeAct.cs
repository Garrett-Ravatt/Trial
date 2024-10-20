using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Interfaces;

namespace Catalyster.Acts
{
    public class DieOnPurposeAct : IAct
    {
        public int Cost { get; set; } = 1000;
        public bool Resolved { get; } = false;
        public bool Suspended { get; set; } = false;

        public bool Confirmed = false;
        public EntityReference? EntityReference;

        public DieOnPurposeAct(EntityReference? entityReference)
        {
            EntityReference = entityReference;
        }

        public IAct Execute()
        {
            if (EntityReference == null || !EntityReference.Value.IsAlive())
            {
                // TODO: Malformed error
                return this;
            }

            var entity = EntityReference.Value.Entity;

            // TODO: Poll input
            Suspended = true;
            return this;
        }
    }
}
