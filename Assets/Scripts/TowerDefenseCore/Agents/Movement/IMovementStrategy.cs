using TowerDefense.Nodes;
using UnityEngine.AI;

namespace TowerDefense.Enemies.Movement
{
    /// <summary>
    /// Interface for movement strategies to move an enemy from its start point to its goal.
    /// </summary>
    public interface IMovementStrategy
    {
        /// <summary>
        /// Called once to start the enemy's movement from the spawn or initial node.
        /// </summary>
        /// <param name="agent">The NavMeshAgent responsible for movement.</param>
        /// <param name="startNode">The starting node.</param>
        /// <param name="enemy">The enemy instance.</param>
        void StartMovement(NavMeshAgent agent, Node startNode, Enemy enemy);

        /// <summary>
        /// Called when the enemy reaches a node to determine and initiate the next movement step.
        /// </summary>
        /// <param name="agent">The NavMeshAgent responsible for movement.</param>
        /// <param name="arrivedNode">The node that was reached.</param>
        /// <param name="enemy">The enemy instance.</param>
        void ProceedToNextNode(NavMeshAgent agent, Node arrivedNode, Enemy enemy);
    }
}

// *Comments and Headers Were Written with the Help of LLM* 
