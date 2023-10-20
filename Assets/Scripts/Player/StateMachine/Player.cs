using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform Orientation;

    [Header("Player Stats")]
    [SerializeField] private PlayerData _playerData;

    [Header("Movement")]
    public Vector3 PlayerVelocity;

    [Header("Bools states")]
    public bool IsGrounded;
    public bool IsDashing;
    public bool IsReadyToJump = true;
    public bool OnSlope;

    [Header("Player Velocity")]
    public float MoveSpeed;

    public bool isDead;

    public RaycastHit SlopeHit;

    private float _dashCDTimer;

    private Rigidbody _rb;
    private CharacterController _controller;
    private PlayerLook _look;
    private InputHandler _inputHandler;
    [SerializeField] private HeadBobController _headBob;

    private Animator _animator;
    private AnimationStateController _animatorStateController;

    private CinemachineVirtualCamera _camera;
    public PlayerData PlayerData { get { return _playerData; } set { _playerData = value; } }
    public Rigidbody Rigidbody { get { return _rb; } }
    public CharacterController Controller { get { return _controller; } }
    public Animator Animator { get { return _animator; } }
    public AnimationStateController AnimatorStateController { get { return _animatorStateController; } }
    public CinemachineVirtualCamera Camera { get { return _camera; } }
    public HeadBobController HeadBob { get { return _headBob; } }
    public float DashCDTimer { get { return _dashCDTimer; } set { _dashCDTimer = value; } }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _controller = GetComponent<CharacterController>();
        _look = GetComponent<PlayerLook>();
        _inputHandler = GetComponent<InputHandler>();
        _animator = GetComponent<Animator>();
        _animatorStateController = GetComponent<AnimationStateController>();
        //_headBob = GetComponent<HeadBobController>();
    }

    private void Start()
    {
        _camera = FindObjectOfType<CinemachineVirtualCamera>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (!IsReadyToJump && IsGrounded) Invoke(nameof(ResetJump), PlayerData.JumpCD);
    }

    private void ResetJump()
    {
        IsReadyToJump = true;
    }
}
