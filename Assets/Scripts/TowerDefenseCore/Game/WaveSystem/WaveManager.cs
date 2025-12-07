using TowerDefense.Level.Wave.Events;
using TowerDefense.Enemies.Movement;
using System.Collections;
using UnityEngine;
using TowerDefense.Enemies.Events;
using TowerDefense.Enemies;

namespace TowerDefense.Level.Wave
{
    /// <summary>
    /// Manages the spawning and progression of enemy waves.
    /// Responsible for handling both sequential and parallel enemy spawns,
    /// tracking active enemies, and raising wave events.
    /// </summary>
    public class WaveManager : MonoBehaviour
    {
        public static WaveManager Instance;

        [Header("Wave Settings")]
        public WaveDefinition[] waves;

        [Header("Standalone Test Settings")]
        [Tooltip("If true, WaveManager will automatically spawn wave 0 if no LevelManager is found.")]
        public bool autoStartWaveOnAwake = false;

        // Tracking
        private int currentWaveIndex = -1;
        private bool waveInProgress = false;

        // Track how many enemies are currently active
        private int activeEnemies = 0;

        // Flag to indicate if we have finished spawning all enemies for the current wave
        private bool spawningComplete = false;

        // If we forcibly stop waves, we set this so coroutines can exit early
        private bool waveStopRequested = false;

