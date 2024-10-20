using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Interfaces;

namespace Catalyster.Components.Directors
{
    // TODO: Possible refactor for the future. **NOT IMPLEMENTED!!!!!!!!!!**
    public struct PlayerDirector : IDirector
    {
        public static IAct? nextAct;
        public IAct? Direct(Entity entity, World world)
        {
            // TODO: set controlling entity
            return nextAct;
        }
    }
}
