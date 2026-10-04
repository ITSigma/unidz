using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    [SerializeField] [Min(0)] private float torqueRandomRange;
    [SerializeField] [Min(0)] private float forceRandomRange;
    [SerializeField] [Min(0.05f)] private float spawnRadius;
    [SerializeField] [Min(1)] private int diceCount;
    [SerializeField] [Min(0.1f)] private float minRollDuration;
    [SerializeField] private Dice prefab;

    private readonly List<Dice> spawnedDice = new();
    private bool isRolling;
    private float rollStartTime;

    private void Awake()
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

    private void OnDiceRoll()
    {
        if (isRolling)
            return;

        StartRoll();
    }

    private void StartRoll()
    {
        isRolling = true;
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
        if (!isRolling)
            return;

        if (Time.time - rollStartTime < minRollDuration)
            return;

        if (spawnedDice.Any(dice => dice.IsRolling))
            return;

        isRolling = false;
        CalculateScore();
    }

    private void CalculateScore()
    {
        var totalScore = spawnedDice.Sum(dice => dice.GetValue());

        Debug.Log($"Total Score: {totalScore}");
    }
}