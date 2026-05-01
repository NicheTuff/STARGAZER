using Assets.Scripts.Systems.Damage;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public abstract class Entity : MonoBehaviour
    {
        public abstract int MaxHealth { get; protected set; }
        public int Health { get; protected set; }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            Health = MaxHealth;
        }

        // Largely placeholder
        private void TakeDamage(RefinedDamage damage)
        {
            Health -= damage.Damage;
        }
    }
}