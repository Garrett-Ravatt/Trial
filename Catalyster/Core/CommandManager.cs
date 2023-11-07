using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalyster.Core
{
    // NOTE: Is in progress pending a refactor.
    // TODO: Move to Muck or refactor Command
    public static class CommandManager
    {
        private enum CommandState
        {
            None,
            Throw,
        }

        public static int? ThrowX {
            get { return ThrowX; }
            set
            {
                ThrowX = value;
                //_checkThrow();
            }
        }
        public static int? ThrowY;
        public static int? ThrowIndex;

        private static CommandState _state
        {
            get { return _state; }
            set {
                switch(_state)
                {
                    case CommandState.Throw:
                        ThrowX = null;
                        ThrowY = null;
                        ThrowIndex = null;
                        break;
                }
                switch(value)
                {
                    case CommandState.Throw:
                        break;
                }
            }
        }

        //private static void _checkThrow()
        //{
        //    if (ThrowX.HasValue && ThrowY.HasValue && ThrowIndex.HasValue)
        //        GameMaster.Command.Throw(ThrowX.Value, ThrowY.Value, ThrowIndex.Value);
        //}
    }
}
