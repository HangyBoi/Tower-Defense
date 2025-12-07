using Core.Extensions;  // For IntVector2
using UnityEngine;

namespace TowerDefense.Towers.Placement
{
    /// <summary>
    // Implements a grid-based placement area where towers can be placed.
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class TowerPlacementGrid : MonoBehaviour, IPlacementArea
    {
        [Header("Grid Setup")]
        public IntVector2 dimensions = new(5, 5);

        [Tooltip("Size of the edge of one grid cell for this area. Should match the physical size of towers.")]
        public float gridSize = 1f;

        [Header("Tile Visualization (Optional)")]
        public PlacementTile placementTilePrefab;

        private float invGridSize;
        private bool[,] availableCells;
        private PlacementTile[,] tiles;

        protected virtual void Awake()
        {
            ResizeCollider();

            // Initialize occupancy array (false = free, true = occupied)
            availableCells = new bool[dimensions.x, dimensions.y];

            // Precompute for efficiency
            invGridSize = 1f / gridSize;

            // Set up optional tile visuals
            SetUpGrid();
        }

        /// <summary>
        /// Adjust BoxCollider size to match the grid dimensions.
        /// </summary>
        private void ResizeCollider()
        {
            BoxCollider myCollider = GetComponent<BoxCollider>();
            Vector3 size = new Vector3(dimensions.x, 0, dimensions.y) * gridSize;
            myCollider.size = size;

            // Center it
            myCollider.center = size * 0.5f;

            // Hide in inspector so nobody modifies it
            myCollider.hideFlags = HideFlags.HideInInspector;
        }

        /// <summary>
        /// (Optional) Instantiate visual tiles to represent each grid cell.
        /// </summary>
        private void SetUpGrid()
        {
            PlacementTile tileToUse;

            tileToUse = placementTilePrefab;

            if (tileToUse != null)
            {
                GameObject tilesParent = new("TilesContainer");
                tilesParent.transform.SetParent(transform, false);

                tiles = new PlacementTile[dimensions.x, dimensions.y];

                for (int y = 0; y < dimensions.y; y++)
                {
                    for (int x = 0; x < dimensions.x; x++)
                    {
                        // Convert each cell to world position
                        IntVector2 cellPos = new(x, y);
                        Vector3 worldPos = ConvertGridToWorld(cellPos, IntVector2.one);

                        worldPos.y += 0.01f; // Slight offset to avoid z-fighting

                        PlacementTile newTile = Instantiate(tileToUse, worldPos, Quaternion.identity, tilesParent.transform);
                        newTile.SetState(PlacementTileState.Empty);

                        tiles[x, y] = newTile;
                    }
                }
            }
        }

        // ----------------------------------------------------------------------------
        // IPlacementArea IMPLEMENTATION
        // ----------------------------------------------------------------------------

        /// <summary>
        /// Converts a world-space position to grid coordinates, factoring in sizeOffset if needed.
        /// For a 1x1 tower, sizeOffset could be (1,1). For bigger footprints, adjust accordingly.
        /// </summary>
        public IntVector2 ConvertWorldToGrid(Vector3 worldPosition, IntVector2 sizeOffset)
        {
            Vector3 localPos = transform.InverseTransformPoint(worldPosition);
            localPos *= invGridSize;

            // Сenter the tower in the middle of its footprint:
            Vector3 offset = new(sizeOffset.x * 0.5f, 0f, sizeOffset.y * 0.5f);
            localPos -= offset;

            int xPos = Mathf.RoundToInt(localPos.x);
            int yPos = Mathf.RoundToInt(localPos.z);

            return new IntVector2(xPos, yPos);
        }

        /// <summary>
        /// Converts grid coordinates back to world space, factoring in sizeOffset if needed.
        /// </summary>
        public Vector3 ConvertGridToWorld(IntVector2 gridPosition, IntVector2 sizeOffset)
        {
            // Object centered in its footprint:
            Vector3 localPos = new Vector3(gridPosition.x + (sizeOffset.x * 0.5f), 0f, gridPosition.y + (sizeOffset.y * 0.5f)) * gridSize;

            return transform.TransformPoint(localPos);
        }

        /// <summary>
        /// Checks if an object of given 'size' can be placed at 'gridPos'.
        /// </summary>
        public TowerFitStatus CanPlace(IntVector2 gridPos, IntVector2 size)
        {
            // Check if the tower footprint is bigger than the entire grid
            if (size.x > dimensions.x || size.y > dimensions.y)
            {
                return TowerFitStatus.Exceeds;
            }

            // Calculate the extents (top-left to bottom-right region)
            IntVector2 extents = gridPos + size;

            // Out of range?
            if (gridPos.x < 0 || gridPos.y < 0 ||
                extents.x > dimensions.x || extents.y > dimensions.y)
            {
                return TowerFitStatus.Exceeds;
            }

            // Check each cell in the footprint for overlap
            for (int y = gridPos.y; y < extents.y; y++)
            {
                for (int x = gridPos.x; x < extents.x; x++)
                {
                    if (availableCells[x, y])
                    {
                        return TowerFitStatus.Overlaps;
                    }
                }
            }

            // If we get here, it fits
            return TowerFitStatus.Fits;
        }

        /// <summary>
        /// Mark the specified region as occupied.
        /// </summary>
        public void Occupy(IntVector2 gridPos, IntVector2 size)
        {
            IntVector2 extents = gridPos + size;

            for (int y = gridPos.y; y < extents.y; y++)
            {
                for (int x = gridPos.x; x < extents.x; x++)
                {
                    availableCells[x, y] = true;
                    // Update tile visuals
                    if (tiles != null && tiles[x, y] != null)
                    {
                        tiles[x, y].SetState(PlacementTileState.Filled);
                    }
                }
            }
        }

        /// <summary>
        /// Mark the specified region as free.
        /// </summary>
        public void Clear(IntVector2 gridPos, IntVector2 size)
        {
            IntVector2 extents = gridPos + size;

            for (int y = gridPos.y; y < extents.y; y++)
            {
                for (int x = gridPos.x; x < extents.x; x++)
                {
                    availableCells[x, y] = false;
                    // Update tile visuals
                    if (tiles != null && tiles[x, y] != null)
                    {
                        tiles[x, y].SetState(PlacementTileState.Empty);
                    }
                }
            }
        }

        private void OnValidate()
        {
            // Basic validation
            if (gridSize <= 0f)
            {
                Debug.LogError($"{name}: Grid size must be positive. Resetting to 1.");
                gridSize = 1f;
            }

            if (dimensions.x < 1 || dimensions.y < 1)
            {
                Debug.LogError($"{name}: Grid dimensions must be at least 1x1. Resetting to (1,1).");
                dimensions = new IntVector2(
                    Mathf.Max(dimensions.x, 1),
                    Mathf.Max(dimensions.y, 1));
            }

            // Adjust collider if needed
            if (Application.isPlaying == false && GetComponent<BoxCollider>() != null)
            {
                ResizeCollider();
            }
        }

        private void OnDrawGizmos()
        {
            // Optional gizmo for debugging
            Gizmos.color = Color.cyan;
            Matrix4x4 originalMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;

            for (int y = 0; y < dimensions.y; y++)
            {
                for (int x = 0; x < dimensions.x; x++)
                {
                    Vector3 cellCenter = new(
                        (x + 0.5f) * gridSize,
                        0f,
                        (y + 0.5f) * gridSize
                    );
                    Gizmos.DrawWireCube(cellCenter, new Vector3(gridSize, 0.01f, gridSize));
                }
            }

            Gizmos.matrix = originalMatrix;
        }
    }
}

// *Comments and Headers Were Written with the Help of LLM* 
