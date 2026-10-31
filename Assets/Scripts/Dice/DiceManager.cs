using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dice
{
    public class DiceManager : MonoBehaviour
    {
        public event Action OnTotalScoreChange;

        public int DiceCount => diceCount;
        public int? TotalScore { get; private set; }
        public bool IsRolling { get; private set; }

        [SerializeField] [Min(0)] private float torqueRandomRange;
        [SerializeField] [Min(0)] private float forceRandomRange;
        [SerializeField] [Min(0.05f)] private float spawnRadius;
        [SerializeField] [Min(1)] private int diceCount;
        [SerializeField] [Min(0.1f)] private float minRollDuration;
        [SerializeField] private Dice prefab;

        private readonly List<Dice> spawnedDice = new();
        private float rollStartTime;

        private void Awake()
        {
            if (prefab == null)
            {
                return;
            }

            SpawnDice();
        }

        public void RollDice()
        {
            if (IsRolling)
                return;

            StartRoll();
        }

        public void RespawnDice(int newCount)
        {
            diceCount = newCount;

            DeleteDice();
            SpawnDice();
        }

        private void StartRoll()
        {
            IsRolling = true;
            TotalScore = null;
            rollStartTime = Time.time;

            foreach (var dice in spawnedDice)
            {
                var forceVector = new Vector3(
                    Random.Range(-forceRandomRange, forceRandomRange),
                    Random.Range(0, 2 * forceRandomRange),
                    Random.Range(-forceRandomRange, forceRandomRange)
                );

                var torqueVector = new Vector3(
                    Random.Range(-torqueRandomRange, torqueRandomRange),
                    Random.Range(-torqueRandomRange, torqueRandomRange),
                    Random.Range(-torqueRandomRange, torqueRandomRange)
                );

                dice.Roll(forceVector, torqueVector);
            }
        }

        private void Update()
        {
            if (!IsRolling)
                return;

            if (Time.time - rollStartTime < minRollDuration)
                return;

            if (spawnedDice.Any(dice => dice.IsRolling))
                return;

            IsRolling = false;
            CalculateScore();
        }

        private void CalculateScore()
        {
            TotalScore = spawnedDice.Sum(dice => dice.GetValue());

            OnTotalScoreChange?.Invoke();
        }

        private void SpawnDice()
        {
            for (var i = 0; i < diceCount; i++)
            {
                var dice = Instantiate(prefab, transform);

                var randomPos = new Vector3(
                    Random.Range(-spawnRadius, spawnRadius),
                    0,
                    Random.Range(-spawnRadius, spawnRadius)
                );

                dice.transform.localPosition += randomPos;
                dice.transform.localRotation = Random.rotation;

                spawnedDice.Add(dice);
            }
        }

        private void DeleteDice()
        {
            foreach (var dice in spawnedDice)
                Destroy(dice.gameObject);

            spawnedDice.Clear();
        }
    }
}