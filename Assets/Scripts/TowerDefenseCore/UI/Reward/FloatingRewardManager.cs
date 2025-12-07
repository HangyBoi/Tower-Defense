using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.UI
{
    /// <summary>
    /// Manages spawning and pooling of FloatingRewardIcon instances.
    /// </summary>
    public class FloatingRewardManager : MonoBehaviour
    {
        public static FloatingRewardManager Instance;

        [Tooltip("Prefab of the FloatingRewardIcon (with FloatingRewardIcon script attached).")]
        public FloatingRewardIcon floatingRewardPrefab;

        [Tooltip("Initial number of pooled icons.")]
        public int initialPoolSize = 10;

        [Tooltip("Spacing between reward icons.")]
        public float spacing = 0.5f;

        private readonly Queue<FloatingRewardIcon> pool = new();

        private void Awake()
        {
            // Set up singleton instance
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            // Pre-instantiate pool
            for (int i = 0; i < initialPoolSize; i++)
            {
                FloatingRewardIcon icon = Instantiate(floatingRewardPrefab, transform);
                icon.gameObject.SetActive(false);
                pool.Enqueue(icon);
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }

        /// <summary>
        /// Spawns a single reward icon at the specified position.
        /// </summary>
        public void SpawnReward(Vector3 position)
        {
            FloatingRewardIcon icon;
            if (pool.Count > 0)
            {
                icon = pool.Dequeue();
            }
            else
            {
                // Pool exhausted; instantiate a new one if needed
                icon = Instantiate(floatingRewardPrefab, transform);
            }
            icon.Activate(position);
        }

        /// <summary>
        /// Spawns multiple reward icons arranged horizontally.
        /// </summary>
        /// <param name="basePosition">The central spawn position.</param>
        /// <param name="count">The number of icons to spawn.</param>
        public void SpawnRewardMultiple(Vector3 basePosition, int count)
        {
            // Use the camera's right vector for horizontal arrangement.
            Vector3 right = Vector3.right;
            if (Camera.main != null)
            {
                right = Camera.main.transform.right;
            }

            float totalWidth = (count - 1) * spacing;

            for (int i = 0; i < count; i++)
            {
                float offsetAmount = i * spacing - totalWidth / 2f;
                Vector3 spawnPos = basePosition + right * offsetAmount;
                SpawnReward(spawnPos);
            }
        }

        /// <summary>
        /// Returns a reward icon back to the pool after its animation completes.
        /// </summary>
        public void ReturnToPool(FloatingRewardIcon icon)
        {
            icon.gameObject.SetActive(false);
            pool.Enqueue(icon);
        }
    }
}

// *Comments and Headers Were Written with the Help of LLM* 
