﻿using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Acts.ItemManipulation;
using Catalyster.Components;
using Catalyster.Helpers;
using Catalyster.Interfaces;
using Catalyster.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalyster.Acts
{
    public class UseAct : IAct
    {
        public EntityReference? Acting { get; set; }
        public bool Suspended { get; set; }
        public bool Resolved { get; set; }

        public UseAct(EntityReference? acting = null)
        {
            Acting = acting;
        }
        public IAct Execute()
        {
            if (!Acting.HasValue || !Acting.Value.IsAlive())
                throw new InvalidOperationException($"Invalid Entity for {this} {Acting}");

            var e = Acting.Value.Entity;
            var position = e.Get<Position>();

            IInterAct? interaction = null;
            // TODO: choose from available Acts
            GameMaster.Instance().World.Query(
                in new QueryDescription().WithAll<Position, InterAct>(),
                (Entity entity, ref Position pos, ref InterAct interAct) =>
                {
                    if (SpatialHelper.LazyDist(position, pos) <= 1)
                    {
                        // TODO: SpatialHash refactor point
                        // TODO: Use Command Buffer inside query
                        if (interaction != null)
                            Console.WriteLine($"{this} is complaining there is more than one InterAct");
                        interaction = interAct.act.Clone();
                        interaction.Subject = entity.Reference();
                    }
                });

            if (interaction != null)
            {
                interaction.Acting = Acting;
                return interaction;
            }
            // if nothing to use, collect items
            else
            {
                return new ItemCollectAct(Acting.Value);
            }

            Resolved = true;
            return this;
        }
    }
}
