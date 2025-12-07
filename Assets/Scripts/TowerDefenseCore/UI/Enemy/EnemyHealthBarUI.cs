using UnityEngine;
using TowerDefense.Enemies;
using TowerDefense.Enemies.Events;
using System.Collections;

namespace TowerDefense.UI
{
    /// <summary>
    /// Displays and animates the health bar for an enemy, including delayed health reduction effects.
    /// </summary>
    public class EnemyHealthBarUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform redBar;    // The "instant" bar
        [SerializeField] private RectTransform whiteBar;  // The "delayed" bar

        [Header("Animation Settings")]
        [SerializeField] private float animationSpeed = 2f;

        private Enemy targetEnemy;
        private float maxWidth; // The full width at 100% health
        private Coroutine adjustBarRoutine;

        /// <summary>
        /// Sets the enemy target and initializes the health bar.
        /// </summary>
        public void SetTarget(Enemy enemy)
        {
            targetEnemy = enemy;

            // Subscribe to the centralized health change event
            EnemyEventsBus.OnHealthChanged += HandleHealthChanged;

            // Store the maximum width from one of the bars
            if (redBar != null)
            {
                maxWidth = redBar.rect.width;
            }

            // Immediately update the bar to the starting health
            UpdateHealthBar(enemy.Health, enemy.MaxHealth, instant: true);
        }

        /// <summary>
        /// Handles health change events for the target enemy.
        /// </summary>
        private void HandleHealthChanged(Enemy enemy, float currentHealth, float maxHealth)
        {
            // Only update if the event belongs to this enemy
            if (enemy == targetEnemy)
            {
                UpdateHealthBar(currentHealth, maxHealth, instant: false);
            }
        }

        /// <summary>
        /// Instantly sets the red bar to the new health ratio, and smoothly
        /// transitions the white bar (if health decreases).
        /// </summary>
        private void UpdateHealthBar(float currentHealth, float maxHealth, bool instant)
        {
            // Safety check
            if (maxWidth <= 0 || redBar == null || whiteBar == null)
                return;

            float ratio = currentHealth / maxHealth;
            float targetWidth = Mathf.Clamp(ratio * maxWidth, 0, maxWidth);

            // 1) Instantly set the red bar
            Vector2 redSize = redBar.sizeDelta;
            redSize.x = targetWidth;
            redBar.sizeDelta = redSize;

            // 2) Animate the white bar if needed
            //    - If "instant" is true or health has increased, just snap to target
            //    - Otherwise, animate to the new width
            if (instant || targetWidth > whiteBar.rect.width)
            {
                // Snap white bar immediately
                Vector2 whiteSize = whiteBar.sizeDelta;
                whiteSize.x = targetWidth;
                whiteBar.sizeDelta = whiteSize;
            }
            else
            {
                // Health decreased: animate the white bar from its current width to targetWidth
                if (adjustBarRoutine != null)
                {
                    StopCoroutine(adjustBarRoutine);
                }
                adjustBarRoutine = StartCoroutine(AnimateWhiteBar(targetWidth));
            }
        }

        /// <summary>
        /// Coroutine that gradually shrinks the white bar width to match the red bar.
        /// </summary>
        private IEnumerator AnimateWhiteBar(float targetWidth)
        {
            while (Mathf.Abs(whiteBar.rect.width - targetWidth) > 0.01f)
            {
                float newWidth = Mathf.Lerp(whiteBar.rect.width, targetWidth, Time.deltaTime * animationSpeed);
                Vector2 size = whiteBar.sizeDelta;
                size.x = newWidth;
                whiteBar.sizeDelta = size;
                yield return null;
            }

            // Ensure final width is exact
            Vector2 finalSize = whiteBar.sizeDelta;
            finalSize.x = targetWidth;
            whiteBar.sizeDelta = finalSize;
        }

        private void LateUpdate()
        {
            // Billboarding: rotate to face the camera
            if (Camera.main != null)
            {
                transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                                 Camera.main.transform.rotation * Vector3.up);
            }
        }

        private void OnDestroy()
        {
            EnemyEventsBus.OnHealthChanged -= HandleHealthChanged;
        }
    }
}

// *Comments and Headers Were Written with the Help of LLM* 
