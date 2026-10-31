    using UnityEngine;

    namespace Survival
    {
        public class HealthParameters : ScriptableObject
        {
            [SerializeField] private float maxHealth = 100f;

            public float MaxHealth => maxHealth;
        }
    }