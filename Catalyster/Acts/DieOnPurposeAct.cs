using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Interfaces;
using Catalyster.Messages;

namespace Catalyster.Acts
{
    public class DieOnPurposeAct : IAct
    {
        public int Cost { get; set; } = 1000;
        public bool Resolved { get; set; } = false;
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

            var hub = GameMaster.MessageLog.Hub;
            var entity = EntityReference.Value.Entity;

            if (Confirmed)
            {
                hub.Publish(new DeathMessage(this, EntityReference.Value));
                GameMaster.Instance().World.Destroy(entity);
                Resolved = true;
                return this;
            }

            // Suspended set BEFORE publishing in case delegate is instantly called
            Suspended = true;
            hub.Publish(new ConfirmationMessage(entity, "Are you sure you want to die?", b => {
                Confirmed = b;
                Suspended = false;
                }));
            return this;
        }


    }
}
