using Catalyster.Interfaces;

namespace Catalyster.Components
{
    public enum DoorState { OPEN, CLOSED }
    public struct Door { public DoorState state; }
    
    public struct InterAct { public IAct act; }
}
