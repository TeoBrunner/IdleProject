using UnityEngine;
namespace Configs
{
    public class PlayerConfig : BaseConfig
    {
        public readonly float MaxSpeed;
        public readonly float Acceleration;
        public readonly float Deceleration;
        public PlayerConfig(
            int level, 
            float maxSpeed, 
            float acceleration, 
            float deceleration) : base(level)
        {
            MaxSpeed = maxSpeed;
            Acceleration = acceleration;
            Deceleration = deceleration;
        }
    }
}

