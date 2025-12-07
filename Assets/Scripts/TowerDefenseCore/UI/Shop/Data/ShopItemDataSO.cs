using TowerDefense.Towers;
using TowerDefense.UI.HUD;
using UnityEngine;

namespace TowerDefense.Economy.Data
{
    /// <summary>
    /// ScriptableObject that holds data for a shop item, including the tower prefab, ghost prefab, and display assets.
    /// </summary>
    [CreateAssetMenu(fileName = "NewShopItem", menuName = "TowerDefense/Shop Item")]
    public class ShopItemDataSO : ScriptableObject
    {
        public Tower towerHolderPrefab;
        public TowerPlacementGhost ghostPrefab;
        public Sprite icon;
        public Sprite buttonBackground;

        /// <summary>
        /// Computes the cost based on the tower's first level data.
        /// </summary>
        public int Cost
        {
            get
            {
                if (towerHolderPrefab != null
                    && towerHolderPrefab.levels != null
                    && towerHolderPrefab.levels.Length > 0)
                {
                    return towerHolderPrefab.levels[0].towerLevelData.towerCost;
                }
                return 0;
            }
        }
    }
}

// *Comments and Headers Were Written with the Help of LLM* 
