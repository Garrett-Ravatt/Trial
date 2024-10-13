using TinyMessenger;

namespace Catalyster.Messages
{
    public class MeleeAttackMessage : TinyMessageBase
    {
        public string Attacker;
        public int ToHit, Damage;
        public bool Hit;
        public MeleeAttackMessage(object sender, string attacker, 
            int toHit, int damage,
            bool hit = true) : base(sender)
        {
            Attacker = attacker;
            ToHit = toHit;
            Damage = damage;
            Hit = hit;
        }
    }
}
