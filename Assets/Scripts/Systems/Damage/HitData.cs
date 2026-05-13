namespace STARGAZER.Systems.Damage
{
    // Final damage returned by a damage calc operation
    public readonly struct HitData
    {
        public readonly IDamageable Victim;
        public readonly DamageInstance IncomingAttack;
        public readonly DamageInstance AppliedAttack;
        public readonly int DamageTaken;
        public readonly bool WasCrit;
        public HitData(IDamageable victim, DamageInstance incomingAttack, DamageInstance appliedAttack, int damageTaken, bool wasCrit)
        {
            Victim = victim;
            IncomingAttack = incomingAttack;
            AppliedAttack = appliedAttack;
            DamageTaken = damageTaken;
            WasCrit = wasCrit;
        }
    }
}