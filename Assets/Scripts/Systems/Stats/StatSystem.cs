using Assets.Scripts.Systems.Damage;
using System.Collections.Generic;
using System;

namespace Assets.Scripts.Systems.Stats
{
    public class StatModifier
    {
        public int Additive;
        public float Percent;
        public float Multiplier = 1f;
        public float Apply(float value)
        {
            value += Additive;
            value *= 1 + Percent;
            value *= Multiplier;
            return value;
        }
    }

    /// <summary>
    /// Modifiers applied to an entity's outgoing attack during the damage refinement process.
    /// Attack modifiers are applied in ASCENDING degree (additive first, then percent, then multiplicative).
    /// </summary>
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

    public class DefenseProfile
    {
        public int MaxHealth { get; private set; }
        public int Health { get; private set; }
        public DefenseProfile(int maxHealth, int health)
        {
            MaxHealth = maxHealth;
            Health = health;
        }

        public void Heal(int healValue) => Health = Math.Clamp(Health + healValue, 0, MaxHealth);
        public void Damage(int damageValue) => Health = Math.Clamp(Health - damageValue, 0, MaxHealth);

        private Dictionary<DamageType, int> typeArmors = new();
        public int GetTypeArmor(DamageType damageType)
        {
            if (!typeArmors.TryGetValue(damageType, out var armor))
            {
                armor = 0;
                typeArmors[damageType] = armor;
            }
            return armor;
        }
    }
}
