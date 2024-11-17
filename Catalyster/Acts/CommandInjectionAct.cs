using Catalyster.Interfaces;

namespace Catalyster.Acts
{
    public struct CommandInjectionAct : IAct
    {
        public int Cost { get; set; } = 0;
        public bool Resolved { get; set; } = false;
        public bool Suspended { get { if (Resolved) return false; return InjectedAct == null; } }
        public static IAct? InjectedAct { get; set; } = null;

        public CommandInjectionAct(IAct? injectedAct = null)
        {
            if (injectedAct != null)
                InjectedAct = injectedAct;
        }

        public IAct Execute()
        {
            if (InjectedAct == null)
            {
                return this;
            }
            Resolved = true;
            // return static member to null before exiting
            var act = InjectedAct;
            InjectedAct = null;
            return act;
        }
    }
}
