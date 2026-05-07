using System;
using System.Collections.Generic;

namespace Assets.Scripts.Systems.Damage
{
    // TODO:
    // This whole system should be distributed across many files

    public enum DamageType
    {
        Physical,
        Spirit
    }

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
    public class DamageStats
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

    public class DefenseStats
    {
        public int MaxHealth;
        public int Health;
        public DefenseStats(int maxHealth, int health)
        {
            MaxHealth = maxHealth;
            Health = health;
        }

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

    public interface IDamageDealer
    {
        public DamageInstance OnCrit(DamageInstance refinedPreCrit, DamageStats damageStats)
        {
            float critDamage = refinedPreCrit.Damage * (1 + damageStats.CritModifiers.Percent);
            return new DamageInstance(refinedPreCrit.DamageType, (int)critDamage, refinedPreCrit.Pierce, true);
        }
        public DamageInstance[] RefineDamage(DamageInstance rawDamage, DamageStats damageStats)
        {
            float damage = damageStats.GetTypeModifiers(rawDamage.DamageType).Apply(rawDamage.Damage);

            float pierce = damageStats.PierceModifiers.Apply(rawDamage.Pierce);

            var preCrit = new DamageInstance(rawDamage.DamageType, (int)damage, (int)pierce, rawDamage.QualifiesCrit);

            var postCrit = OnCrit(preCrit, damageStats);

            return new DamageInstance[] { preCrit, postCrit };
        }
    }

    public interface IDamageable
    {
        public bool CritRequirement() => true;
        public HitData TakeDamage(DamageInstance[] incomingAttack, DefenseStats defenseStats)
        {
            bool critLands = incomingAttack[0].QualifiesCrit && CritRequirement();
            var appliedAttack = critLands ? incomingAttack[1] : incomingAttack[0];

            int effectiveArmor = defenseStats.GetTypeArmor(appliedAttack.DamageType) - appliedAttack.Pierce;
            int damageReceived = appliedAttack.Damage - effectiveArmor;

            defenseStats.Health = Math.Clamp(defenseStats.Health - damageReceived, 0, defenseStats.MaxHealth);

            if (critLands)
            {
                OnReceiveCrit();
            }
            if (defenseStats.Health <= 0)
            {
                OnDeath();
            }

            return new HitData(this, damageReceived, critLands);
        }
        public void OnReceiveCrit() { }
        public void OnDeath();
    }

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