using Catalyster.Interfaces;

namespace Catalyster.Acts
{
    public struct CommandInjectionAct : IAct
    {
        public int Cost { get; set; } = 1000;
        public bool Resolved { get; set; } = false;
        public bool Suspended { get { return InjectedAct == null && !Resolved; } }
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
            // return static member to null before exiting
            var act = InjectedAct;
            InjectedAct = null;
            return act;
        }
    }
}
