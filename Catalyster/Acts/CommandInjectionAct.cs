using Arch.Core;
using Catalyster.Interfaces;

namespace Catalyster.Acts
{
    public class CommandInjectionAct : IAct
    {
        public int Cost { get; set; } = 0;
        public EntityReference? Acting { get; set; } = null;
        public bool Resolved { get; set; } = false;
        public bool Suspended { get { if (Resolved) return false; return InjectedAct == null; } }
        public static IAct? InjectedAct { get; set; } = null;

        public CommandInjectionAct(IAct? injectedAct = null, EntityReference? acting = null)
        {
            if (injectedAct != null)
                InjectedAct = injectedAct;
            if (acting != null)
                Acting = acting;
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
