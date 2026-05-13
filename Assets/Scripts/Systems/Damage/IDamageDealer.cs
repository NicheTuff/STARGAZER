using STARGAZER.Systems.Stats.StatProfiles;

namespace STARGAZER.Systems.Damage
{
    public interface IDamageDealer
    {
        DamageProfile DamageProfile { get; }
        public void OnHit(IDamageable victim);
        public void PostHit(HitData hitData);
    }
}