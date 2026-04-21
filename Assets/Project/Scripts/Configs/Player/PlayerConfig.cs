using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/Player")]
public class PlayerConfig : ScriptableObject
{
    [Header("Movement")]
    public float MaxSpeed = 6f;
    public float Acceleration = 20f;
    public float Deceleration = 25f;
}
