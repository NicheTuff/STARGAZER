namespace STARGAZER.Systems.Damage
{
    public readonly struct DamageInstance
    {
        public readonly DamageType DamageType;
        public readonly IDamageDealer Source;
        public readonly int Damage;
        public readonly int Pierce;
        public readonly bool QualifiesCrit;
        public readonly bool ForceCrit;
        public DamageInstance(DamageType damageType, IDamageDealer source, int damage, int pierce, bool qualifiesCrit, bool forceCrit)
        {
            DamageType = damageType;
            Source = source;
            Damage = damage;
            Pierce = pierce;
            QualifiesCrit = qualifiesCrit;
            ForceCrit = forceCrit;
        }
    }
}