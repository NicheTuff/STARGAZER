using System;
using STARGAZER.Systems.Stats.StatProfiles;

namespace STARGAZER.Systems.Damage
{
	public class DamageResolver
	{
		protected static bool EvaluateCrit(DamageInstance attack, IDamageable target) => (attack.QualifiesCrit && target.AllowsCrit(attack)) || attack.ForceCrit || target.ForceCrit(attack);
		public CritResult ResolveCrit(DamageInstance attack, IDamageable target)
		{
			bool critLands = EvaluateCrit(attack, target);
			if (critLands)
				attack = attack.Source.DamageCalculator.ApplyCrit(attack);
			return new CritResult(critLands, attack);
		}
		public int ApplyArmor(DamageInstance attack, DefenseProfile stats)
		{
            int effectiveArmor = stats.GetTypeArmor(attack.DamageType) - attack.Pierce;
            return Math.Max(attack.Damage - effectiveArmor, 0);
        }
		public static readonly DamageResolver Instance = new();
	}
	public readonly struct CritResult
	{
		public readonly bool Lands;
		public readonly DamageInstance	Attack;
		public CritResult(bool lands, DamageInstance attack)
		{
			Lands = lands;
			Attack = attack;
		}
	}
}