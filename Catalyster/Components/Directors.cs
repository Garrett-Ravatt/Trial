using Arch.Core;
using Catalyster.Interfaces;

namespace Catalyster.Components
{
    // Does one thing over and over
    public struct MonoBehavior : IDirector
    {
        public IDirective Directive;
        public void Direct(Entity entity, World world)
        {
            // Run until Directive fails :)
            while (Directive.Enter(entity, world)) {}
        }
    }
}
