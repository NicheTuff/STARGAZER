namespace Assets.Scripts.Systems.Stats
{
    public class StatModifier
    {
        public int Additive;
        public float Percent;
        public float Multiplier = 1f;
        public float Apply(float value)
        {
            value += Additive;
            value *= 1 + Percent;
            value *= Multiplier;
            return value;
        }
    }
}
