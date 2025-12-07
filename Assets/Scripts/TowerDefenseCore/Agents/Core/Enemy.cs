using TowerDefense.Towers.Projectiles;
using TowerDefense.Enemies.Movement;
using TowerDefense.Enemies.Data;
using TowerDefense.Enemies.Events;
using TowerDefense.UI;
using UnityEngine;

namespace TowerDefense.Enemies
{
    /// <summary>
    /// Represents an enemy in the Tower Defense game. Handles health, damage, movement, and debuffs.
    /// </summary>
    [RequireComponent(typeof(EnemyMovementController))]
    public class Enemy : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private EnemyDataSO enemyData;

        private float currentHealth;
        private EnemyMovementController movementController;
        private DebuffManager debuffManager;

        /// <summary>
        /// The reward given to the player upon enemy death.
        /// </summary>
        public int Reward => enemyData.deathReward;

        /// <summary>
        /// The current health of the enemy.
        /// </summary>
        public float Health => currentHealth;

        /// <summary>
        /// The maximum health of the enemy.
        /// </summary>
        public float MaxHealth => enemyData.maxHealth;

        /// <summary>
        /// The base movement speed of the enemy.
        /// </summary>
        public float MaxSpeed => enemyData.moveSpeed;

        /// <summary>
        /// The effective movement speed of the enemy factoring in active debuffs.
        /// </summary>
        public float EffectiveSpeed
        {
            get
            {
                if (debuffManager != null)
                {
                    return enemyData.moveSpeed * debuffManager.GetEffectiveMultiplier();
                }
                return enemyData.moveSpeed;
            }
        }

        private void Awake()
        {
            currentHealth = enemyData.maxHealth;
            movementController = GetComponent<EnemyMovementController>();
            movementController.Initialize(enemyData, this);

            // Ensure a DebuffManager is attached; add one if missing.
            debuffManager = GetComponent<DebuffManager>();
            if (debuffManager == null)
            {
                debuffManager = gameObject.AddComponent<DebuffManager>();
            }
        }

        /// <summary>
        /// Reduces the enemy's health by the specified damage amount.
        /// </summary>
        /// <param name="amount">Damage amount to apply.</param>
        public void TakeDamage(float amount)
        {
            currentHealth -= amount;
            EnemyEventsBus.RaiseHealthChanged(this, currentHealth, enemyData.maxHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        /// <summary>
        /// Called when the enemy reaches the goal. Raises the event and destroys the enemy.
        /// </summary>
        public void OnReachedGoal()
        {
            EnemyEventsBus.RaiseEnemyReachedGoal(this);
            Destroy(gameObject);
        }

        /// <summary>
        /// Handles enemy death, raises the death event, and shows a reward popup.
        /// </summary>
        private void Die()
        {
            ShowRewardPopup(enemyData.deathReward);
            EnemyEventsBus.RaiseEnemyDied(this);
            Destroy(gameObject);
        }

        /// <summary>
        /// Spawns a popup displaying the reward amount at the enemy's position.
        /// </summary>
        /// <param name="amount">The reward amount to display.</param>
        private void ShowRewardPopup(int amount)
        {
            Vector3 popupPosition = transform.position + new Vector3(0, 1f, 0);
            if (FloatingRewardManager.Instance != null)
            {
                FloatingRewardManager.Instance.SpawnRewardMultiple(popupPosition, amount);
            }
        }

        /// <summary>
        /// Delegates the application of a debuff to the attached DebuffManager.
        /// </summary>
        /// <param name="debuffType">Type of debuff.</param>
        /// <param name="debuffValue">Value/magnitude of the debuff.</param>
        /// <param name="debuffDuration">Duration of the debuff in seconds.</param>
        internal void ApplyDebuff(DebuffType debuffType, float debuffValue, float debuffDuration)
        {
            if (debuffManager != null)
            {
                debuffManager.ApplyDebuff(debuffType, debuffValue, debuffDuration);
            }
        }
    }
}

// *Comments and Headers Were Written with the Help of LLM* 
