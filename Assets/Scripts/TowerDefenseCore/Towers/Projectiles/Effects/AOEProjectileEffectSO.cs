using TowerDefense.Enemies;
using UnityEngine;

namespace TowerDefense.Towers.Projectiles.Data
{
    /// <summary>
    /// Damages all enemies within a specified radius.
    /// </summary>
    [CreateAssetMenu(fileName = "AOEProjectileEffect", menuName = "TowerDefense/Projectile Effects/AOE", order = 3)]
    public class AOEProjectileEffectSO : ProjectileEffectSO
    {
        public override void ApplyEffect(Projectile projectile)
        {
            // Detect enemies within the AOE radius.
            Collider[] hits = Physics.OverlapSphere(projectile.transform.position, projectile.aoeRadius, projectile.enemyLayerMask);
            foreach (Collider c in hits)
            {
                if (c.TryGetComponent<Enemy>(out var enemy))
                {
                    enemy.TakeDamage(projectile.damage);
                }
            }
        }
    }
}

// *Comments and Headers Were Written with the Help of LLM* 