        private void Awake()
        {
            // Setup singleton instance.
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            // If no LevelManager is present and auto-start is enabled, start the first wave.
            var levelManager = FindObjectOfType<LevelManager>();
            if (levelManager == null && autoStartWaveOnAwake)
            {
                Debug.Log("WaveManager: No LevelManager found. Auto-starting wave 0.");
                StartWave(0);
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }

        void OnEnable()
        {
            // Subscribe to enemy events
            EnemyEventsBus.OnEnemyDied += HandleEnemyRemoved;
            EnemyEventsBus.OnEnemyReachedGoal += HandleEnemyRemoved;
        }

        void OnDisable()
        {
            // Unsubscribe from enemy events
            EnemyEventsBus.OnEnemyDied -= HandleEnemyRemoved;
            EnemyEventsBus.OnEnemyReachedGoal -= HandleEnemyRemoved;
        }

        /// <summary>
        /// Spawns exactly one wave by index, raising wave started event.
        /// </summary>
        public void StartWave(int waveIndex)
        {
            if (waveInProgress)
            {
                Debug.LogWarning("A wave is already in progress!");
                return;
            }

            if (waveIndex < 0 || waveIndex >= waves.Length)
            {
                Debug.LogError($"Wave index {waveIndex} is out of range!");
                return;
            }

            // Reset flags and counters for the new wave.
            waveStopRequested = false;
            currentWaveIndex = waveIndex;
            waveInProgress = true;
            spawningComplete = false;
            activeEnemies = 0;

            // Raise wave started
            WaveEventsBus.RaiseWaveStarted(currentWaveIndex + 1);

            // Start a coroutine that spawns the wave
            StartCoroutine(SpawnOneWaveCoroutine(waves[currentWaveIndex]));
        }

        /// <summary>
        /// Spawns all instructions for a single wave, then marks spawning as complete.
        /// </summary>
        private IEnumerator SpawnOneWaveCoroutine(WaveDefinition wave)
        {
            if (wave.spawnInstructionsSimultaneously)
            {
                // Launch all instructions in parallel
                yield return StartCoroutine(SpawnInstructionsInParallel(wave));
            }
            else
            {
                // Default: spawn instructions sequentially
                yield return StartCoroutine(SpawnInstructionsSequentially(wave));
            }

            // If we were forced to stop, just exit
            if (waveStopRequested)
                yield break;

            // Mark that we've finished spawning
            spawningComplete = true;

            // If no enemies remain active, the wave is complete
            if (activeEnemies <= 0 && waveInProgress)
            {
                FinishCurrentWave();
            }
        }

        /// <summary>
        /// Spawns each instruction one by one in sequence.
        /// </summary>
        private IEnumerator SpawnInstructionsSequentially(WaveDefinition wave)
        {
            foreach (var instruction in wave.spawnInstructions)
            {
                // spawn the 'amount' enemies in a loop
                for (int i = 0; i < instruction.amount; i++)
                {
                    if (waveStopRequested) yield break; // exit early if losing
                    SpawnSingleEnemy(instruction);
                    // wait before the next enemy
                    yield return new WaitForSeconds(instruction.spawnDelay);
                }
            }

            // Optional post-wave delay
            if (wave.postWaveDelay > 0f)
            {
                yield return new WaitForSeconds(wave.postWaveDelay);
            }
        }

        /// <summary>
        /// Spawns all instructions in parallel. We'll wait until all instructions are done.
        /// </summary>
        private IEnumerator SpawnInstructionsInParallel(WaveDefinition wave)
        {
            int instructionsCount = wave.spawnInstructions.Length;
            int instructionsCompleted = 0;

            // We'll run a separate coroutine for each spawn instruction
            for (int i = 0; i < instructionsCount; i++)
            {
                SpawnInstructionParallel(wave.spawnInstructions[i], () =>
                {
                    instructionsCompleted++;
                });
            }

            // Wait until we've completed all instructions
            yield return new WaitUntil(() => instructionsCompleted >= instructionsCount);

            if (waveStopRequested) yield break;  // exit early if losing

            // Then wait for the post-wave delay, if any
            if (wave.postWaveDelay > 0f)
            {
                yield return new WaitForSeconds(wave.postWaveDelay);
            }
        }

        /// <summary>
        /// Launches a coroutine that spawns a group of enemies for a single instruction in parallel.
        /// When finished, it calls onCompleted().
        /// </summary>
        private void SpawnInstructionParallel(SpawnInstruction instruction, System.Action onCompleted)
        {
            StartCoroutine(SpawnInstructionParallelCoroutine(instruction, onCompleted));
        }

        private IEnumerator SpawnInstructionParallelCoroutine(SpawnInstruction instruction, System.Action onCompleted)
        {
            // We spawn "instruction.amount" enemies in a loop with a small delay between each
            for (int i = 0; i < instruction.amount; i++)
            {
                if (waveStopRequested) yield break;  // exit early if losing
                SpawnSingleEnemy(instruction);
                yield return new WaitForSeconds(instruction.spawnDelay);
            }

            onCompleted?.Invoke();
        }

        /// <summary>
        /// Instantiates a single enemy from the given instruction, applies movement, etc.
        /// </summary>
        private void SpawnSingleEnemy(SpawnInstruction instruction)
        {
            var enemyPrefab = instruction.enemyData.enemyPrefab;
            instruction.spawnPoint.GetPositionAndRotation(out Vector3 spawnPos, out Quaternion spawnRot);

            // Instantiate
            GameObject newEnemyObj = Instantiate(enemyPrefab, spawnPos, spawnRot);

            // Increment active enemies
            activeEnemies++;

            var movement = newEnemyObj.GetComponent<EnemyMovementController>();
            if (movement != null && instruction.startNode != null)
            {
                movement.StartMovement(instruction.startNode);
            }
        }

        /// <summary>
        /// Called whenever an enemy is removed (died or reached goal).
        /// Decrements the activeEnemies count and checks if wave can be finished.
        /// </summary>
        private void HandleEnemyRemoved(Enemy enemy)
        {
            activeEnemies--;

            // If we have finished spawning AND no enemies are left, the wave is truly done
            if (!waveStopRequested && waveInProgress && spawningComplete && activeEnemies <= 0)
            {
                FinishCurrentWave();
            }
        }

        /// <summary>
        /// Completes the current wave, raises events, and checks if it's the last wave.
        /// </summary>
        private void FinishCurrentWave()
        {
            waveInProgress = false;

            // Raise wave completed event
            WaveEventsBus.RaiseWaveCompleted(currentWaveIndex + 1);

            // If this wave was the last wave in the array, raise AllWavesCompleted
            if (currentWaveIndex == waves.Length - 1)
            {
                WaveEventsBus.RaiseAllWavesCompleted();
            }
        }

        /// <summary>
        /// Called by LevelManager when we lose (or otherwise want to cancel all waves).
        /// Forces the spawning coroutines to stop early.
        /// </summary>
        public void StopAllWaves()
        {
            Debug.Log("WaveManager: Stopping all wave coroutines due to Lose condition.");
            waveStopRequested = true;
            waveInProgress = false;
            StopAllCoroutines();
        }
    }
}

// *Comments and Headers Were Written with the Help of LLM* 
