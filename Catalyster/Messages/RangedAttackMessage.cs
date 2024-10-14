using Arch.Core;
using TinyMessenger;

namespace Catalyster.Messages
{
    public class RangedAttackMessage : TinyMessageBase
    {
        public EntityReference Attacker;
        public EntityReference Defender;
        public int ToHit, Damage;
        public bool Hit;
        public RangedAttackMessage(object sender, EntityReference attacker, EntityReference defender,
            int toHit, int damage = 0,
            bool hit = true) : base(sender)
        {
            Attacker = attacker;
            Defender = defender;
            ToHit = toHit;
            Damage = damage;
            Hit = hit;
        }
    }
}
