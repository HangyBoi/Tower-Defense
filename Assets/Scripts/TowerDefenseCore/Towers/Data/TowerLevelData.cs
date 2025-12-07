using TowerDefense.Towers.Projectiles.Data;
using UnityEngine;

public enum DebuffType
{
    None,
    Debuff,
    AOE
}

namespace TowerDefense.Towers.Data
{
    /// <summary>
    /// Data container for settings per tower level
    /// </summary>
    [CreateAssetMenu(fileName = "TowerData.asset", menuName = "TowerDefense/Tower Configuration", order = 1)]
    public class TowerLevelData : ScriptableObject
    {
        /// <summary>
        /// A description of the tower for displaying on the UI
        /// </summary>
        public string towerDescription;

        /// <summary>
        /// A description of the tower for displaying on the UI
        /// </summary>
        public string upgradeTowerDescription;

        /// <summary>
        /// Amount of damage dealt to the enemies
        /// </summary>
        public float towerDamage;

        /// <summary>
        /// How far the tower reaches the enemies
        /// </summary>
        public float towerRange;

        /// <summary>
        /// How fast the tower is shooting at opposing agents
        /// </summary>
        public float towerFireRate;

        /// <summary>
        /// Reference to the projectile effect to use for this tower level.
        /// </summary>
        public ProjectileEffectSO projectileEffectType;

        /// <summary>
        /// Having a certain debuff if needed
        /// </summary>
        public DebuffType projectileDebuffType;

        /// <summary>
        /// How much of debuff is applied
        /// </summary>
        public float projectileDebuffMultiplier;

        /// <summary>
        /// How long debuff lasts
        /// </summary>
        public float projectileDebuffDuration;

        /// <summary>
        /// Having AOE radius if needed
        /// </summary>
        public float AOERadius;

        /// <summary>
        /// Price for building the tower of first level
        /// </summary>
        public int towerCost;

        /// <summary>
        /// How much 
        /// </summary>
        public int towerUpgradeCost;

        /// <summary>
        /// The sell cost of the tower
        /// </summary>
        public int towerSellCost;

        /// <summary>
        /// The tower icon
        /// </summary>
        public Sprite towerIcon;
    }
}

// *Comments and Headers Were Written with the Help of LLM* 