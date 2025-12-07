using TowerDefense.Towers.Events;
using TowerDefense.Economy;
using TowerDefense.UI.HUD;
using Core.Extensions;
using UnityEngine;
using System;

namespace TowerDefense.Towers.Placement
{
    /// <summary>
    /// Manages the process of placing towers on the grid, including cost checks and placement finalization.
    /// </summary>
    public class TowerPlacementManager : MonoBehaviour
    {
        public static TowerPlacementManager Instance;

        private TowerPlacementGhost activeGhost;
        private bool canPlaceTowers = true;

        // Keep track of the cost for the current tower
        private int currentTowerCost;

        private CurrencyManager currencyManager;

        public static event Action OnTowerPlacementEnded;

        void Awake()
        {
            Instance = this;
            currencyManager = FindObjectOfType<CurrencyManager>();
            TowerEventsBus.OnTowerSold += HandleTowerSold;
        }

        void OnDestroy()
        {
            TowerEventsBus.OnTowerSold -= HandleTowerSold;
        }

        /// <summary>
        /// Called by LevelManager or other systems to allow/disallow new tower placements.
        /// </summary>
        public void SetCanPlaceTowers(bool value)
        {
            canPlaceTowers = value;
            // If we turn off tower placement, also cancel any ghost in progress:
            if (!canPlaceTowers && activeGhost != null)
            {
                CancelPlacement();
            }
        }

        /// <summary>
        /// Called by the Shop system to start placing a new tower.
        /// </summary>
        /// <param name="towerPrefab">The tower prefab.</param>
        /// <param name="cost">The cost of the tower (from TowerLevelData or computed in ShopItemDataSO).</param>
        public void StartPlacingTower(Tower towerHolderPrefab, TowerPlacementGhost ghostPrefab, int cost)
        {
            if (!canPlaceTowers) return;

            // Destroy old ghost if one is active
            if (activeGhost != null)
            {
                Destroy(activeGhost.gameObject);
            }

            // Instantiate the chosen ghost
            activeGhost = Instantiate(ghostPrefab);
            activeGhost.Initialize(towerHolderPrefab);

            currentTowerCost = cost;
        }

        void Update()
        {
            if (!canPlaceTowers) return;

            if (activeGhost != null)
            {
                // Right-click => cancel
                if (Input.GetMouseButtonDown(1))
                {
                    CancelPlacement();
                    return;
                }

                // 1) Raycast
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                LayerMask placementLayerMask = LayerMask.GetMask("Ground", "PlacementGrid");
                if (Physics.Raycast(ray, out RaycastHit hit, 1000f, placementLayerMask))
                {
                    // Attempt to find a grid
                    var grid = hit.collider.GetComponentInParent<TowerPlacementGrid>();
                    if (grid != null)
                    {
                        Vector3 worldPos = hit.point;
                        IntVector2 gridPos = grid.ConvertWorldToGrid(worldPos, IntVector2.one);
                        Vector3 snappedPos = grid.ConvertGridToWorld(gridPos, IntVector2.one);

                        TowerFitStatus status = grid.CanPlace(gridPos, IntVector2.one);
                        bool isValid = (status == TowerFitStatus.Fits);

                        // Move ghost
                        activeGhost.Move(snappedPos, Quaternion.identity, isValid);

                        // Left-click => place if valid
                        if (Input.GetMouseButtonDown(0) && isValid)
                        {
                            ConfirmPlacement(grid, gridPos);
                        }
                    }
                    else
                    {
                        // If no grid was hit, optionally set ghost invalid
                        // Or you can simply hide ghost, etc.
                        activeGhost.Move(hit.point, Quaternion.identity, false);
                    }
                }
            }
        }

        /// <summary>
        /// Finalizes tower placement if funds are still available.
        /// </summary>
        /// <param name="gridPos">The grid coordinates where the tower is placed.</param>
        private void ConfirmPlacement(TowerPlacementGrid grid, IntVector2 gridPos)
        {
            if (currencyManager != null && currencyManager.CanAfford(currentTowerCost))
            {
                currencyManager.Spend(currentTowerCost);

                // Place the tower
                PlaceTower(activeGhost.controller, grid, gridPos);
            }
            else
            {
                Debug.LogWarning("Insufficient funds to place tower.");
            }

            // Remove the ghost
            if (activeGhost != null)
            {
                Destroy(activeGhost.gameObject);
                activeGhost = null;
            }

            OnTowerPlacementEnded?.Invoke();
        }

        /// <summary>
        /// Cancels the placement of the tower.
        /// </summary>
        private void CancelPlacement()
        {
            // Simply destroy the ghost; no money is deducted.
            if (activeGhost != null)
            {
                Destroy(activeGhost.gameObject);
                activeGhost = null;
            }

            OnTowerPlacementEnded?.Invoke();
        }

        /// <summary>
        /// Instantiates the tower at the grid position.
        /// (Assumes that affordability was already checked by the shop system.)
        /// </summary>
        private void PlaceTower(Tower towerPrefab, TowerPlacementGrid grid, IntVector2 gridPos)
        {
            Vector3 worldPos = grid.ConvertGridToWorld(gridPos, IntVector2.one);

            Tower newTower = Instantiate(towerPrefab, worldPos, Quaternion.identity, grid.transform);

            if (!newTower.TryGetComponent<TowerPlacementInfo>(out var placementInfo))
            {
                placementInfo = newTower.gameObject.AddComponent<TowerPlacementInfo>();
            }

            placementInfo.gridPosition = gridPos;
            placementInfo.size = IntVector2.one;

            // Occupy in this specific grid
            grid.Occupy(gridPos, IntVector2.one);

            TowerEventsBus.RaiseTowerPlaced(newTower);
        }

        /// <summary>
        /// Handles the sale of a tower by freeing its occupied grid cells.
        /// </summary>
        private void HandleTowerSold(Tower soldTower)
        {
            if (soldTower.TryGetComponent<TowerPlacementInfo>(out var placementInfo))
            {
                var grid = soldTower.GetComponentInParent<TowerPlacementGrid>();

                if (grid != null)
                {
                    grid.Clear(placementInfo.gridPosition, placementInfo.size);
                }
            }
        }
    }
}

// *Comments and Headers Were Written with the Help of LLM* 
