using Arch.Core;

namespace Catalyster.Interfaces
{
    public interface IAct
    {
        public int Cost { get; }
        public bool Resolved {  get; }
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