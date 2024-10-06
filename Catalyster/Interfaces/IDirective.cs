using Arch.Core;

namespace Catalyster.Interfaces
{
    public interface IDirective
    {
        public IAct Act(EntityReference entityref);
        public bool Enter(EntityReference entityref);
        public bool Enter(World world, Entity entity)
        {
            return Enter(world.Reference(entity));
        }
        // TODO: get the contained Act (?)
    }
}
