using TowerDefense.Towers;
using Unity.VisualScripting;
using UnityEngine;

namespace TowerDefense.UI.HUD
{
    /// <summary>
    /// Ghost object for tower placement.
    /// Displays a preview of a tower to indicate placement validity.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class TowerPlacementGhost : MonoBehaviour
    {
        public Tower controller { get; private set; }

        // Visualizer for the tower’s attack radius (optional)
        public GameObject radiusVisualizer;
        public float radiusVisualizerHeight = 0.02f;
        public float dampSpeed = 0.075f;

        // Materials to indicate valid and invalid placement
        public Material validMaterial;
        public Material invalidPositionMaterial;

        protected MeshRenderer[] meshRenderers;
        protected MeshRenderer radiusRenderer;

        protected Vector3 moveVelocity;
        protected Vector3 targetPosition;
        protected bool validPosition;

        public Collider ghostCollider { get; private set; }

        /// <summary>
        /// Initialize the ghost with the tower prefab it is previewing.
        /// </summary>
        public virtual void Initialize(Tower tower)
        {
            meshRenderers = GetComponentsInChildren<MeshRenderer>();

            controller = tower;
            ghostCollider = GetComponent<Collider>();
            moveVelocity = Vector3.zero;
            validPosition = false;

            // Scale radiusVisualizer based on tower range for the first level
            if (radiusVisualizer != null && controller != null && controller.levels.Length > 0)
            {
                radiusRenderer = radiusVisualizer.GetComponent<MeshRenderer>();

                float range = controller.levels[0].towerLevelData.towerRange;

                radiusVisualizer.transform.localScale = new Vector3(range*2, range * 2, 1);

                // Adjust vertical position
                Vector3 pos = radiusVisualizer.transform.localPosition;
                pos.y = radiusVisualizerHeight;
                radiusVisualizer.transform.localPosition = pos;

                radiusVisualizer.SetActive(true);
            }
        }

        /// <summary>
        /// Hides the ghost.
        /// </summary>
        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Shows the ghost and resets movement.
        /// </summary>
        public virtual void Show()
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
                moveVelocity = Vector3.zero;
                validPosition = false;
            }
        }

        /// <summary>
        /// Moves the ghost to a new world position, updates rotation, and changes material based on valid placement.
        /// </summary>
        public virtual void Move(Vector3 worldPosition, Quaternion rotation, bool validLocation)
        {
            targetPosition = worldPosition;
            if (!validPosition)
            {
                // Snap to initial position on first valid update
                validPosition = true;
                transform.position = targetPosition;
            }
            transform.rotation = rotation;
            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                // Skip the radius visualizer
                if (meshRenderer == radiusRenderer)
                    continue;

                meshRenderer.sharedMaterial = validLocation ? validMaterial : invalidPositionMaterial;
            }
        }

        protected virtual void Update()
        {
            // Smoothly move the ghost toward the target position
            Vector3 currentPos = transform.position;
            if ((currentPos - targetPosition).sqrMagnitude > 0.01f)
            {
                currentPos = Vector3.SmoothDamp(currentPos, targetPosition, ref moveVelocity, dampSpeed);
                transform.position = currentPos;
            }
            else
            {
                moveVelocity = Vector3.zero;
            }
        }
    }
}

// *Comments and Headers Were Written with the Help of LLM* 
