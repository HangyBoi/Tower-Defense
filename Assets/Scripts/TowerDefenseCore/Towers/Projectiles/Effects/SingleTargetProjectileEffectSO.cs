using TowerDefense.Enemies;
using UnityEngine;

namespace TowerDefense.Towers.Projectiles.Data
{
    /// <summary>
    /// Applies damage to a single target enemy.
    /// </summary>
    [CreateAssetMenu(fileName = "SingleTargetProjectileEffect", menuName = "TowerDefense/Projectile Effects/Single Target", order = 2)]
    public class SingleTargetProjectileEffectSO : ProjectileEffectSO
    {
        public override void ApplyEffect(Projectile projectile)
        {
            if (projectile.target != null)
            {
                if (projectile.target.TryGetComponent<Enemy>(out var enemy))
                {
                    // Apply damage to the single target.
                    enemy.TakeDamage(projectile.damage);
                }
            }
        }
    }
}

// *Comments and Headers Were Written with the Help of LLM* 
