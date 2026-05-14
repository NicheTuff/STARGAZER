namespace STARGAZER.Systems.Damage
{
    public interface IDamageable
    {
        public bool AllowsCrit(DamageInstance incomingAttack);
        public bool ForceCrit(DamageInstance incomingAttack);
        public HitData TakeHit(DamageInstance incomingAttack);
        public void OnReceiveCrit();
        public void OnDeath();
    }
}