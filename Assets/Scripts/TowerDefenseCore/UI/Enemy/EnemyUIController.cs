using TowerDefense.Enemies;
using UnityEngine;

namespace TowerDefense.UI
{
    /// <summary>
    /// Attaches a health bar UI to an enemy so that it follows the enemy in world space.
    /// </summary>
    public class EnemyUIController : MonoBehaviour
    {
        [SerializeField] private GameObject healthBarUIPrefab;
        [SerializeField] private Vector3 healthBarOffset = new(0, 2f, 0);

        private EnemyHealthBarUI healthBarUI;
        private Enemy enemy;

        private void Awake()
        {
            enemy = GetComponent<Enemy>();
        }

        private void Start()
        {
            // Instantiate the health bar UI as a child so it follows the enemy.
            GameObject healthBarGO = Instantiate(healthBarUIPrefab, transform.position + healthBarOffset, Quaternion.identity);
            healthBarGO.transform.SetParent(transform);

            healthBarUI = healthBarGO.GetComponent<EnemyHealthBarUI>();
            if (healthBarUI != null && enemy != null)
            {
                healthBarUI.SetTarget(enemy);
            }
        }
    }
}

// *Comments and Headers Were Written with the Help of LLM* 
