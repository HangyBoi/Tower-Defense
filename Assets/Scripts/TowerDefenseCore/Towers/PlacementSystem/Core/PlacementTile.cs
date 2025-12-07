using UnityEngine;

namespace TowerDefense.Towers.Placement
{
    public enum PlacementTileState
    {
        Empty,
        Filled
    }

    public class PlacementTile : MonoBehaviour
    {
        public bool IsOccupied { get; private set; }
        public Tower CurrentTower { get; private set; }

        // Optionally, a Renderer to change the material/color
        [SerializeField] private Renderer tileRenderer;
        [SerializeField] private Material emptyMaterial;
        [SerializeField] private Material filledMaterial;

        private int gridX, gridY;
        private TowerPlacementGrid parentGrid;

        /// <summary>
        /// Initializes the tile with its grid coordinates and parent grid.
        /// </summary>
        public void Initialize(int x, int y, TowerPlacementGrid grid)
        {
            gridX = x;
            gridY = y;
            parentGrid = grid;
        }

        /// <summary>
        /// Sets the occupied state and optionally updates visuals.
        /// </summary>
        public void SetOccupied(bool occupied, Tower tower)
        {
            IsOccupied = occupied;
            CurrentTower = tower;
            SetState(occupied ? PlacementTileState.Filled : PlacementTileState.Empty);
        }

        /// <summary>
        /// Changes the visual state of the tile.
        /// </summary>
        public void SetState(PlacementTileState state)
        {
            if (tileRenderer != null)
            {
                tileRenderer.material = (state == PlacementTileState.Filled) ? filledMaterial : emptyMaterial;
            }
        }
    }
}

// *Comments and Headers Were Written with the Help of LLM* 
