using Arch.Core;

namespace Catalyster.Interfaces
{
    public interface IAct
    {
        public int Cost { get; }
        public bool Enter(Entity entity, World world);// TODO: add map or remove world
    }
}