using Arch.Core;
using Catalyster.Acts;
using Catalyster.Interfaces;

namespace Catalyster.Components.Directors
{
    public struct PlayerDirector : IDirector
    {
        public IAct? Direct(Entity entity, World world)
        {
            // TODO: set controlling entity
            return new CommandInjectionAct();
        }
    }
}
