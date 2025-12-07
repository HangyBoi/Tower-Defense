using TowerDefense.Enemies.Data;

namespace TowerDefense.Enemies.Movement
{
    /// <summary>
    /// Factory class to create the appropriate movement strategy based on the enemy's movement type.
    /// </summary>
    public static class MovementStrategyFactory
    {
        /// <summary>
        /// Creates an instance of a movement strategy based on the specified MovementType.
        /// </summary>
        /// <param name="type">The movement type (Flying or Ground).</param>
        /// <returns>An instance of IMovementStrategy corresponding to the movement type.</returns>
        public static IMovementStrategy CreateStrategy(MovementType type)
        {
            switch (type)
            {
                case MovementType.Flying:
                    return new FlyingMovementStrategy();
                case MovementType.Ground:
                default:
                    return new GroundMovementStrategy();
            }
        }
    }
}
