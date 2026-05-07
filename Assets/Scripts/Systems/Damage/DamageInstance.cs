namespace Assets.Scripts.Systems.Damage
{
    public readonly struct DamageInstance
    {
        public readonly DamageType DamageType;
        public readonly int Damage;
        public readonly int Pierce;
        public readonly bool QualifiesCrit;
        public DamageInstance(DamageType damageType, int damage, int pierce, bool qualifiesCrit)
        {
            DamageType = damageType;
            Damage = damage;
            Pierce = pierce;
            QualifiesCrit = qualifiesCrit;
        }
    }
}