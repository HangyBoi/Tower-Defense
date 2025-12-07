using UnityEngine.SceneManagement;
using TowerDefense.Level.Events;
using TowerDefense.Level;
using UnityEngine;

namespace TowerDefense.UI
{
    /// <summary>
    /// Central manager for special overlay panels (Win, Lose, Pause).
    /// Also handles time-scale adjustments at runtime.
    /// 
    /// Responsibilities:
    /// - Show/hide pause panel and manage pausing the game
    /// - Show/hide win/lose panels based on LevelEventsBus
    /// - Listen to user input to increase or decrease Time.timeScale
    /// - Provide method to restart game without closing the app
    /// </summary>
    public class GameUIManager : MonoBehaviour
    {
        [Header("Panel References")]
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private GameObject winPanel;
        [SerializeField] private GameObject losePanel;

        [Header("Time Scale Settings")]
        [Tooltip("How much to increment or decrement time scale when pressing arrow keys.")]
        [SerializeField] private float timeScaleStep = 0.5f;
        [Tooltip("Maximum time scale speed allowed.")]
        [SerializeField] private float maxTimeScale = 5f;

        private float originalTimeScale = 1f;

        private void OnEnable()
        {
            // Listen to level-state changes so we know when to show Win or Lose
            LevelEventsBus.OnLevelStateChanged += HandleLevelStateChanged;
        }

        private void OnDisable()
        {
            LevelEventsBus.OnLevelStateChanged -= HandleLevelStateChanged;
        }

        private void Start()
        {
            // Ensure all special panels are hidden at game start
            if (pausePanel != null) pausePanel.SetActive(false);
            if (winPanel != null) winPanel.SetActive(false);
            if (losePanel != null) losePanel.SetActive(false);

            // In case the game starts with timeScale != 1:
            originalTimeScale = Time.timeScale;
        }

        private void Update()
        {
            HandleTimeScaleInput();
        }

        /// <summary>
        /// Called whenever the LevelEventsBus raises a new LevelState.
        /// We show/hide the appropriate overlays if Win or Lose.
        /// </summary>
        private void HandleLevelStateChanged(LevelState newState)
        {
            switch (newState)
            {
                case LevelState.Win:
                    ShowWinPanel();
                    break;
                case LevelState.Lose:
                    ShowLosePanel();
                    break;
                default:
                    // Hide them in case we returned to some other state 
                    HideWinPanel();
                    HideLosePanel();
                    break;
            }
        }

        #region Public Methods (wired to Buttons)

        /// <summary>
        /// Pauses the game by setting timeScale to 0 and showing the pause panel.
        /// </summary>
        public void OnPauseButton()
        {
            // Save current timescale so we can restore it
            originalTimeScale = Time.timeScale;
            Time.timeScale = 0f;

            if (pausePanel != null)
                pausePanel.SetActive(true);
        }

        /// <summary>
        /// Resumes the game from paused state.
        /// </summary>
        public void OnContinueButton()
        {
            Time.timeScale = originalTimeScale;

            if (pausePanel != null)
                pausePanel.SetActive(false);
        }

        /// <summary>
        /// Restarts the current scene without fully quitting the app.
        /// We also reset timeScale to 1 in case the game was paused or sped up.
        /// </summary>
        public void OnRestartButton()
        {
            Time.timeScale = 1f;
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }

        #endregion

        #region Private Helpers

        private void ShowWinPanel()
        {
            if (winPanel != null)
                winPanel.SetActive(true);
        }

        private void HideWinPanel()
        {
            if (winPanel != null)
                winPanel.SetActive(false);
        }

        private void ShowLosePanel()
        {
            if (losePanel != null)
                losePanel.SetActive(true);
        }

        private void HideLosePanel()
        {
            if (losePanel != null)
                losePanel.SetActive(false);
        }

        /// <summary>
        /// Increases/decreases Time.timeScale when pressing Left/Right arrows.
        /// Also ensure it never goes below 0 or above maxTimeScale.
        /// </summary>
        private void HandleTimeScaleInput()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Time.timeScale += timeScaleStep;
                if (Time.timeScale > maxTimeScale)
                {
                    Time.timeScale = maxTimeScale;
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Time.timeScale -= timeScaleStep;
                if (Time.timeScale < 0f)
                {
                    Time.timeScale = 0f;
                }
            }
        }

        #endregion

        private void OnDestroy()
        {
            LevelEventsBus.OnLevelStateChanged -= HandleLevelStateChanged;
        }
    }
}

// *Comments and Headers Were Written with the Help of LLM* 
