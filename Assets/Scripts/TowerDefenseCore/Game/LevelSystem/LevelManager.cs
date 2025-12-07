using TowerDefense.Towers.Placement;
using TowerDefense.Level.Wave.Events;
using TowerDefense.Enemies.Events;
using TowerDefense.Level.Events;
using TowerDefense.Level.Wave;
using UnityEngine;

namespace TowerDefense.Level
{
    /// <summary>
    /// Manages the overall state and progression of the level including build phases,
    /// wave spawning, enemy tracking, and win/lose conditions.
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;

        [Header("References")]
        [SerializeField] private WaveManager waveManager;

        [Header("Settings")]
        [SerializeField] private float buildPhaseDuration = 5f;
        [SerializeField] private int maxEnemiesAllowedToPass = 2;

        [Tooltip("Whether the player can place towers during wave phases (for testing).")]
        [SerializeField] private bool allowTowerBuildingDuringWave = false;

        private LevelState currentState = LevelState.Intro;
        private float buildTimer = 0f;
        private int enemiesPassed = 0;

        private int currentWaveIndex = 0;
        private int totalWaves = 0;

        public float BuildTimer => buildTimer;
        public float BuildPhaseDuration => buildPhaseDuration;
        public LevelState CurrentState => currentState;
        public int EnemiesPassed => enemiesPassed;
        public int MaxEnemiesAllowed => maxEnemiesAllowedToPass;

        private void Awake()
        {
            // Singleton pattern to ensure a single LevelManager instance.
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
            // We expect waveManager to have the wave definitions
            totalWaves = (waveManager != null && waveManager.waves != null)
                         ? waveManager.waves.Length
                         : 0;

            // Subscribe to wave events (now handled via WaveEventsBus)
            WaveEventsBus.OnWaveStarted += HandleWaveStarted;
            WaveEventsBus.OnWaveCompleted += HandleWaveCompleted;
            WaveEventsBus.OnAllWavesCompleted += HandleAllWavesCompleted;

            // Subscribe to enemy events
            EnemyEventsBus.OnEnemyReachedGoal += HandleEnemyReachedGoal;
            EnemyEventsBus.OnEnemyDied += HandleEnemyDied;

            totalWaves = (waveManager.waves != null) ? waveManager.waves.Length : 0;

            // Move out of Intro into initial build phase
            ChangeState(LevelState.Building);
        }

        private void Update()
        {
            if (currentState == LevelState.Building)
            {
                buildTimer += Time.deltaTime;

                // When build time expires, start next wave or declare win if no waves remain.
                if (buildTimer >= buildPhaseDuration)
                {
                    if (currentWaveIndex < totalWaves)
                    {
                        ChangeState(LevelState.SpawningEnemies);
                        waveManager.StartWave(currentWaveIndex);  // Spawn the wave at current index.
                    }
                    else
                    {
                        ChangeState(LevelState.Win);
                    }
                }
            }
        }

        /// <summary>
        /// Changes the current level state, resets timers as needed, and adjusts tower building permissions.
        /// </summary>
        /// <param name="newState">The new level state.</param>
        private void ChangeState(LevelState newState)
        {
            // On leaving Building state, reset the build timer
            if (currentState == LevelState.Building)
            {
                buildTimer = 0f;
            }

            currentState = newState;

            // Adjust whether building is allowed
            switch (newState)
            {
                case LevelState.Building:
                    // Always allow building in a Build Phase
                    TowerPlacementManager.Instance.SetCanPlaceTowers(true);
                    break;

                case LevelState.SpawningEnemies:
                    TowerPlacementManager.Instance.SetCanPlaceTowers(allowTowerBuildingDuringWave);
                    break;

                case LevelState.Win:
                case LevelState.Lose:
                    // No building in end states
                    TowerPlacementManager.Instance.SetCanPlaceTowers(false);
                    break;

                default:
                    TowerPlacementManager.Instance.SetCanPlaceTowers(false);
                    break;
            }

            // Inform other systems (like the UI) about the state change.
            LevelEventsBus.RaiseLevelStateChanged(newState);

            Debug.Log($"Level state changed to {newState}");
        }


        #region Enemy and Wave Event Handlers

        /// <summary>
        /// Called when an enemy reaches the goal. Increments the count and checks for loss condition.
        /// </summary>
        private void HandleEnemyReachedGoal(Enemies.Enemy enemy)
        {
            enemiesPassed++;
            if (enemiesPassed >= maxEnemiesAllowedToPass)
            {
                PlayerLost();
            }
        }

        /// <summary>
        /// Placeholder for handling enemy death (e.g., tracking stats).
        /// </summary>
        private void HandleEnemyDied(Enemies.Enemy enemy)
        {
            // e.g. track stats, do XP, etc.
        }

        /// <summary>
        /// Handles logic when a new wave starts.
        /// </summary>
        private void HandleWaveStarted(int waveNumber)
        {
            if (currentState == LevelState.Lose || currentState == LevelState.Win)
            {
                return;
            }

            Debug.Log($"Wave {waveNumber} started!");
        }

        /// <summary>
        /// Handles logic when a wave is completed. Transitions back to build phase if waves remain.
        /// </summary>
        private void HandleWaveCompleted(int waveNumber)
        {
            if (currentState == LevelState.Lose || currentState == LevelState.Win)
            {
                return;
            }

            Debug.Log($"Wave {waveNumber} completed!");

            // We finished wave index = waveNumber - 1
            currentWaveIndex++;

            // If we still have more waves left, let's go back to a Build Phase
            if (currentWaveIndex < totalWaves)
            {
                ChangeState(LevelState.Building);
            }
            else
            {
                // Let AllWavesCompleted handle the final Win
                Debug.Log("All waves done, waiting for OnAllWavesCompleted event...");
            }
        }

        /// <summary>
        /// Called when all waves are completed. Sets the level to Win if not already lost.
        /// </summary>
        private void HandleAllWavesCompleted()
        {
            if (currentState == LevelState.Lose || currentState == LevelState.Win)
            {
                return;
            }

            Debug.Log("All waves completed – let's see if we can declare a win!");
            // If the player hasn't lost yet, we can set Win
            ChangeState(LevelState.Win);
        }

        #endregion

        /// <summary>
        /// Handles player loss by changing state and stopping wave spawning.
        /// </summary>
        public void PlayerLost()
        {
            ChangeState(LevelState.Lose);

            waveManager.StopAllWaves();

            // show a "You Lost" screen
            Debug.Log("Game Over! Too many enemies reached the end.");
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;

            // Unsubscribe from ALL events you subscribed to
            WaveEventsBus.OnWaveStarted -= HandleWaveStarted;
            WaveEventsBus.OnWaveCompleted -= HandleWaveCompleted;
            WaveEventsBus.OnAllWavesCompleted -= HandleAllWavesCompleted;

            EnemyEventsBus.OnEnemyReachedGoal -= HandleEnemyReachedGoal;
            EnemyEventsBus.OnEnemyDied -= HandleEnemyDied;
        }
    }
}

// *Comments and Headers Were Written with the Help of LLM* 