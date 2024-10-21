using Catalyster.Interfaces;

namespace Catalyster.Acts
{
    public struct CommandInjectionAct : IAct
    {
        public int Cost { get; set; } = 1000;
        public bool Resolved { get; set; } = false;
        public bool Suspended { get { return !Resolved && InjectedAct == null; } }
        public static IAct? InjectedAct { get; set; }

        public CommandInjectionAct(IAct? injectedAct = null)
        {
            InjectedAct = injectedAct;
        }

        public IAct Execute()
        {
            if (InjectedAct == null)
            {
                return this;
            }
            Resolved = true;
            return InjectedAct;
        }
    }
}
