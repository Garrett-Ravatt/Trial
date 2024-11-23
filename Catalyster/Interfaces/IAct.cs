using Arch.Core;

namespace Catalyster.Interfaces
{
    public interface IAct
    {
        // TODO: Do we need this? For what? Can't the Act do this themselves?
        public int Cost { get; }
        public EntityReference? Acting { get; set; }
        public bool Resolved { get; }
        public bool Suspended { get; }
        public IAct Execute();
        
    }
    public static class ActExtensions
    {
        public static IAct Consume(this IAct act)
        {
            while (!act.Resolved && !act.Suspended)
                act = act.Execute();
            return act;
        }
    }
}