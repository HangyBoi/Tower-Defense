using System;

namespace TowerDefense.Level.Events
{
    /// <summary>
    /// Event bus for broadcasting level state changes.
    /// </summary>
    public static class LevelEventsBus
    {
        // Event triggered when the level state changes.
        public static event Action<LevelState> OnLevelStateChanged;

        /// <summary>
        /// Raises the level state change event.
        /// </summary>
        /// <param name="newState">The new state of the level.</param>
        public static void RaiseLevelStateChanged(LevelState newState)
        {
            OnLevelStateChanged?.Invoke(newState);
        }
    }
}

// *Comments and Headers Were Written with the Help of LLM* 