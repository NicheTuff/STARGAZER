using STARGAZER.Systems.Damage;
using System.Collections.Generic;
using System;

namespace STARGAZER.Systems.Stats.StatProfiles
{
    public class DefenseProfile
    {

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