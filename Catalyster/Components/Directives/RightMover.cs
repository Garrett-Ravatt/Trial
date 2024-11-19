using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Interfaces;
using Catalyster.Acts;

namespace Catalyster.Components.Directives
{
    public struct RightMover : IDirective
    {
        public int Cost { get; set; } = 1000;
        public bool Passive { get; set; } = false;
        public RightMover() { }
        public IAct Act(EntityReference entityref) { return new WalkAct(entityref, x: 1, y: 0, passive: Passive); }
        public IAct? Enter(EntityReference entityref)
        {
            var entity = entityref.Entity;
            // Fail out if we can't perform the action.
            ref var stats = ref entity.Get<Stats>();
            if (stats.Energy <= 0) return null;

            var moveAct = (WalkAct)Act(entityref);
            return moveAct;
        }
    }
}
