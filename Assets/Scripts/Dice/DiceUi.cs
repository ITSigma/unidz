using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dice
{
    public class DiceUi : MonoBehaviour
    {
        public event Action<int, int> OnAllTotalScoreFill;

        [SerializeField] private DiceManager playerDiceManager;
        [SerializeField] private DiceManager opponentDiceManager;
        [SerializeField] private GameObject winResult;
        [SerializeField] private GameObject drawResult;
        [SerializeField] private GameObject loseResult;
        [SerializeField] private string scorePlaceholder;

        [SerializeField] private TMP_InputField diceCountInputField;
        [SerializeField] private Button rollButton;
        [SerializeField] private TMP_Text playerTotalScoreText;
        [SerializeField] private TMP_Text opponentTotalScoreText;

        private GameObject[] allResults;

        private void Awake()
        {
            if (playerDiceManager == null || opponentDiceManager == null)
                return;

            playerDiceManager.OnTotalScoreChange += ChangePlayerTotalScore;
            opponentDiceManager.OnTotalScoreChange += ChangeOpponentTotalScore;
            OnAllTotalScoreFill += ShowResults;

            diceCountInputField.onEndEdit.AddListener(OnDiceCountChange);
            rollButton.onClick.AddListener(StartRoll);

            allResults = new[] { winResult, drawResult, loseResult };
        }

        private void OnDestroy()
        {
            playerDiceManager.OnTotalScoreChange -= ChangePlayerTotalScore;
            opponentDiceManager.OnTotalScoreChange -= ChangeOpponentTotalScore;
            diceCountInputField.onEndEdit.RemoveListener(OnDiceCountChange);
            rollButton.onClick.RemoveListener(StartRoll);
        }

        private void OnDiceCountChange(string countRaw)
        {
            if (!int.TryParse(countRaw, out var diceCount) || diceCount <= 0)
            {
                diceCountInputField.text = playerDiceManager.DiceCount.ToString();
                return;
            }

            playerDiceManager.RespawnDice(diceCount);
            opponentDiceManager.RespawnDice(diceCount);
        }

        private void StartRoll()
        {
            if (playerDiceManager.IsRolling || opponentDiceManager.IsRolling)
                return;

            playerDiceManager.RollDice();
            opponentDiceManager.RollDice();

            opponentTotalScoreText.text = scorePlaceholder;
            playerTotalScoreText.text = scorePlaceholder;

            foreach (var result in allResults)
                result.SetActive(false);
        }

        private void ChangePlayerTotalScore()
        {
            ChangeTotalScore(playerDiceManager, opponentDiceManager, playerTotalScoreText);
        }

        private void ChangeOpponentTotalScore()
        {
            ChangeTotalScore(opponentDiceManager, playerDiceManager, opponentTotalScoreText);
        }

        private void ChangeTotalScore(DiceManager diceManager, DiceManager otherDiceManager, TMP_Text totalScoreText)
        {
            if (!diceManager.TotalScore.HasValue)
                return;

            totalScoreText.text = diceManager.TotalScore.ToString();

            if (!otherDiceManager.TotalScore.HasValue)
                return;

            OnAllTotalScoreFill?.Invoke(playerDiceManager.TotalScore!.Value, opponentDiceManager.TotalScore!.Value);
        }

        private void ShowResults(int playerScore, int opponentScore)
        {
            var result = playerScore switch
            {
                _ when playerScore > opponentScore => winResult,
                _ when playerScore < opponentScore => loseResult,
                _ => drawResult
            };

            result.gameObject.SetActive(true);
        }
    }
}