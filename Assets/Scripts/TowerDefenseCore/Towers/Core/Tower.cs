using TowerDefense.Towers.Events;
using UnityEngine;

namespace TowerDefense.Towers
{
    /// <summary>
    // Manages the tower's levels, upgrades, selling, and associated properties in the game.
    /// </summary>
    public class Tower : MonoBehaviour
    {
        // Assigned in the Inspector: each element is a “TowerLevel” prefab 
        // with its associated TowerLevelData.
        public TowerLevel[] levels;
        public string towerName;
        public LayerMask enemyLayerMask;

        public int currentLevel { get; protected set; }
        public TowerLevel currentTowerLevel { get; protected set; }

        public bool isAtMaxLevel => currentLevel >= levels.Length - 1;

        // For building cost (the cost of the first level)
        public int purchaseCost => levels[0].towerLevelData.towerCost;


        /// <summary>
        /// Returns the upgrade cost for the next level; returns -1 if at maximum level.
        /// </summary>
        public int GetCostForNextLevel()
        {
            if (isAtMaxLevel)
                return -1;
            return levels[currentLevel].towerLevelData.towerUpgradeCost;
        }

        private void Start()
        {
            // When tower is first created, set to level 0
            SetLevel(0);
        }

        /// <summary>
        /// Upgrades the tower by one level.
        /// </summary>
        public virtual bool UpgradeTower()
        {
            if (isAtMaxLevel) return false;
            SetLevel(currentLevel + 1);
            return true;
        }

        /// <summary>
        /// Upgrades the tower to a specified level.
        /// </summary>
        public virtual bool UpgradeTowerToLevel(int level)
        {
            if (level < 0 || level >= levels.Length) return false;
            SetLevel(level);
            return true;
        }

        /// <summary>
        /// Sells the tower, triggering an event and removing it from the scene.
        /// </summary>
        public void Sell()
        {
            TowerEventsBus.RaiseTowerSold(this);
            Destroy(gameObject);
        }

        /// <summary>
        /// Sets the tower's current level and updates its visual representation.
        /// </summary>
        /// <param name="level">The level to set.</param>
        protected void SetLevel(int level)
        {
            if (level < 0 || level >= levels.Length) return;

            currentLevel = level;

            // Clean up any existing visual level
            if (currentTowerLevel != null)
            {
                Destroy(currentTowerLevel.gameObject);
            }

            // Instantiate the new tower level prefab as a child of the tower.
            currentTowerLevel = Instantiate(levels[currentLevel], transform);

            // Initialize the new tower level with the parent tower's properties.
            currentTowerLevel.Initialize(this, enemyLayerMask);
        }
    }
}

// *Comments and Headers Were Written with the Help of LLM* 
