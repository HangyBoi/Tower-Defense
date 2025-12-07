using TowerDefense.Towers.Placement;
using TowerDefense.Economy.Events;
using TowerDefense.Economy.Data;
using TowerDefense.UI;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace TowerDefense.Economy
{
    /// <summary>
    /// UI button for a shop item that displays its icon, cost, and handles purchase and tower placement.
    /// </summary>
    public class ShopItemButton : MonoBehaviour
    {
        public Image iconImage;
        public TextMeshProUGUI costText;
        public Button buyButton;
        public Image backgroundImage;

        private TowerInfoDisplayPanel towerInfoDisplayPanel;

        private ShopItemDataSO shopItem;
        private CurrencyManager currencyManager;

        /// <summary>
        /// Sets the panel that displays detailed tower info.
        /// </summary>
        public void SetInfoDisplayPanel(TowerInfoDisplayPanel panel)
        {
            towerInfoDisplayPanel = panel;
        }

        /// <summary>
        /// Initializes the shop item button with item data and currency manager.
        /// </summary>
        public void Setup(ShopItemDataSO item, CurrencyManager manager)
        {
            shopItem = item;
            currencyManager = manager;

            // Set the button icon
            if (iconImage != null)
                iconImage.sprite = shopItem.icon;

            // NEW: Set the background image
            if (backgroundImage != null && shopItem.buttonBackground != null)
                backgroundImage.sprite = shopItem.buttonBackground;

            // Set the cost text
            if (costText != null)
                costText.text = shopItem.Cost.ToString();

            // Hook up the buy button
            buyButton.onClick.AddListener(OnBuyButtonClicked);
            CurrencyEventsBus.OnMoneyChanged += UpdateButton;
            UpdateButton(currencyManager.GetCurrentMoney());
        }

        /// <summary>
        /// Updates button interactability based on current funds.
        /// </summary>
        private void UpdateButton(int currentMoney)
        {
            buyButton.interactable = currentMoney >= shopItem.Cost;
        }

        /// <summary>
        /// Handles the purchase click, showing tower info and starting placement.
        /// </summary>
        private void OnBuyButtonClicked()
        {
            if (currencyManager.CanAfford(shopItem.Cost))
            {
                // Show tower info, etc.
                if (towerInfoDisplayPanel != null)
                {
                    towerInfoDisplayPanel.gameObject.SetActive(true);
                    towerInfoDisplayPanel.ShowShopItemInfo(shopItem);
                }

                // Start tower placement with the chosen ghost
                TowerPlacementManager.Instance.StartPlacingTower(
                    shopItem.towerHolderPrefab,
                    shopItem.ghostPrefab,
                    shopItem.Cost
                );

                TowerPlacementManager.OnTowerPlacementEnded += HideTowerInfoPanel;
            }
        }

        /// <summary>
        /// Hides the tower info panel after placement completes.
        /// </summary>
        private void HideTowerInfoPanel()
        {
            // Unsubscribe so we don't hide it at the wrong time in the future
            TowerPlacementManager.OnTowerPlacementEnded -= HideTowerInfoPanel;

            // Actually hide the panel
            if (towerInfoDisplayPanel != null)
            {
                towerInfoDisplayPanel.Hide();
            }
        }

        private void OnDestroy()
        {
            CurrencyEventsBus.OnMoneyChanged -= UpdateButton;
            TowerPlacementManager.OnTowerPlacementEnded -= HideTowerInfoPanel;
        }
    }
}

// *Comments and Headers Were Written with the Help of LLM* 
