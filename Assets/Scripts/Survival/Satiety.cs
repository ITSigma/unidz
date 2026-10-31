using System;
using System.Collections;
using UnityEngine;

namespace Survival
{
    public class Satiety : MonoBehaviour
    {
        public float CurrentSatiety
        {
            get => currentSatiety;
            private set
            {
                if (currentSatiety > parameters.StarvingThreshold && value <= parameters.StarvingThreshold)
                    OnStartStarving?.Invoke();

                if (currentSatiety < parameters.SatietyThreshold && value >= parameters.SatietyThreshold)
                    OnBecomeSated?.Invoke();

                currentSatiety = Math.Clamp(value, 0, parameters.MaxSatiety);
            }
        }

        public event Action OnStartStarving;
        public event Action OnBecomeSated;

        public bool IsStarving => CurrentSatiety <= parameters.StarvingThreshold;
        public bool IsSated => CurrentSatiety >= parameters.SatietyThreshold;

        public float StarvingDamageValue => parameters.StarvingDamageValue;
        public float StarvingDamagePeriodInSeconds => parameters.StarvingDamagePeriodInSeconds;
        public float SatietyHealValue => parameters.SatietyHealValue;
        public float SatietyHealPeriodInSeconds => parameters.SatietyHealPeriodInSeconds;

        [SerializeField] private SatietyParameters parameters;

        private float currentSatiety;

        private void Start()
        {
            if (parameters == null)
            {
                Debug.LogError("Не назначена конфигурация сытости.");
                return;
            }

            CurrentSatiety = parameters.MaxSatiety;
            StartCoroutine(DecreaseSatietyCycle());
        }

        public void IncreaseSatiety(float amount)
        {
            if (amount <= 0)
                return;

            CurrentSatiety += amount;
        }

        public void DecreaseSatiety(float amount)
        {
            if (amount <= 0)
                return;

            CurrentSatiety -= amount;
        }

        private IEnumerator DecreaseSatietyCycle()
        {
            while (true)
            {
                DecreaseSatiety(parameters.SatietyConstantDecreaseValue);
                yield return new WaitForSeconds(parameters.SatietyConstantDecreasePeriodInSeconds);
            }
        }
    }
}