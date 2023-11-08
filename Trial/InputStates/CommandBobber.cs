
namespace Trial.InputStates
{
    public static class CommandBobber
    {
        private enum CommandState
        {
            None,
            Throw,
        }

        private static CommandState _state = CommandState.None;

        private static void SetState(CommandState value)
        {
            if (_state != value)
            {
                // Teardown Previous State
                switch (_state)
                {
                    case CommandState.Throw:
                        ThrowX = null;
                        ThrowY = null;
                        ThrowIndex = null;
                        break;
                }

                // Initialize New State (if necessary)
                switch (value)
                {
                    case CommandState.None:
                        Program.GameMaster.Update();
                        Program.GameMaster.Update();
                        Program.Draw();
                        break;
                    case CommandState.Throw:
                        break;
                }

                _state = value;
            }
        }

        // ### Throw ###

        public static int? ThrowX;
        public static int? ThrowY;
        public static int? ThrowIndex;

        public static void Throw(int? x = null, int? y = null, int? index = null)
        {
            SetState(CommandState.Throw);

            if (x != null) { ThrowX = x; }
            if (y != null) { ThrowY = y; }
            if (index != null) { ThrowIndex = index; }

            if (ThrowX.HasValue && ThrowY.HasValue && ThrowIndex.HasValue)
            {
                Program.GameMaster.Command.Throw(ThrowX.Value, ThrowY.Value, ThrowIndex.Value);
                SetState(CommandState.None);
            }
        }
    }
}
