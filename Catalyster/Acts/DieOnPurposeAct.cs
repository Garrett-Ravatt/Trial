using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Interfaces;
using Catalyster.Messages;

namespace Catalyster.Acts
{
    public class DieOnPurposeAct : IAct
    {
        public int Cost { get; set; } = 1000;
        public EntityReference? Acting { get; set; }
        public bool Resolved { get; set; } = false;
        public bool Suspended { get; set; } = false;

        public bool Confirmed = false;

        public DieOnPurposeAct(EntityReference? entityReference)
        {
            Acting = entityReference;
        }

        public IAct Execute()
        {
            if (Acting == null || !Acting.Value.IsAlive())
            {
                throw new Exception($"Acting entity is not valid: {Acting}");
            }

            var hub = GameMaster.Instance().MessageLog.Hub;
            var entity = Acting.Value.Entity;

            if (Confirmed)
            {
                hub.Publish(new DeathMessage(this, Acting.Value));
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
