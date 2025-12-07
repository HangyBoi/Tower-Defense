using TowerDefense.Level;
using UnityEngine;
using TMPro;

namespace TowerDefense.UI
{
    /// <summary>
    /// HUD display for wave info, build timer, enemy pass count, etc.
    /// </summary>
    public class HUDManager : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI waveNumberText;
        [SerializeField] private TextMeshProUGUI buildPhaseTimerText;
        [SerializeField] private TextMeshProUGUI enemyPassCountText;
        [SerializeField] private TextMeshProUGUI gameStateText;

        [Header("Currency Display")]
        [SerializeField] private TextMeshProUGUI moneyText;

        private LevelManager levelManager;

        private void Start()
        {
            levelManager = LevelManager.Instance;
        }

        private void Update()
        {
            // 1) Build phase timer
            if (levelManager != null && levelManager.CurrentState == LevelState.Building)
            {
                float remainingTime = levelManager.BuildPhaseDuration - levelManager.BuildTimer;
                if (buildPhaseTimerText != null)
                    buildPhaseTimerText.text = $"Build Time Left: {remainingTime:F1}s";
            }
            else
            {
                if (buildPhaseTimerText != null) buildPhaseTimerText.text = "";
            }

            // 2) Enemies passed
            if (enemyPassCountText != null && levelManager != null)
            {
                enemyPassCountText.text = $"Enemies Passed: {levelManager.EnemiesPassed}/{levelManager.MaxEnemiesAllowed}";
            }
        }

        #region Methods Called by UICore

        /// <summary>
        /// Display the latest wave number (called from UICore).
        /// </summary>
        public void SetWaveNumber(int waveNumber)
        {
            if (waveNumberText != null)
            {
                waveNumberText.text = $"Wave: {waveNumber}";
            }
        }

        /// <summary>
        /// Display the current game state (called from UICore).
        /// </summary>
        public void SetGameState(LevelState newState)
        {
            if (gameStateText != null)
            {
                gameStateText.text = $"Game State: {newState}";
            }
        }

        /// <summary>
        /// Display the player's money (called from UICore).
        /// </summary>
        public void SetMoney(int newAmount)
        {
            if (moneyText != null)
            {
                moneyText.text = newAmount.ToString();
            }
        }

        #endregion
    }
}

// *Comments and Headers Were Written with the Help of LLM* 
