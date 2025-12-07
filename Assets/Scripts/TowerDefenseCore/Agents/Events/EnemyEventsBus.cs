using System;

namespace TowerDefense.Enemies.Events
{
    /// <summary>
    /// Simple event bus for Enemy events.
    /// </summary>
    public static class EnemyEventsBus
    {
        public static event Action<Enemy> OnEnemyDied;
        public static event Action<Enemy> OnEnemyReachedGoal;
        public static event Action<Enemy, float, float> OnHealthChanged;

        /// <summary>
        /// Raises the OnEnemyDied event.
        /// </summary>
        public static void RaiseEnemyDied(Enemy enemy)
        {
            OnEnemyDied?.Invoke(enemy);
        }

        /// <summary>
        /// Raises the OnEnemyReachedGoal event.
        /// </summary>
        public static void RaiseEnemyReachedGoal(Enemy enemy)
        {
            OnEnemyReachedGoal?.Invoke(enemy);
        }

        /// <summary>
        /// Raises the OnHealthChanged event.
        /// </summary>
        /// <param name="enemy">The enemy whose health has changed.</param>
        /// <param name="currentHealth">The current health.</param>
        /// <param name="maxHealth">The maximum health.</param>
        public static void RaiseHealthChanged(Enemy enemy, float currentHealth, float maxHealth)
        {
            OnHealthChanged?.Invoke(enemy, currentHealth, maxHealth);
        }
    }
}

// *Comments and Headers Were Written with the Help of LLM* 
