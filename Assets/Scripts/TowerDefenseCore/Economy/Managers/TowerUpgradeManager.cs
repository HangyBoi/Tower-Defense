using TowerDefense.Towers.Events;
using TowerDefense.Economy;
using UnityEngine;

namespace TowerDefense.Towers.Economy.Upgrade
{
    /// <summary>
    /// Manages tower upgrade logic including validating player currency and triggering tower upgrades.
    /// Implements Single Responsibility by decoupling upgrade business logic from tower and UI code.
    /// </summary> 
    public class TowerUpgradeManager : MonoBehaviour
    {
        public static TowerUpgradeManager Instance;

        private CurrencyManager currencyManager;

        private void Awake()
        {
            // Set up the Singleton instance.
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            // Retrieve a reference to the CurrencyManager in the scene.
            currencyManager = FindObjectOfType<CurrencyManager>();
            if (currencyManager == null)
            {
                Debug.LogError("CurrencyManager not found in scene.");
            }
        }

        /// <summary>
        /// Attempts to upgrade the given tower by one level.
        /// Checks if the tower can be upgraded and if the player has enough money.
        /// </summary>
        /// <param name="tower">The tower to upgrade.</param>
        /// <returns>True if the upgrade was successful, false otherwise.</returns>
        public bool TryUpgradeTower(Tower tower)
        {
            if (tower == null)
            {
                Debug.LogError("Tower is null.");
                return false;
            }

            if (tower.isAtMaxLevel)
            {
                Debug.Log("Tower is already at max level.");
                return false;
            }

            // Retrieve the cost for the next upgrade level.
            int upgradeCost = tower.GetCostForNextLevel();
            if (upgradeCost < 0)
            {
                Debug.LogError("Invalid upgrade cost.");
                return false;
            }

            if (currencyManager == null)
            {
                Debug.LogError("CurrencyManager reference is missing.");
                return false;
            }

            if (!currencyManager.CanAfford(upgradeCost))
            {
                Debug.Log("Not enough money to upgrade tower.");
                return false;
            }

            // Deduct the upgrade cost from the player's currency.
            currencyManager.Spend(upgradeCost);

            // Perform the tower upgrade.
            bool upgraded = tower.UpgradeTower();
            if (upgraded)
            {
                // Inform subscribers that the tower has been upgraded.
                TowerEventsBus.RaiseTowerUpgraded(tower, tower.currentLevel);
                Debug.Log($"Tower upgraded to level {tower.currentLevel}.");
                return true;
            }
            else
            {
                Debug.LogError("Tower upgrade failed.");
                return false;
            }
        }

        /// <summary>
        /// Upgrades the specified tower to a target level if possible.
        /// Calculates the cumulative cost and processes upgrades level by level.
        /// </summary>
        /// <param name="tower">The tower to upgrade.</param>
        /// <param name="targetLevel">The desired target level.</param>
        /// <returns>True if the upgrade was successful, false otherwise.</returns>
        public bool TryUpgradeTowerToLevel(Tower tower, int targetLevel)
        {
            if (tower == null)
            {
                Debug.LogError("Tower is null.");
                return false;
            }

            if (targetLevel < tower.currentLevel || targetLevel >= tower.levels.Length)
            {
                Debug.LogError("Invalid target level.");
                return false;
            }

            int totalCost = 0;
            // Calculate the total cost required from the current level to the target level.
            for (int i = tower.currentLevel + 1; i <= targetLevel; i++)
            {
                totalCost += tower.levels[i].towerLevelData.towerUpgradeCost;
            }

            if (currencyManager == null)
            {
                Debug.LogError("CurrencyManager reference is missing.");
                return false;
            }

            if (!currencyManager.CanAfford(totalCost))
            {
                Debug.Log("Not enough money to upgrade tower to the desired level.");
                return false;
            }

            // Deduct the total cumulative upgrade cost.
            currencyManager.Spend(totalCost);

            // Upgrade the tower one level at a time.
            bool success = true;
            for (int i = tower.currentLevel + 1; i <= targetLevel; i++)
            {
                // success accumulates the result of each upgrade step.
                success &= tower.UpgradeTower();
            }

            if (success)
            {
                // Notify subscribers about the successful upgrade.
                TowerEventsBus.RaiseTowerUpgraded(tower, tower.currentLevel);
                Debug.Log($"Tower upgraded to level {tower.currentLevel}.");
            }
            else
            {
                Debug.LogError("Tower upgrade to target level failed.");
            }
            return success;
        }
    }
}

// *Comments and Headers Were Written with the Help of LLM* 
