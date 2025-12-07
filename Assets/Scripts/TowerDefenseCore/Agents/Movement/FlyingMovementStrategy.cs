using TowerDefense.Nodes;
using UnityEngine.AI;

namespace TowerDefense.Enemies.Movement
{
    /// <summary>
    /// Movement strategy for flying enemies that bypass the node graph and move directly to the final goal.
    /// </summary>
    public class FlyingMovementStrategy : IMovementStrategy
    {
        public void StartMovement(NavMeshAgent agent, Node startNode, Enemy enemy)
        {

            // Single direct NavMesh path to final node.
            if (agent != null)
            {
                agent.enabled = true;
                agent.speed = enemy.MaxSpeed;

                // Find the final destination node for the enemy.
                var lastNode = FindFinalNode(startNode);
                if (lastNode != null)
                {
                    agent.SetDestination(lastNode.transform.position);
                }
            }
        }

        public void ProceedToNextNode(NavMeshAgent agent, Node arrivedNode, Enemy enemy)
        {
            if (arrivedNode.GetNextNode() == null)
            {
                enemy.OnReachedGoal();
            }
        }

        /// <summary>
        /// Traverses the node chain to locate the final destination node.
        /// </summary>
        /// <param name="startNode">The node from which to start the search.</param>
        /// <returns>The final node in the chain, or null if not found.</returns>
        private Node FindFinalNode(Node startNode)
        {
            Node current = startNode;
            while (current != null)
            {
                var next = current.GetNextNode();
                if (next == null) break;
                current = next;
            }
            return current;
        }
    }
}

// *Comments and Headers Were Written with the Help of LLM* 
