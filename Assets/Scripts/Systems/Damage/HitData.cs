namespace Assets.Scripts.Systems.Damage
{
    // Final damage returned by a damage calc operation
    public readonly struct HitData
    {
        public readonly IDamageable Victim;
        public readonly int DamageTaken;
        public readonly bool WasCrit;
        public HitData(IDamageable victim, int damageTaken, bool wasCrit)
        {
            Victim = victim;
            DamageTaken = damageTaken;
            WasCrit = wasCrit;
        }
    }
}