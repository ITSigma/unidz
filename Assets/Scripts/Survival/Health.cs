using System;
using UnityEngine;

namespace Survival
{
    public class Health : MonoBehaviour
    {
        public float CurrentHealth
        {
            get => currentHealth;
            private set
            {
                if (currentHealth == 0)
                    return;

                if (currentHealth > 0 && value <= 0)
                    OnDie?.Invoke();


                currentHealth = Math.Clamp(value, 0, healthParameters.MaxHealth);
            }
        }

        public event Action OnDie;
        public bool IsDead => CurrentHealth <= 0;

        [SerializeField] private HealthParameters healthParameters;

        private float currentHealth;

        private void Start()
        {
            if (healthParameters == null)
            {
                Debug.LogError("Не назначена конфигурация для здоровья.");
                return;
            }

            CurrentHealth = healthParameters.MaxHealth;
        }

        public void IncreaseHealth(float amount)
        {
            if (amount <= 0)
                return;

            CurrentHealth += amount;
        }

        public void DecreaseHealth(float amount)
        {
            if (amount <= 0)
                return;

            CurrentHealth -= amount;
        }
    }
}