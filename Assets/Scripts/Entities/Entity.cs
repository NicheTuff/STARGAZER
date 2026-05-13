using STARGAZER.Systems.Damage;
using STARGAZER.Systems.Stats.StatProfiles;
using UnityEngine;
using System;

namespace STARGAZER.Entities
{
    public abstract class Entity : MonoBehaviour, IDamageable
    {
        /// <summary>
        /// Responsible for controlling an entities health. Overload to modify an entity's max or starting health.
        /// </summary>
        public virtual HealthComponent HealthComponent { get; protected set; } = new HealthComponent(100, 100);

        public virtual DamageProfile DamageProfile { get; protected set; } = new DamageProfile();

        public virtual DefenseProfile DefenseProfile { get; protected set; } = new DefenseProfile();

        public bool AllowsCrit(DamageInstance incomingAttack) => true;

        public virtual HitData TakeHit(DamageInstance incomingAttack)
        {
            var profile = incomingAttack.Source.DamageProfile;
            var critResult = DefenseProfile.Resolver.ResolveCrit(incomingAttack, this);

            // Effective armor can become negative when faced with enough pierce and this will increase the damage received.
            // This behavior is intended.
            int damageReceived = DefenseProfile.Resolver.ApplyArmor(critResult.Attack, DefenseProfile.Stats);

            HealthComponent.Hurt(damageReceived);

            if (critResult.Lands)
                OnReceiveCrit();
            if (HealthComponent.CurrentHealth <= 0)
                OnDeath();

            return new HitData(this, incomingAttack, critResult.Attack, damageReceived, critResult.Lands);
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

        /// <summary>
        /// Increases CurrentHealth by healValue, clamped between 0 and MaxHealth.<br/>
        /// A negative healValue argument will reduce CurrentHealth. If you don't want this to happen, ensure the healValue argument cannot go below zero.
        /// </summary>
        public virtual void Heal(int healValue) => CurrentHealth = Math.Clamp(CurrentHealth + healValue, 0, MaxHealth);

        /// <summary>
        /// Decreases CurrentHealth by hurtValue, clamped between 0 and MaxHealth.<br/>
        /// A negative hurtValue argument will increase CurrentHealth. If you don't want this to happen, ensure the hurtValue argument cannot go below zero.
        /// </summary>
        public virtual void Hurt(int hurtValue) => CurrentHealth = Math.Clamp(CurrentHealth - hurtValue, 0, MaxHealth);
    }
}