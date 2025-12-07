using TowerDefense.Enemies.Data;
using TowerDefense.Nodes;
using UnityEngine.AI;
using UnityEngine;

namespace TowerDefense.Enemies.Movement
{
    /// <summary>
    /// Controls the movement behavior of an enemy using a selected movement strategy.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyMovementController : MonoBehaviour
    {
        private NavMeshAgent agent;
        private Enemy enemy;
        private IMovementStrategy movementStrategy;

        /// <summary>
        /// One-time initialization with the appropriate movement strategy.
        /// </summary>
        public void Initialize(EnemyDataSO data, Enemy enemy)
        {
            // We pick a movement strategy based on the data's movementType
            movementStrategy = MovementStrategyFactory.CreateStrategy(data.movementType);

            // Optionally set speed from data
            if (agent != null)
            {
                agent.speed = data.moveSpeed;
            }

            // We store a reference to the Enemy instance if needed
            this.enemy = enemy;
        }

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            enemy = GetComponent<Enemy>();
        }

        /// <summary>
        /// Called by external spawners or wave managers to begin movement.
        /// </summary>
        public void StartMovement(Node startNode)
        {
            movementStrategy?.StartMovement(agent, startNode, enemy);
        }

        /// <summary>
        /// Called by the Node (via OnTriggerEnter) when we arrive there.
        /// </summary>
        public void ProceedToNextNode(Node arrivedNode)
        {
            movementStrategy?.ProceedToNextNode(agent, arrivedNode, enemy);
        }

        private void Update()
        {
            // Update the NavMeshAgent speed with the effective speed from debuffs.
            if (agent != null && enemy != null)
            {
                agent.speed = enemy.EffectiveSpeed;
            }
        }
    }
}

// *Comments and Headers Were Written with the Help of LLM* 
