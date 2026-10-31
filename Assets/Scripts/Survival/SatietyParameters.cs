using UnityEngine;

namespace Survival
{
    public class SatietyParameters : ScriptableObject
    {
        [SerializeField] private float maxSatiety;
        [SerializeField] private float starvingThreshold;
        [SerializeField] private float satietyThreshold;

        [SerializeField] private float satietyConstantDecreaseValue;
        [SerializeField] private float satietyConstantDecreasePeriodInSeconds;

        [SerializeField] private float starvingDamageValue;
        [SerializeField] private float starvingDamagePeriodInSeconds;
        [SerializeField] private float satietyHealValue;
        [SerializeField] private float satietyHealPeriodInSeconds;

        public float MaxSatiety => maxSatiety;
        public float StarvingThreshold => starvingThreshold;
        public float SatietyThreshold => satietyThreshold;

        public float SatietyConstantDecreaseValue => satietyConstantDecreaseValue;
        public float SatietyConstantDecreasePeriodInSeconds => satietyConstantDecreasePeriodInSeconds;

        public float StarvingDamageValue => starvingDamageValue;
        public float StarvingDamagePeriodInSeconds => starvingDamagePeriodInSeconds;
        public float SatietyHealValue => satietyHealValue;
        public float SatietyHealPeriodInSeconds => satietyHealPeriodInSeconds;
    }
}