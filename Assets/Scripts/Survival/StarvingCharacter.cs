using System;
using System.Collections;
using UnityEngine;

namespace Survival
{
    public class StarvingCharacter : MonoBehaviour
    {
        [SerializeField] private Health health;
        [SerializeField] private Satiety satiety;

        private void OnEnable()
        {
            satiety.OnStartStarving += HandleStarving;
            satiety.OnBecomeSated += HandleBeingSated;
            health.OnDie += HandleDie;
        }

        private void OnDisable()
        {
            satiety.OnStartStarving -= HandleStarving;
            satiety.OnBecomeSated -= HandleBeingSated;
            health.OnDie -= HandleDie;
        }

        public void TakeDamage(float amount)
        {
            health.DecreaseHealth(amount);
        }

        public void Heal(float amount)
        {
            health.IncreaseHealth(amount);
        }

        public void Eat(float amount)
        {
            satiety.IncreaseSatiety(amount);
        }

        private void HandleStarving()
        {
            StartCoroutine(
                DecreaseHealthCycle(
                    satiety.StarvingDamageValue,
                    satiety.StarvingDamagePeriodInSeconds,
                    () => satiety.IsStarving
                )
            );
        }

        private void HandleBeingSated()
        {
            StartCoroutine(
                IncreaseHealthCycle(
                    satiety.SatietyHealValue,
                    satiety.SatietyHealPeriodInSeconds,
                    () => satiety.IsSated
                )
            );
        }

        private IEnumerator DecreaseHealthCycle(float amount, float periodInSeconds, Func<bool> continuationCondition)
        {
            while (!health.IsDead && continuationCondition())
            {
                TakeDamage(amount);
                yield return new WaitForSeconds(periodInSeconds);
            }
        }

        private IEnumerator IncreaseHealthCycle(float amount, float periodInSeconds, Func<bool> continuationCondition)
        {
            while (!health.IsDead && continuationCondition())
            {
                Heal(amount);
                yield return new WaitForSeconds(periodInSeconds);
            }
        }

        private void HandleDie()
        {
            Destroy(gameObject);
        }
    }
}