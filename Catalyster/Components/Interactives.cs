using Catalyster.Interfaces;
using System.Linq.Expressions;

namespace Catalyster.Components
{
    public enum DoorState { OPEN, CLOSED }
    public struct Door { public DoorState state; }
    
    // NOTE: Is a container because it may become a list
    public struct InterAct { public IInterAct act; }
}
