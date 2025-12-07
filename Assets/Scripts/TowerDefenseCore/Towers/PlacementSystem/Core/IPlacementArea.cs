using UnityEngine;
using Core.Extensions;

namespace TowerDefense.Towers.Placement
{
    /// <summary>
    /// An interface for a designated placement area where towers can be placed
    /// </summary>
    public interface IPlacementArea
    {

        /// <summary>
        /// Determines whether an object of a specified size can be placed at a given grid position.
        /// </summary>
        /// <param name="gridPos">The grid location</param>
        /// <param name="size">The dimensions of the object.</param>
        /// <returns>Returns true if the object can be placed at <paramref name="gridPos"/>, otherwise false.</returns>
        TowerFitStatus CanPlace(IntVector2 gridPos, IntVector2 size);

        /// <summary>
        /// Marks the specified grid region as occupied.
        /// </summary>
        void Occupy(IntVector2 gridPos, IntVector2 size);

        /// <summary>
        /// Clears the specified grid region (making it available).
        /// </summary>
        void Clear(IntVector2 gridPos, IntVector2 size);

        /// <summary>
        /// Converts a world-space position to a corresponding grid coordinate, adjusting for an object's size.
        /// </summary>
        IntVector2 ConvertWorldToGrid(Vector3 worldPosition, IntVector2 sizeOffset);

        /// <summary>
        /// Converts a grid coordinate to its corresponding world-space position.
        /// </summary>
        Vector3 ConvertGridToWorld(IntVector2 gridPosition, IntVector2 sizeOffset);
    }
}

// *Comments and Headers Were Written with the Help of LLM* 
