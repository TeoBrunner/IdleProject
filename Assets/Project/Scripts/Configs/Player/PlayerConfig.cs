using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/Player")]
public class PlayerConfig : ScriptableObject
{
    [Header("Movement")]
    [SerializeField] private float maxSpeed = 6f;
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float deceleration = 25f;

    public float MaxSpeed => maxSpeed;
    public float Acceleration => acceleration;
    public float Deceleration => deceleration;
}
