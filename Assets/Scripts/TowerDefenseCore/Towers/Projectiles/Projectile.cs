using TowerDefense.Towers.Projectiles.Data;
using TowerDefense.Enemies;
using UnityEngine;

namespace TowerDefense.Towers.Projectiles
{
    /// <summary>
    /// Represents a projectile that moves towards a target and applies an effect upon hitting.
    /// </summary>
    public class Projectile : MonoBehaviour
    {
        public Transform target { get; private set; }
        public float damage { get; private set; }
        public float debuffValue { get; private set; }
        public float debuffDuration { get; private set; }
        public float aoeRadius { get; private set; }
        public LayerMask enemyLayerMask { get; private set; }
        public float speed = 10f;
        public ProjectileEffectSO projectileEffect { get; private set; }
        public DebuffType debuffType { get; private set; }

        /// <summary>
        /// Initializes the projectile with target and effect parameters.
        /// </summary>
        public void Initialize(Transform enemyTarget, float dmg, float dValue, float dDuration, float radius, LayerMask enemyLayer, ProjectileEffectSO effect, DebuffType debuffType)
        {
            target = enemyTarget;
            damage = dmg;
            debuffValue = dValue;
            debuffDuration = dDuration;
            aoeRadius = radius;
            enemyLayerMask = enemyLayer;
            projectileEffect = effect;
            this.debuffType = debuffType;
        }

        private void Update()
        {
            if (target == null)
            {
                // Target might have been destroyed; remove projectile.
                Destroy(gameObject);
                return;
            }

            // Move the projectile toward its target.
            Vector3 dir = target.position - transform.position;
            float distanceThisFrame = speed * Time.deltaTime;

            if (dir.magnitude <= distanceThisFrame)
            {
                HitTarget();
                return;
            }
            transform.Translate(dir.normalized * distanceThisFrame, Space.World);
            transform.LookAt(target); // Ensures the projectile faces the target.
        }

        /// <summary>
        /// Applies the projectile effect upon hitting the target and destroys the projectile.
        /// </summary>
        private void HitTarget()
        {
            // Delegate the effect application to the projectile effect ScriptableObject.
            if (projectileEffect != null)
            {
                projectileEffect.ApplyEffect(this);
            }
            else
            {
                // Fallback: apply single-target damage.
                if (target != null)
                {
                    if (target.TryGetComponent<Enemy>(out var enemy))
                    {
                        enemy.TakeDamage(damage);
                    }
                }
            }
            Destroy(gameObject);
        }
    }
}

// *Comments and Headers Were Written with the Help of LLM* 
