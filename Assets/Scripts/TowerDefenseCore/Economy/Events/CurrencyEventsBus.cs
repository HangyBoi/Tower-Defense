using System;

namespace TowerDefense.Economy.Events
{
    /// <summary>
    /// Event bus for broadcasting currency-related events.
    /// </summary>
    public static class CurrencyEventsBus
    {
        // Event triggered when the player's money changes.
        public static event Action<int> OnMoneyChanged;

        /// <summary>
        /// Raises the money changed event with the updated amount.
        /// </summary>
        /// <param name="newAmount">The new money amount.</param>
        public static void RaiseMoneyChanged(int newAmount)
        {
            // Only invoke the event if there are subscribers.
            OnMoneyChanged?.Invoke(newAmount);
        }
    }
}

// *Comments and Headers Were Written with the Help of LLM* 