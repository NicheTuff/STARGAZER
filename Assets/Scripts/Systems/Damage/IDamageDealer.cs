using STARGAZER.Systems.Stats.StatProfiles;

namespace STARGAZER.Systems.Damage
{
    public interface IDamageDealer
    {
        public DamageProfile DamageProfile { get; }
        public DamageCalculator DamageCalculator { get; }
        public void OnHit(IDamageable victim);
        public void PostHit(HitData hitData);
    }
}