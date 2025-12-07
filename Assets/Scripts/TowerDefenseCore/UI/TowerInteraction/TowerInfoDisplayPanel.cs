using TowerDefense.Economy.Data;
using TowerDefense.Towers;
using UnityEngine;
using TMPro;

namespace TowerDefense.UI
{
    /// <summary>
    /// Displays detailed information about a tower, either when selected or in the shop.
    /// </summary>
    public class TowerInfoDisplayPanel : MonoBehaviour
    {
        [Header("Text Fields")]
        [SerializeField] private TextMeshProUGUI towerNameText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI attackTypeText;
        [SerializeField] private TextMeshProUGUI dpsText;

        /// <summary>
        /// Show info from a fully placed Tower in the scene.
        /// </summary>
        public void Show(Tower tower)
        {
            if (tower == null)
            {
                return;
            }

            var currentData = tower.levels[tower.currentLevel].towerLevelData;

            // Tower name
            if (towerNameText != null)
            {
                towerNameText.text = tower.towerName;
            }
            // Description
            if (descriptionText != null)
            {
                descriptionText.text = currentData.towerDescription;
            }
            // Attack Type
            if (attackTypeText != null)
            {
                var debuff = currentData.projectileDebuffType.ToString();
                attackTypeText.text = $"Type: {debuff}";
            }
            // DPS
            if (dpsText != null)
            {
                float dps = currentData.towerDamage * currentData.towerFireRate;
                dpsText.text = $"DPS: {dps:F1}";
            }
        }

        /// <summary>
        /// Show info from a ShopItemDataSO (i.e., from the prefab’s first level).
        /// Call this when you're showing a tower *before* it’s placed.
        /// </summary>
        public void ShowShopItemInfo(ShopItemDataSO shopItem)
        {
            if (shopItem == null || shopItem.towerHolderPrefab == null
                || shopItem.towerHolderPrefab.levels == null
                || shopItem.towerHolderPrefab.levels.Length == 0)
            {
                return;
            }

            var tower = shopItem.towerHolderPrefab;
            var data = tower.levels[0].towerLevelData; // First level’s data

            // Tower name
            if (towerNameText != null)
            {
                towerNameText.text = tower.towerName;
            }
            // Description
            if (descriptionText != null)
            {
                descriptionText.text = data.towerDescription;
            }
            // Attack Type
            if (attackTypeText != null)
            {
                var debuff = data.projectileDebuffType.ToString();
                attackTypeText.text = $"Type: {debuff}";
            }
            // DPS
            if (dpsText != null)
            {
                float dps = data.towerDamage * data.towerFireRate;
                dpsText.text = $"DPS: {dps:F1}";
            }
        }

        /// <summary>
        /// Hide the display panel. 
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}

// *Comments and Headers Were Written with the Help of LLM* 
