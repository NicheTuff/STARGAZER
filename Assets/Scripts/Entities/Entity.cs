using Assets.Scripts.Systems.Damage;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public abstract class Entity : MonoBehaviour, IDamageable
    {
        public abstract DefenseStats DefenseStats { get; set; }
        public DamageStats DamageStats;
        public void OnDeath() { }
    }
}