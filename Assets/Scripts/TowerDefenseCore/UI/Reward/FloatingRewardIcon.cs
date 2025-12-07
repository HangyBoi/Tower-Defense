using UnityEngine;

namespace TowerDefense.UI
{
    /// <summary>
    /// Visual icon for rewards that animates and returns to a pool after completion.
    /// </summary>
    public class FloatingRewardIcon : MonoBehaviour
    {
        [Tooltip("Upward movement distance over the lifetime of the icon.")]
        public float moveDistance = 1f;

        [Tooltip("Duration (in seconds) for the icon to fade out.")]
        public float fadeDuration = 1f;

        private Vector3 startPosition;
        private float timer;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// Activates the icon at the specified starting position.
        /// </summary>
        public void Activate(Vector3 startPos)
        {
            transform.position = startPos;
            startPosition = startPos;
            timer = 0f;
            SetAlpha(1f);
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Sets the sprite's transparency.
        /// </summary>
        private void SetAlpha(float alpha)
        {
            Color c = spriteRenderer.color;
            c.a = alpha;
            spriteRenderer.color = c;
        }

        private void Update()
        {
            // Ensure the icon faces the camera
            if (Camera.main != null)
            {
                // Billboarding: set the rotation to match the camera's forward direction
                transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
            }

            // Animate upward movement and fade out over time
            timer += Time.deltaTime;
            float t = timer / fadeDuration;

            // Move upward proportionally
            transform.position = startPosition + Vector3.up * (moveDistance * t);
            // Fade out
            SetAlpha(1f - t);

            // When the animation is complete, return this icon to the pool
            if (t >= 1f)
            {
                FloatingRewardManager.Instance.ReturnToPool(this);
            }
        }
    }
}

// *Comments and Headers Were Written with the Help of LLM* 
