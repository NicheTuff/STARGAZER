using Assets.Scripts.Systems.Stats;

namespace Assets.Scripts.Systems.Damage
{
    public interface IDamageDealer
    {
        public DamageInstance OnCrit(DamageInstance refinedPreCrit, DamageProfile damageStats)
        {
            float critDamage = refinedPreCrit.Damage * (1 + damageStats.CritModifiers.Percent);
            return new DamageInstance(refinedPreCrit.DamageType, (int)critDamage, refinedPreCrit.Pierce, true);
        }
        public DamageInstance[] RefineDamage(DamageInstance rawDamage, DamageProfile damageStats)
        {
            float damage = damageStats.GetTypeModifiers(rawDamage.DamageType).Apply(rawDamage.Damage);
            float pierce = damageStats.PierceModifiers.Apply(rawDamage.Pierce);

            var preCrit = new DamageInstance(rawDamage.DamageType, (int)damage, (int)pierce, rawDamage.QualifiesCrit);
            var postCrit = OnCrit(preCrit, damageStats);

            return new DamageInstance[] { preCrit, postCrit };
        }
    }
}