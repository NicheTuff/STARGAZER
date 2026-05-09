using STARGAZER.Systems.Damage;
using System.Collections.Generic;

namespace STARGAZER.Systems.Stats.StatProfiles
{
    public class DamageProfile
    {
        private Dictionary<DamageType, StatModifier> typeModifiers = new();
        public StatModifier GetTypeModifiers(DamageType damageType)
        {
            if (!typeModifiers.TryGetValue(damageType, out var modifier))
            {
                modifier = new StatModifier();
                typeModifiers[damageType] = modifier;
            }
            return modifier;
        }

        public StatModifier PierceModifiers = new StatModifier();
        public StatModifier CritModifiers = new StatModifier { Percent = 1f };
    }
}