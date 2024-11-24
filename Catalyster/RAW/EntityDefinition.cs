using Arch.Core;

namespace Catalyster.RAW
{
    public class EntityDefinition
    {
        public string Name;
        public string Description;
        public EntityReference EntityReference;

        public EntityDefinition(string name, string description, EntityReference entityReference)
        {
            Name = name;
            Description = description;
            EntityReference = entityReference;
        }
    }
}
