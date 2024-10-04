using Arch.Core;

namespace Catalyster.Interfaces
{
    public interface IAct
    {
        public int Cost { get; }
        public bool Execute();
    }
}