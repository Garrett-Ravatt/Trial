﻿using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Components;
using Catalyster.Helpers;
using Catalyster.Interfaces;
using Catalyster.Items;

namespace Catalyster.Acts
{
    public class WalkAct : IAct
    {
        public int Cost { get; set; } = 1000;

        public EntityReference? EntityRef;

        public int? X;
        public int? Y;

        // Won't automatically attack if true
        public bool Passive;

        public WalkAct(EntityReference? e = null, int? x = null, int? y = 0, bool passive = false)
        {
            EntityRef = e;
            X = x;
            Y = y;
            Passive = passive;
        }

        public bool Execute()
        {
            if (!EntityRef.HasValue || !X.HasValue || !Y.HasValue) return false;
            var (entity, x, y) = (EntityRef.Value.Entity, X.Value, Y.Value);
            ref var energy = ref entity.Get<Energy>();

            ref var position = ref entity.Get<Position>();
            var newPos = new Position { X = position.X + x, Y = position.Y + y };

            if (GameMaster.DungeonMap.IsWalkable(newPos.X, newPos.Y))
            {
                Entity? bumped = null;
                if (SpatialHelper.ClearOrAssign(position.X + x, position.Y + y, ref bumped))
                {
                    position = newPos;
                    // TODO: refer to movement speed
                    energy.Points -= WiggleHelper.Wiggle(1000, .1);
                    return true;
                }

                else if (!Passive) // ran into a creature; attack them
                {
                    // TODO: swap places with friendly creature
                    var attackAct = new MeleeAttackAct(EntityRef.Value, bumped.Value.Reference());
                    return attackAct.Execute();
                }
            }

            else if (entity.Has<Player>())
            {
                GameMaster.MessageLog.Add("You bump into the wall. You fool.");
                // TODO: wall bump depth check
            }

            return true;
        }
    }

    public class ThrowAct : IAct
    {
        public int Cost { get; set; } = 1000;

        public EntityReference? EntityRef;
        int? X, Y, I;
        public ThrowAct(EntityReference? e = null, int? x = null, int? y = null, int? i = null)
        {
            EntityRef = e;
            X = x; Y = y; I = i;
        }

        public bool Execute()
        {
            if (!EntityRef.HasValue || !X.HasValue || !Y.HasValue || !I.HasValue)
                return false;
            var (entity, x, y, i) = (EntityRef.Value.Entity, X.Value, Y.Value, I.Value);

            if (entity.Has<Player>() && !GameMaster.DungeonMap.IsInFov(x, y))
                return false;

            ref var energy = ref entity.Get<Energy>();

            var item = entity.Get<Inventory>().Items[i];
            if (item == null)
            {
                Console.WriteLine($"Invalid item index {i} was selected");
                return false;
            }

            // Check for a target.
            {
                Entity? bumped = null;
                if (!SpatialHelper.ClearOrAssign(x, y, ref bumped))
                {
                    // Resolve an attack attempt
                    ActionHelper.ResolveRanged(ItemPropHelper.ThrownAttack(item), bumped.Value);
                }
                item.Entity.Add(new Position { X = x, Y = y });
            }

            // Resolve an explosion
            // TODO: SpatialHash refactor point
            // TODO: refactor explosion resolution into helper somewhere
            var bomb = ItemPropHelper.BombOf(item);
            var bombFormula = bomb.DamageFormula;
            if (bombFormula.MinRoll().Value > 0)
            {
                var radius = bomb.Range;
                for (var _x = x - radius; _x <= x + radius; _x++)
                {
                    for (var _y = y - radius; _y <= y + radius; _y++)
                    {
                        // Check for a target.
                        Entity? bumped = null;
                        if (!SpatialHelper.ClearOrAssign(_x, _y, ref bumped))
                        {
                            // TODO: Some other method than this
                            var rangedAttack = new RangedAttack
                            {
                                Range = 6,
                                AttackFormula = RogueSharp.DiceNotation.Dice.Parse("20"),
                                DamageFormula = bombFormula
                            };
                            // Resolve an attack attempt
                            ActionHelper.ResolveRanged(rangedAttack, bumped.Value, attacker: "Bomb");
                        }
                    }
                }
            }

            energy.Points -= WiggleHelper.Wiggle(1000, .1);
            // TODO: Dispose of the thrown item entity and contents
            entity.Get<Inventory>().Items.RemoveAt(i);

            return true;
        }
    }

    public class MeleeAttackAct : IAct
    {
        public int Cost { get; set; } = 1000;

        public EntityReference? Attacker;
        public EntityReference? Defender;
        public MeleeAttack? Attack;

        public MeleeAttackAct(EntityReference? attacker = null, EntityReference? defender = null, MeleeAttack? attack = null)
        {
            Attacker = attacker;
            Defender = defender;
            Attack = attack;
        }

        public bool Execute()
        {
            Entity atkr;
            Entity def;
            MeleeAttack att;
            if (!Defender.HasValue || !Attacker.HasValue || !Defender.Value.IsAlive())
                return false;
            else if (!Attack.HasValue)
            {
                (atkr, def) = (Attacker.Value.Entity, Defender.Value.Entity);
                ActionHelper.ResolveAttack(atkr, def);
            }
            else
            {
                (atkr, def, att) = (Attacker.Value.Entity, Defender.Value.Entity, Attack.Value);
                var name = "";
                if (atkr.Has<Token>())
                    ActionHelper.ResolveMelee(att, def, name);
            }
            ref var e = ref atkr.Get<Energy>();
            e.Points -= WiggleHelper.Wiggle(Cost, 0.1);
            return true;
        }
    }
}
