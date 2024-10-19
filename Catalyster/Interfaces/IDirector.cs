using Arch.Core;

namespace Catalyster.Interfaces
{
    public interface IDirector
    {
        //TODO: refactor form
        public IAct? Direct(Entity entity, World world);
    }
}
