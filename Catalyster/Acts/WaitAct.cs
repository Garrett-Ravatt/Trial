using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Components;
using Catalyster.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalyster.Acts
{
    public class WaitAct : IAct
    {
        public int Cost {get;} = 0;
        public bool Resolved { get; set; } = false;
        public bool Suspended { get; set; } = false;
        public EntityReference? EntityRef;
        public WaitAct(EntityReference? entityRef = null)
        {
            EntityRef = entityRef;
        }

        public IAct Execute()
        {
            // TODO: Malformed error
            //if (EntityRef == null)
            
            var entity = EntityRef.Value.Entity;

            entity.Get<Energy>().Points = 0;
            Resolved = true;
            return this;
        }
    }
}
