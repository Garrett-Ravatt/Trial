using Arch.Core;

namespace Catalyster.Core
{
    public class Command
    {
        // The Entity being controlled
        // TODO: use eventbus instead
        public Entity? Entity;
        public Command() { }
    }
}
