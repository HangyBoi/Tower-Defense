using System.Collections.Generic;
using TowerDefense.Economy.Data;
using TowerDefense.UI;
using UnityEngine;

namespace TowerDefense.Economy
{
    /// <summary>
    /// Manages the in-game shop, instantiating shop item buttons and wiring up interactions.
    /// Implements a Singleton for global access.
    /// </summary>
    public class ShopManager : MonoBehaviour
    {
        public static ShopManager Instance;

        [Tooltip("List of available shop items (ScriptableObjects)")]
        public List<ShopItemDataSO> shopItems;

        [Tooltip("Shop item button prefab")]
        public GameObject shopItemButtonPrefab;

        [Tooltip("Parent panel where shop buttons will be instantiated")]
        public Transform shopPanel;

        // Reference to the panel that displays tower information.
        [SerializeField] private TowerInfoDisplayPanel towerInfoDisplayPanelInScene;

        private CurrencyManager currencyManager;

        private void Awake()
        {
            // Set up the Singleton instance.
            Instance = this;
            // Find the CurrencyManager in the scene.
            currencyManager = FindObjectOfType<CurrencyManager>();
            // Initialize the shop UI.
            InitializeShop();
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }

        /// <summary>
        /// Initializes the shop by creating buttons for each shop item.
        /// </summary>
        private void InitializeShop()
        {
            // Loop through all defined shop items.
            foreach (var item in shopItems)
            {
                // Instantiate a shop item button under the shop panel.
                GameObject buttonGO = Instantiate(shopItemButtonPrefab, shopPanel);
                if (buttonGO.TryGetComponent<ShopItemButton>(out var button))
                {
                    // Link the button to the tower info display panel for showing details.
                    button.SetInfoDisplayPanel(towerInfoDisplayPanelInScene);
                    // Configure the button with the shop item data and currency manager reference.
                    button.Setup(item, currencyManager);
                }
            }
        }
    }
}

// *Comments and Headers Were Written with the Help of LLM* 