using UnityEngine;

namespace TowerDefense.Towers.Projectiles
{
    /// <summary>
    /// Handles debuff logic, including applying and refreshing non-stacking debuffs.
    /// </summary>
    public class DebuffManager : MonoBehaviour
    {
        private DebuffType activeDebuff = DebuffType.None;
        private float activeDebuffValue = 0f;
        private float debuffTimer = 0f;

        /// <summary>
        /// Returns the effective multiplier after debuffs are applied.
        /// Ensures the value is between 0 and 1.
        /// </summary>
        public float GetEffectiveMultiplier()
        {
            // Ensure the multiplier stays within [0,1] to avoid negative speed.
            return Mathf.Clamp(1f - activeDebuffValue, 0f, 1f);
        }

        /// <summary>
        /// Applies or refreshes a debuff based on non-stacking rules.
        /// </summary>
        public void ApplyDebuff(DebuffType debuffType, float debuffValue, float debuffDuration)
        {
            if (debuffType == DebuffType.None)
                return;

            if (debuffType == DebuffType.Debuff)
            {
                if (activeDebuff == DebuffType.Debuff)
                {
                    // Refresh timer and update to the stronger slow value.
                    activeDebuffValue = Mathf.Max(activeDebuffValue, debuffValue);
                    debuffTimer = Mathf.Max(debuffTimer, debuffDuration);
                }
                else
                {
                    activeDebuff = DebuffType.Debuff;
                    activeDebuffValue = debuffValue;
                    debuffTimer = debuffDuration;
                }
            }
            // Implement additional debuff types (e.g. Poison).
        }

        private void Update()
        {
            // Countdown the debuff timer and reset when it expires.
            if (activeDebuff != DebuffType.None)
            {
                debuffTimer -= Time.deltaTime;
                if (debuffTimer <= 0f)
                {
                    activeDebuff = DebuffType.None;
                    activeDebuffValue = 0f;
                }
            }
        }
    }
}

// *Comments and Headers Were Written with the Help of LLM* 