using STARGAZER.Systems.Damage;
using STARGAZER.Systems.Stats.StatProfiles;
using UnityEngine;
using System;

namespace STARGAZER.Entities
{
    public abstract class Entity : MonoBehaviour, IDamageable
    {
        public virtual HealthComponent HealthComponent { get; protected set; } = new HealthComponent(100, 100);
        public virtual DamageProfile DamageProfile { get; protected set; } = new DamageProfile();
        public virtual DefenseProfile DefenseProfile { get; protected set; } = new DefenseProfile();

        public virtual bool CanCrit(DamageInstance incomingAttack) => true;
        public virtual HitData TakeHit(DamageInstance[] incomingAttack)
        {
            bool critLands = incomingAttack[0].QualifiesCrit && CanCrit(incomingAttack[0]);
            var appliedAttack = critLands ? incomingAttack[1] : incomingAttack[0];
            int effectiveArmor = DefenseProfile.GetTypeArmor(appliedAttack.DamageType) - appliedAttack.Pierce;
            int damageReceived = appliedAttack.Damage - effectiveArmor;

            HealthComponent.Hurt(damageReceived);

            if (critLands)
            {
                OnReceiveCrit();
            }
            if (HealthComponent.CurrentHealth <= 0)
            {
                OnDeath();
            }

            return new HitData(this, damageReceived, critLands);
        }
        public virtual void OnReceiveCrit() { }
        public virtual void OnDeath() { }
    }
    public class HealthComponent
    {
        public int MaxHealth { get; private set; }
        public int CurrentHealth { get; private set; }
        public HealthComponent(int maxHealth, int currentHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = currentHealth;
        }
        public virtual void Heal(int healValue) => CurrentHealth = Math.Clamp(CurrentHealth + healValue, 0, MaxHealth);
        public virtual void Hurt(int hurtValue) => CurrentHealth = Math.Clamp(CurrentHealth - hurtValue, 0, MaxHealth);
    }
}