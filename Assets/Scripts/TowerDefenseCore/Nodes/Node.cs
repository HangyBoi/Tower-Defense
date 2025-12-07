using TowerDefense.Enemies.Movement;
using UnityEngine;

namespace TowerDefense.Nodes
{
    /// <summary>
    /// Represents a node in the level graph. 
    /// Handles interactions with enemies and triggers node selection.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class Node : MonoBehaviour
    {
        /// <summary>
        /// Retrieves the next node by using the NodeSelector attached to this GameObject.
        /// </summary>
        /// <returns>The next Node if available; otherwise, null.</returns>
        public Node GetNextNode()
        {
            var selector = GetComponent<NodeSelector>();
            return selector != null ? selector.GetNextNode() : null;
        }

        /// <summary>
        /// When an enemy enters the node's collider, instructs its movement controller to proceed.
        /// </summary>
        /// <param name="other">The collider that entered the trigger.</param>
        public virtual void OnTriggerEnter(Collider other)
        {
            // Check if the colliding object has an EnemyMovementController.
            if (other.TryGetComponent<EnemyMovementController>(out var movementController))
            {
                // Let the movement controller decide the next node.
                movementController.ProceedToNextNode(this);
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// Ensures the collider is set as a trigger in the editor.
        /// </summary>
        protected void OnValidate()
        {
            var trigger = GetComponent<Collider>();
            if (trigger != null)
            {
                trigger.isTrigger = true;
            }
        }
#endif
    }
}

// *Comments and Headers Were Written with the Help of LLM* 