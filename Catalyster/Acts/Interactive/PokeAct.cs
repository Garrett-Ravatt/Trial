
using Arch.Core;
using Catalyster.Interfaces;

namespace Catalyster.Acts.Interactive
{
    public class PokeAct : IInterAct
    {
        public EntityReference? Acting {  get; set; }
        public bool Resolved { get; set; }
        public bool Suspended { get; set; }
        public EntityReference? Subject { get; set; }

        public PokeAct(EntityReference? acting = null, EntityReference? subject = null)
        {
            Acting = acting;
            Subject = subject;
        }

        public IAct Execute()
        {
            // TODO: find a purpose for this interaction
            if (!Acting.HasValue || !Acting.Value.IsAlive() || !Subject.HasValue || !Subject.Value.IsAlive())
                throw new InvalidOperationException($"{this} executed with invalid entity references {Acting}, {Subject}");

            Resolved = true;
            return this;
        }

        public IInterAct Clone()
        {
            return (IInterAct) MemberwiseClone();
        }
    }
}
