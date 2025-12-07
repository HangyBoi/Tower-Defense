using TowerDefense.Enemies;
using UnityEngine;

namespace TowerDefense.Towers.Projectiles.Data
{
    /// <summary>
    /// Applies damage and a debuff effect to an enemy.
    /// </summary>
    [CreateAssetMenu(fileName = "DebuffProjectileEffect", menuName = "TowerDefense/Projectile Effects/Debuff", order = 4)]
    public class DebuffProjectileEffectSO : ProjectileEffectSO
    {
        public override void ApplyEffect(Projectile projectile)
        {
            if (projectile.target != null)
            {
                if (projectile.target.TryGetComponent<Enemy>(out var enemy))
                {
                    // Apply base damage.
                    enemy.TakeDamage(projectile.damage);
                    // Apply the debuff (e.g., slow), ensuring it doesn't stack.
                    enemy.ApplyDebuff(projectile.debuffType, projectile.debuffValue, projectile.debuffDuration);
                }
            }
        }
    }
}

// *Comments and Headers Were Written with the Help of LLM* 
