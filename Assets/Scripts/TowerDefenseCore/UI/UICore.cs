using TowerDefense.Economy.Events;      // for CurrencyEventsBus
using TowerDefense.Level.Events;        // for LevelEventsBus
using TowerDefense.Level.Wave.Events;   // for WaveEventsBus
using TowerDefense.Level;
using UnityEngine;

namespace TowerDefense.UI
{
    /// <summary>
    /// Central controller for all UI panels.
    /// Subscribes to game-wide events (wave, level state, currency) and updates the sub-UIs accordingly.
    /// </summary>
    public class UICore : MonoBehaviour
    {
        public static UICore Instance { get; private set; }

        [Header("Sub UI Panels")]
        [SerializeField] private HUDManager hudPanel;
        [SerializeField] private TowerUI towerPanel;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void OnEnable()
        {
            // Subscribe to relevant global events in one place
            CurrencyEventsBus.OnMoneyChanged += HandleMoneyChanged;
            LevelEventsBus.OnLevelStateChanged += HandleLevelStateChanged;
            WaveEventsBus.OnWaveStarted += HandleWaveStarted;
            TowerSelectionHandler.OnTowerSelected += HandleTowerSelected;
        }

        private void OnDisable()
        {
            // Unsubscribe in one place
            CurrencyEventsBus.OnMoneyChanged -= HandleMoneyChanged;
            LevelEventsBus.OnLevelStateChanged -= HandleLevelStateChanged;
            WaveEventsBus.OnWaveStarted -= HandleWaveStarted;
            TowerSelectionHandler.OnTowerSelected -= HandleTowerSelected;
        }

        #region Event Handlers

        private void HandleMoneyChanged(int newAmount)
        {
            // Forward to HUD for money display
            if (hudPanel != null)
            {
                hudPanel.SetMoney(newAmount);
            }

            // Forward to TowerUI so it can re-check upgrade affordability
            if (towerPanel != null)
            {
                towerPanel.HandleMoneyChanged(newAmount);
            }

        }

        private void HandleLevelStateChanged(LevelState newState)
        {
            // Forward state changes to HUD
            if (hudPanel != null)
            {
                hudPanel.SetGameState(newState);
            }
        }

        private void HandleWaveStarted(int waveIndex)
        {
            // waveIndex is 1-based
            if (hudPanel != null)
            {
                hudPanel.SetWaveNumber(waveIndex);
            }
        }

        private void HandleTowerSelected(TowerDefense.Towers.Tower tower)
        {
            // route tower selection
            if (towerPanel != null)
            {
                towerPanel.SetSelectedTower(tower);
            }
        }

        #endregion

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;

            CurrencyEventsBus.OnMoneyChanged -= HandleMoneyChanged;
            LevelEventsBus.OnLevelStateChanged -= HandleLevelStateChanged;
            WaveEventsBus.OnWaveStarted -= HandleWaveStarted;
            TowerSelectionHandler.OnTowerSelected -= HandleTowerSelected;
        }
    }
}

// *Comments and Headers Were Written with the Help of LLM* 
