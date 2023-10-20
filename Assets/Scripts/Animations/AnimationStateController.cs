using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    private Animator _animator;
    private InputHandler _inputHandler;
    private Player _player;
    private PlayerStateMachine _playerState;
    private PlayerStateFactory _playerStateFactory;

    public Vector3 RootMotion;

    private float _velocity;
    private bool _isGrounded;
    private int VelocityHash;
    private int _isWalkingHash;
    private int _isRunningHash;
    private int _isJumpingHash;
    private int _isGroundedHash;
    private int _inputXHash;
    private int _inputYHash;

    [SerializeField]
    private float jumpSpeed;

    [SerializeField]
    private float jumpButtonGracePeriod;

    [SerializeField]
    private float rotationSpeed;

    private float ySpeed = -0.5f;
    private float originalStepOffset;
    private float? lastGroundedTime;
    private float? jumpButtonPressedTime;

    public Transform CameraTransform;

    public int IsWalking { get { return _isWalkingHash; } }
    public int IsRunning { get { return _isRunningHash; } }
    public int IsJumping { get { return _isJumpingHash; } }
    public int IsGrounded { get { return _isGroundedHash; } }

    private void Awake()
    {

    }

    private void Start()
    {

    }

    private void Update()
    {

    }
}
