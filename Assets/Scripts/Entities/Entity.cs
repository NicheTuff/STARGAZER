using Assets.Scripts.Systems.Damage;
using Assets.Scripts.Systems.Stats.StatProfiles;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public abstract class Entity : MonoBehaviour, IDamageable
    {
        public abstract DefenseProfile DefenseProfile { get; set; }
        public DamageProfile DamageProfile;
        public void OnDeath() { }
    }
}