using TowerDefense.Nodes;
using UnityEngine.AI;
using UnityEngine;

namespace TowerDefense.Enemies.Movement
{
    /// <summary>
    /// Concrete Strategy for ground-based movement using node-to-node pathfinding.
    /// </summary>
    public class GroundMovementStrategy : IMovementStrategy
    {
        public void StartMovement(NavMeshAgent agent, Node startNode, Enemy enemy)
        {
            if (agent == null || startNode == null) return;
            agent.enabled = true;
            agent.speed = enemy.MaxSpeed;
            SetDestination(agent, startNode.transform.position);
        }

        public void ProceedToNextNode(NavMeshAgent agent, Node arrivedNode, Enemy enemy)
        {
            if (arrivedNode == null) return;

            Node nextNode = arrivedNode.GetNextNode();
            if (nextNode == null)
            {
                // No more nodes => enemy reached the goal.
                enemy.OnReachedGoal();
            }
            else
            {
                SetDestination(agent, nextNode.transform.position);
            }
        }

        /// <summary>
        /// Sets the NavMeshAgent destination if the agent is active.
        /// </summary>
        /// <param name="agent">The NavMeshAgent to update.</param>
        /// <param name="destination">The target destination position.</param>
        private void SetDestination(NavMeshAgent agent, Vector3 destination)
        {
            if (agent != null && agent.isActiveAndEnabled)
            {
                agent.SetDestination(destination);
            }
        }
    }
}

// *Comments and Headers Were Written with the Help of LLM* 
