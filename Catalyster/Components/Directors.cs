using Arch.Core;
using Arch.Core.Extensions;
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

    // Two brain cell hunter
    public struct CrazedHunter : IDirector
    {
        public void Direct(Entity entity, World world)
        {
            ref var pos = ref entity.Get<Position>();

            // does nothing if the player can't see it.
            if (!GameMaster.DungeonMap.IsInFov(pos.X, pos.Y))
                return;


        }
    }
}
