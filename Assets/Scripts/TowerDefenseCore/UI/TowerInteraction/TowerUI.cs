using TowerDefense.Towers.Economy.Upgrade;
using TowerDefense.Towers;
using TowerDefense.Economy;
using UnityEngine.UI;
using UnityEngine;
using TMPro;    


namespace TowerDefense.UI
{
    /// <summary>
    /// UI panel for displaying selected tower information and handling upgrade/sell actions.
    /// </summary>
    public class TowerUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI upgradeDescription;
        [SerializeField] private Button upgradeButton;
        [SerializeField] private Button sellButton;
        [SerializeField] private TowerInfoDisplayPanel towerInfoDisplayPanel;
        [SerializeField] private GameObject panel;

        private Tower currentTower;

        private void Start()
        {
            // Hide panel initially
            panel.SetActive(false);

            // Hook up button clicks
            upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
            sellButton.onClick.AddListener(OnSellButtonClicked);
        }

        /// <summary>
        /// Called by UICore's event handler for TowerSelected.
        /// </summary>
        public void SetSelectedTower(Tower tower)
        {
            currentTower = tower;

            if (tower == null)
            {
                panel.SetActive(false);
                return;
            }

            panel.SetActive(true);
            UpdateUI();
        }

        /// <summary>
        /// Called by UICore whenever money changes, so we can re-check upgrade availability.
        /// </summary>
        public void HandleMoneyChanged(int newMoney)
        {
            if (panel.activeInHierarchy)
            {
                UpdateUI();
            }
        }

        /// <summary>
        /// Updates the UI elements based on the current tower's status.
        /// </summary>
        private void UpdateUI()
        {
            if (currentTower == null) return;

            towerInfoDisplayPanel.Show(currentTower);

            int upgradeCost = currentTower.GetCostForNextLevel();
            bool canUpgrade = !currentTower.isAtMaxLevel &&
                              CurrencyManager.Instance.CanAfford(upgradeCost);
            upgradeButton.interactable = canUpgrade;

            if (!currentTower.isAtMaxLevel)
            {
                upgradeDescription.text = $"Next Level - {currentTower.levels[currentTower.currentLevel + 1].towerLevelData.upgradeTowerDescription}";
            }
            else
            {
                upgradeDescription.text = "Max Level!";
            }
        }

        /// <summary>
        /// Attempts to upgrade the selected tower.
        /// </summary>
        public void OnUpgradeButtonClicked()
        {
            if (currentTower == null) return;

            // Actually call the upgrade manager
            bool success = TowerUpgradeManager.Instance.TryUpgradeTower(currentTower);
            if (success)
            {
                // Refresh the UI
                UpdateUI();
            }
        }

        /// <summary>
        /// Sells the selected tower and refunds money.
        /// </summary>
        public void OnSellButtonClicked()
        {
            if (currentTower == null) return;

            // Refund
            int sellValue = currentTower.currentTowerLevel.towerLevelData.towerSellCost;
            CurrencyManager.Instance.AddMoney(sellValue);

            // Actually remove the tower
            currentTower.Sell();

            // Hide the panel
            panel.SetActive(false);
        }
    }
}

// *Comments and Headers Were Written with the Help of LLM* 
