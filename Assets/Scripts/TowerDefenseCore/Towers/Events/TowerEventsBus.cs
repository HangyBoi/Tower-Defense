using System;

namespace TowerDefense.Towers.Events
{
    /// <summary>
    /// Static class for broadcasting tower events.
    /// </summary>
    public static class TowerEventsBus
    {
        public static event Action<Tower> OnTowerPlaced;
        public static event Action<Tower> OnTowerSold;
        public static event Action<Tower, int> OnTowerUpgraded;
        // etc.

        /// <summary>
        /// Raises the event for when a tower is placed.
        /// </summary>
        public static void RaiseTowerPlaced(Tower tower)
        {
            OnTowerPlaced?.Invoke(tower);
        }

        /// <summary>
        /// Raises the event for when a tower is sold.
        /// </summary>
        public static void RaiseTowerSold(Tower tower)
        {
            OnTowerSold?.Invoke(tower);
        }

        /// <summary>
        /// Raises the event for when a tower is upgraded.
        /// </summary>
        public static void RaiseTowerUpgraded(Tower tower, int newLevel)
        {
            OnTowerUpgraded?.Invoke(tower, newLevel);
        }
    }
}

// *Comments and Headers Were Written with the Help of LLM* 
