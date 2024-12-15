using Arch.Core;
using Catalyster.Interfaces;
using Catalyster.Components.Extensions;
using Arch.Core.Extensions;
using Catalyster.Components;
using Catalyster.Helpers;

namespace Catalyster.Acts.Interactive
{
    public class UseDoorAct : IInterAct
    {
        public EntityReference? Acting {  get; set; }
        public bool Resolved { get; set; }
        public bool Suspended {  get; set; }

        // the door
        public EntityReference? Subject { get; set; }

        public int Cost = 300;

        public UseDoorAct(EntityReference? acting = null, EntityReference? door = null)
        {
            Acting = acting;
            Subject = door;
        }

        public IAct Execute()
        {
            if (!Acting.HasValue || !Acting.Value.IsAlive() || !Subject.HasValue || !Subject.Value.IsAlive())
                throw new InvalidOperationException($"{this} executed with invalid entity references {Acting}, {Subject}");
            
            var a = Acting.Value.Entity;
            ref var apos = ref a.Get<Position>();
            ref var stats = ref a.Get<Stats>();

            var s = Subject.Value.Entity;
            ref var p = ref s.Get<Position>();
            ref var t = ref s.Get<Token>();
            ref var d = ref s.Get<Door>();

            switch (d.state)
            {
                case DoorState.OPEN:
                    if (apos.Equals(p)) // no close if standing in the door
                        break;
                    stats.Energy -= WiggleHelper.Wiggle(Cost, .1);
                    s.Get<Door>().state.SetUpdate(DoorState.CLOSED, p, ref t);
                    break;
                case DoorState.CLOSED:
                    stats.Energy -= WiggleHelper.Wiggle(Cost, .1);
                    s.Get<Door>().state.SetUpdate(DoorState.OPEN, p, ref t);
                    break;
            }

            Resolved = true;
            return this;
        }

        public IInterAct Clone()
        {
            return (IInterAct)MemberwiseClone();
        }
    }
}
