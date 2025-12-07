using UnityEngine;

namespace TowerDefense.Enemies.Data
{
    public enum MovementType
    {
        Ground,
        Flying
    }

    /// <summary>
    /// ScriptableObject that stores data and configuration for an enemy.
    /// </summary>
    [CreateAssetMenu(fileName = "EnemyData.asset", menuName = "TowerDefense/Enemy Data", order = 1)]
    public class EnemyDataSO : ScriptableObject
    {

        [Header("Displayed Information")]
        /// <summary>
        /// The name of the agent
        /// </summary>y
        public string enemyName;

        /// <summary>
        /// Short summary of the agent
        /// </summary>
        [TextArea]
        public string description;

        /// <summary>
        /// The Agent prefab that will be used on instantiation
        /// </summary>
        public GameObject enemyPrefab;

        [Header("Stats")]
        /// <summary>
        /// The maximum health value for the enemy.
        /// </summary>
        public float maxHealth = 10;

        /// <summary>
        /// The base movement speed of the enemy.
        /// </summary>
        public float moveSpeed = 3;

        /// <summary>
        /// The reward given upon enemy death.
        /// </summary>
        public int deathReward = 5;

        [Header("Behavior")]
        /// <summary>
        /// Determines the movement type (e.g., Ground or Flying) of the enemy.
        /// </summary>
        public MovementType movementType = MovementType.Ground;
    }
}

// *Comments and Headers Were Written with the Help of LLM* 
