using System;

namespace TowerDefense.Level.Wave.Events
{
    /// <summary>
    /// Event bus for broadcasting events related to enemy waves.
    /// </summary>
    public static class WaveEventsBus
    {
        // Events for when a wave starts, completes, or when all waves are finished.
        public static event Action<int> OnWaveStarted;
        public static event Action<int> OnWaveCompleted;
        public static event Action OnAllWavesCompleted;

        /// <summary>
        /// Raises the event indicating a wave has started.
        /// </summary>
        /// <param name="waveIndex">The index of the wave (1-indexed for clarity).</param>
        public static void RaiseWaveStarted(int waveIndex)
        {
            OnWaveStarted?.Invoke(waveIndex);
        }

        /// <summary>
        /// Raises the event indicating a wave has completed.
        /// </summary>
        /// <param name="waveIndex">The index of the wave (1-indexed for clarity).</param>
        public static void RaiseWaveCompleted(int waveIndex)
        {
            OnWaveCompleted?.Invoke(waveIndex);
        }

        /// <summary>
        /// Raises the event indicating all waves have been completed.
        /// </summary>
        public static void RaiseAllWavesCompleted()
        {
            OnAllWavesCompleted?.Invoke();
        }
    }
}

// *Comments and Headers Were Written with the Help of LLM* 