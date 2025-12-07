using UnityEngine;

namespace TowerDefense.Towers.Projectiles.Data
{
    /// <summary>
    /// Base ScriptableObject for projectile effects.
    /// </summary>
    public abstract class ProjectileEffectSO : ScriptableObject
    {
        /// <summary>
        /// Applies the effect when a projectile hits its target.
        /// </summary>
        /// <param name="projectile">The projectile instance that hit a target.</param>
        public abstract void ApplyEffect(Projectile projectile);
    }
}

// *Comments and Headers Were Written with the Help of LLM* 
