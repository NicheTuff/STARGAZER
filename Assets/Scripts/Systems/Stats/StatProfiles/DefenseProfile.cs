using Assets.Scripts.Systems.Damage;
using System.Collections.Generic;
using System;

namespace Assets.Scripts.Systems.Stats.StatProfiles
{
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
