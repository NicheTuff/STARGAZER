using Assets.Scripts.Systems.Stats.StatProfiles;

namespace Assets.Scripts.Systems.Damage
{
    public interface IDamageable
    {
        public bool CanCrit(DamageInstance incomingAttack) => true;
        public HitData TakeHit(DamageInstance[] incomingAttack, DefenseProfile defenseStats)
        {
            bool critLands = incomingAttack[0].QualifiesCrit && CanCrit(incomingAttack[0]);
            var appliedAttack = critLands ? incomingAttack[1] : incomingAttack[0];
            int effectiveArmor = defenseStats.GetTypeArmor(appliedAttack.DamageType) - appliedAttack.Pierce;
            int damageReceived = appliedAttack.Damage - effectiveArmor;

            defenseStats.Damage(damageReceived);

            if (critLands)
            {
                OnReceiveCrit();
            }
            if (defenseStats.Health <= 0)
            {
                OnDeath();
            }

            return new HitData(this, damageReceived, critLands);
        }
        public void OnReceiveCrit() { }
        public void OnDeath();
    }
}