using STARGAZER.Systems.Damage;
using System.Collections.Generic;
using System;

namespace STARGAZER.Systems.Stats.StatProfiles
{
    public class DefenseProfile
    {
        public DamageResolver Resolver = DamageResolver.Instance;
        public DefenseStats Stats = new DefenseStats();
    }
    public class DefenseStats
    {
        public Dictionary<DamageType, StatModifier> armorModifiers = new();
        private StatModifier GetArmorModifiers(DamageType damageType)
        {
            if (!armorModifiers.TryGetValue(damageType, out var modifiers))
            {
                modifiers = new StatModifier();
                armorModifiers[damageType] = modifiers;
            }
            return modifiers;
        }
        private Dictionary<DamageType, int> typeArmors = new();
        public int GetTypeArmor(DamageType damageType)
        {
            var armorStats = GetArmorModifiers(damageType);
            if (!typeArmors.TryGetValue(damageType, out var armor))
            {
                armor = (int)(armorStats.Additive * (1 + armorStats.Percent) * armorStats.Multiplier);
                typeArmors[damageType] = armor;
            }
            return armor;
        }
    }
}