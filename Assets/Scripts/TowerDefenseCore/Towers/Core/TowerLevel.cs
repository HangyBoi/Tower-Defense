using TowerDefense.Towers.Projectiles;
using TowerDefense.Towers.Data;
using UnityEngine;

namespace TowerDefense.Towers
{
    /// <summary>
    /// Represents a tower's level, managing its targeting and attack logic.
    /// </summary>
    public class TowerLevel : MonoBehaviour
    {
        [Tooltip("Reference to the data for this tower level")]
        public TowerLevelData towerLevelData;

        private Tower parentTower;          // The Tower script that spawned us
        private float attackCooldown = 0f;
        private LayerMask enemyLayerMask;

        // A child or reference used to rotate the top of the tower
        public Transform turretHead;

        // Projectile prefab assigned in the Inspector or in your data
        [SerializeField]
        private GameObject projectilePrefab;

        /// <summary>
        /// Initializes the tower level with its parent tower and enemy layer mask.
        /// </summary>
        public void Initialize(Tower tower, LayerMask mask)
        {
            parentTower = tower;
            enemyLayerMask = mask;
        }

        private void Update()
        {
            // Reduce the cooldown timer.
            if (attackCooldown > 0f)
                attackCooldown -= Time.deltaTime;

            // Search for the closest enemy target.
            Transform target = FindClosestEnemy();
            if (target)
            {
                // Rotate turret towards the target.
                RotateTurretHead(target.position);

                // Shoot if ready.
                if (attackCooldown <= 0f)
                {
                    ShootAt(target);
                    attackCooldown = 1f / towerLevelData.towerFireRate;
                }
            }
        }

        /// <summary>
        /// Rotates the turret to face the target position.
        /// </summary>
        private void RotateTurretHead(Vector3 targetPos)
        {
            if (turretHead == null) return;

            Vector3 direction = targetPos - turretHead.position;
            direction.y = 0; // Keep rotation on the Y-axis only.
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            turretHead.rotation = Quaternion.Lerp(turretHead.rotation, lookRotation, Time.deltaTime * 5f);
        }

        /// <summary>
        /// Finds the closest enemy within the tower's range.
        /// </summary>
        private Transform FindClosestEnemy()
        {
            // Option 1: Physics Overlap
            Collider[] hits = Physics.OverlapSphere(transform.position, towerLevelData.towerRange, enemyLayerMask);
            float minDist = float.MaxValue;
            Transform closest = null;

            for (int i = 0; i < hits.Length; i++)
            {
                float dist = Vector3.Distance(hits[i].transform.position, transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = hits[i].transform;
                }
            }
            return closest;
        }

        /// <summary>
        /// Instantiates a projectile and initializes it with tower level data.
        /// </summary>
        private void ShootAt(Transform target)
        {
            if (projectilePrefab == null) return;

            // Instantiate the projectile.
            GameObject projGO = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            if (projGO.TryGetComponent<Projectile>(out var projectile))
            {
                // Initialize the projectile using tower level data.
                projectile.Initialize(
                             target,
                             towerLevelData.towerDamage,
                             towerLevelData.projectileDebuffMultiplier,
                             towerLevelData.projectileDebuffDuration,
                             towerLevelData.AOERadius,
                             enemyLayerMask,
                             towerLevelData.projectileEffectType,       // The chosen effect (strategy)
                             towerLevelData.projectileDebuffType        // Pass the debuff type from TowerLevelData
                );
            }
        }
    }
}

// *Comments and Headers Were Written with the Help of LLM* 
