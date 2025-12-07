using TowerDefense.Enemies.Events;
using TowerDefense.Economy.Events;
using TowerDefense.Enemies;
using UnityEngine;
using TowerDefense.Level.Wave.Events;

namespace TowerDefense.Economy
{
    /// <summary>
    /// Manages the player's currency, including spending and earning.
    /// Implements a Singleton for global access.
    /// </summary>
    public class CurrencyManager : MonoBehaviour
    {
        public static CurrencyManager Instance;

        // Starting money assigned from the inspector.
        public int startingMoney = 10;
        private int currentMoney;

        private void Awake()
        {
            // Singleton setup: ensure only one instance exists.
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            currentMoney = startingMoney;
            // Immediately notify subscribers of the initial money value.
            CurrencyEventsBus.RaiseMoneyChanged(currentMoney);
        }

        private void OnEnable()
        {
            // Subscribe to enemy death events to earn money.
            EnemyEventsBus.OnEnemyDied += HandleEnemyDied;
        }

        private void OnDisable()
        {
            // Unsubscribe from enemy death events.
            EnemyEventsBus.OnEnemyDied -= HandleEnemyDied;
        }

        /// <summary>
        /// Callback for when an enemy dies; adds the enemy's reward to current money.
        /// </summary>
        /// <param name="enemy">The enemy that died.</param>
        private void HandleEnemyDied(Enemy enemy)
        {
            AddMoney(enemy.Reward);
        }

        /// <summary>
        /// Checks if the player has enough money to afford a given cost.
        /// </summary>
        /// <param name="cost">The cost to check.</param>
        /// <returns>True if the player can afford the cost.</returns>
        public bool CanAfford(int cost)
        {
            return currentMoney >= cost;
        }

        /// <summary>
        /// Deducts the specified cost from the current money if affordable.
        /// </summary>
        /// <param name="cost">The amount to spend.</param>
        public void Spend(int cost)
        {
            if (!CanAfford(cost)) return;

            currentMoney -= cost;
            // Update subscribers about the new money amount.
            CurrencyEventsBus.RaiseMoneyChanged(currentMoney);
        }

        /// <summary>
        /// Adds the specified amount to the current money.
        /// </summary>
        /// <param name="amount">The amount to add.</param>
        public void AddMoney(int amount)
        {
            currentMoney += amount;
            // Notify subscribers that the money has been updated.
            CurrencyEventsBus.RaiseMoneyChanged(currentMoney);
        }

        /// <summary>
        /// Returns the current money value.
        /// </summary>
        /// <returns>The current money.</returns>
        public int GetCurrentMoney() => currentMoney;

        private void OnDestroy()
        {
            // Clear the singleton reference if this instance is being destroyed.
            if (Instance == this)
                Instance = null;

            // Ensure we unsubscribe from enemy events to prevent potential memory leaks.
            EnemyEventsBus.OnEnemyDied -= HandleEnemyDied;
        }
    }
}

// *Comments and Headers Were Written with the Help of LLM* 