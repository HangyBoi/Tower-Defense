using Core.Extensions;
using UnityEngine;

namespace TowerDefense.Towers.Placement
{
    /// <summary>
    /// Component that stores placement grid info for a tower.
    /// </summary>
    public class TowerPlacementInfo : MonoBehaviour
    {
        // Grid coordinate where the tower is placed.
        public IntVector2 gridPosition;

        // Size of the tower in grid cells (default is 1x1).
        public IntVector2 size = IntVector2.one;
    }
}

// *Comments and Headers Were Written with the Help of LLM* 
