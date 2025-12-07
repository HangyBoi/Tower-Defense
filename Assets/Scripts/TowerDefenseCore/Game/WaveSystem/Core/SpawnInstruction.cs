using TowerDefense.Enemies.Data;
using TowerDefense.Nodes;
using UnityEngine;

namespace TowerDefense.Level.Wave
{
    /// <summary>
    /// Defines a single spawn instruction for a group of enemies.
    /// </summary>
    [System.Serializable]
    public class SpawnInstruction
    {
        [Tooltip("Spawning from an EnemyData ScriptableObject")]
        public EnemyDataSO enemyData;

        [Tooltip("How many of this enemy to spawn")]
        public int amount = 5;

        [Tooltip("Delay between each enemy spawn in this group")]
        public float spawnDelay = 0.5f;

        [Tooltip("Where to place the spawned enemy (if you have multiple paths)")]
        public Transform spawnPoint;

        // Defines the starting node for enemy movement.
        public Node startNode;
    }
}

// *Comments and Headers Were Written with the Help of LLM* 