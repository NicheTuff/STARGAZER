using System.Collections.Generic;

namespace Assets.Scripts.Systems.Damage
{
    // TODO:
    // This whole system should be distributed across many files
    // Obviously increase functionality

    // Placeholder types
    public enum DamageType
    {
        Physical,
        Spirit
    }

    public struct DamageModifier
    {
        public int Additive;
        public float Percent;
        public float Multiplier;
    }

    // Contains info about an entities damage stats
    // Should be compositionally related with entities that deal damage 
    public class DamageProfile
    {
        private Dictionary<DamageType, DamageModifier> modifiers;

        public DamageModifier GetModifiers(DamageType damageType)
        {
            if (!modifiers.TryGetValue(damageType, out var modifier))
            {
                modifier = new DamageModifier();
                modifiers[damageType] = modifier;
            }
            return modifier;
        }
    }

    // Uncalculated damage instance
    public class RawDamage
    {
        public DamageType DamageType;
        public int Damage;
    }

    // Final damage returned by a damage calc operation
    public class RefinedDamage
    {
        public int Damage {get; private set;}
        public bool IsCrit { get; private set;}
    }

    // doesnt do anything yet llol
    public class DamageCalculator { }
}