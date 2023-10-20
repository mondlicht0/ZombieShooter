using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Movement Values")]
    public float WalkSpeed = 5f;
    public float RunSpeed = 5f;
    public float DashSpeed = 5f;
    public float JumpHeight = 7f;

    [Header("Jumping")]
    public float JumpForce;
    public float JumpCD;
    public float AirMultiplier;

    [Header("Dashing")]
    public float DashForce = 10f;
    public float DashDuration = 0.25f;
    public float DashCD = 2f;
    public bool AllowAllDirections;
    public bool UseCameraForward;

    [Header("Gravity")]
    public float Gravity = -9.8f;
    public float GroundedGravity = -0.005f;
    public float FallMultiplier = 2f;

    [Header("Ground")]
    public float PlayerHeight = 2f;
    public float GroundDrag = 5f;
    public LayerMask GroundLayer;

    [Header("Slope Settings")]
    public float MaxSlopeAngle = 40;

    [Header("Camera Settings")]
    public float IdleFOV = 70f;
    public float RunFOV = 90f;
    
}
