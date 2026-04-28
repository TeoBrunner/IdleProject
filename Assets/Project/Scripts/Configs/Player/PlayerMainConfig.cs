using UnityEngine;
namespace Configs
{
    public class PlayerMainConfig
    {
        public readonly float MaxSpeed;
        public readonly float Acceleration;
        public readonly float Deceleration;
        public PlayerMainConfig(
            float maxSpeed, 
            float acceleration, 
            float deceleration)
        {
            MaxSpeed = maxSpeed;
            Acceleration = acceleration;
            Deceleration = deceleration;
        }
    }
}

