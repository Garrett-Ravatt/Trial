using Arch.Core;
using Arch.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalyster.Helpers
{
    public static class QueryHelper
    {
        public static List<EntityReference> ListByComponent<T>()
        {
            var list = new List<EntityReference>();
            var desc = new QueryDescription().WithAll<T>();

            GameMaster.World.Query(in desc, (Entity e) =>
            {
                list.Add(e.Reference());
            });

            return list;
        }
    }
}
