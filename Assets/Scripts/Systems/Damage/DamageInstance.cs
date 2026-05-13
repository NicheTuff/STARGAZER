namespace STARGAZER.Systems.Damage
{
    public readonly struct DamageInstance
    {
        public readonly DamageType DamageType;
        public readonly IDamageDealer Source;
        public readonly int Damage;
        public readonly int Pierce;
        public readonly bool QualifiesCrit;
        public DamageInstance(DamageType damageType, IDamageDealer source, int damage, int pierce, bool qualifiesCrit)
        {
            DamageType = damageType;
            Source = source;
            Damage = damage;
            Pierce = pierce;
            QualifiesCrit = qualifiesCrit;
        }
    }
}