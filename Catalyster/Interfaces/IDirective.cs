using Arch.Core;

namespace Catalyster.Interfaces
{
    public interface IDirective
    {
        // Unformed Act
        public IAct Act(EntityReference entityref);

        // Fully Formed Act
        public IAct? Enter(EntityReference entityref);
        public IAct? Enter(World world, Entity entity)
        {
            return Enter(world.Reference(entity));
        }
    }
}
