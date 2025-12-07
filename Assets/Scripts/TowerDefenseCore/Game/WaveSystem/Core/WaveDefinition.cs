using UnityEngine;

namespace TowerDefense.Level.Wave
{
    /// <summary>
    /// Contains the definition for a wave including its name, spawn instructions, and timing settings.
    /// </summary>
    [System.Serializable]
    public class WaveDefinition
    {
        [Tooltip("Name for debugging or design clarity")]
        public string waveName = "Wave 1";

        [Tooltip("All instructions to spawn enemies in this wave")]
        public SpawnInstruction[] spawnInstructions;

        [Tooltip("Time to wait after the final enemy is spawned, before wave ends")]
        public float postWaveDelay = 0f;

        [Tooltip("Option to run instructions simultaneously per-wave basis")]
        public bool spawnInstructionsSimultaneously = false;
    }
}

// *Comments and Headers Were Written with the Help of LLM* 