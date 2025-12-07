using TowerDefense.Towers;
using UnityEngine;
using System;

namespace TowerDefense.UI
{
    /// <summary>
    /// Handles tower selection via mouse input and broadcasts the selected tower to listeners.
    /// </summary>
    public class TowerSelectionHandler : MonoBehaviour
    {
        public static event Action<Tower> OnTowerSelected;

        private void Update()
        {
            // Raycast from mouse, or OnMouseDown in each tower prefab
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                LayerMask TowerLayerMask = LayerMask.GetMask("Tower");
                if (Physics.Raycast(ray, out RaycastHit hit, 1000f, TowerLayerMask))
                {
                    Tower clickedTower = hit.collider.GetComponentInParent<Tower>();
                    if (clickedTower != null)
                    {
                        OnTowerSelected?.Invoke(clickedTower);
                    }
                }
            }

            // Right click to deselect.
            if (Input.GetMouseButtonDown(1))
            {
                // Right click -> hide tower panel
                OnTowerSelected?.Invoke(null);
            }
        }
    }
}

// *Comments and Headers Were Written with the Help of LLM* 
