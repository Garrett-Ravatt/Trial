using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Interfaces;

namespace Catalyster.Components.Directors
{
    // Does one thing over and over
    public struct MonoBehavior : IDirector
    {
        public IDirective Directive;
        public IAct? Direct(Entity entity, World world)
        {
            var act = Directive.Enter(entity.Reference());
            return act;
        }
    }
}
