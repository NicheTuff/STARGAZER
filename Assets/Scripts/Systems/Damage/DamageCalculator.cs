using STARGAZER.Systems.Stats.StatProfiles;

namespace STARGAZER.Systems.Damage
{
	public class DamageCalculator
	{
		public DamageInstance Refine(DamageInstance rawDamage)
		{
            float damage = rawDamage.Source.DamageProfile.Stats.GetTypeModifiers(rawDamage.DamageType).Apply(rawDamage.Damage);
            float pierce = rawDamage.Source.DamageProfile.Stats.PierceModifiers.Apply(rawDamage.Pierce);

            var nonCritInstance = new DamageInstance(rawDamage.DamageType, rawDamage.Source, (int)damage, (int)pierce, rawDamage.QualifiesCrit);

            return nonCritInstance;
        }
        public DamageInstance ApplyCrit(DamageInstance refinedPreCrit)
        {
            float critDamage = refinedPreCrit.Damage * (1 + refinedPreCrit.Source.DamageProfile.Stats.CritModifiers.Percent);
            return new DamageInstance(refinedPreCrit.DamageType, refinedPreCrit.Source, (int)critDamage, refinedPreCrit.Pierce, true);
        }
        public static readonly DamageCalculator Instance = new();
    }
}