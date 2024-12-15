using Arch.Core;

namespace Catalyster.Interfaces
{
    public interface IInterAct : IAct
    {
        public EntityReference? Subject { get; set; }
        // TODO: Refactor to use Directives made for interacts and not this
        public IInterAct Clone();
    }
}
