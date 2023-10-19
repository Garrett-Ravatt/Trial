using Arch.Core;

namespace Catalyster.Interfaces
{
    public interface IDirective
    {
        public int Cost { get; }
        public bool Enter(Entity entity, World world);// TODO: add map
    }
}
