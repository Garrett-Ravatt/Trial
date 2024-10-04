using Arch.Core;
using System.Runtime.InteropServices;

namespace Catalyster.Interfaces
{
    public interface IDirective
    {
        public int Cost { get; }
        public bool Enter(EntityReference entityref);
        public bool Enter(World world, Entity entity)
        {
            return Enter(world.Reference(entity));
        }
        // TODO: get the contained Act
    }
}
