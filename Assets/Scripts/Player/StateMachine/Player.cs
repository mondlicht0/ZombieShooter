using Cinemachine;
using UnityEngine;

public class Player : MonoBehaviour
{
	public Transform Orientation;

	[Header("Player Stats")]
	[SerializeField] private PlayerData _playerData;
	
	[SerializeField] private Animator _cameraAnimator;

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
	private PlayerGunSelector _gunSelector;
	private InputHandler _inputHandler;
	private PlayerHealth _health;
	[SerializeField] private HeadBobController _headBob;

	private Animator _animator;
	private AnimationStateController _animatorStateController;

	private CinemachineVirtualCamera _camera;
	public PlayerData PlayerData { get => _playerData; set { _playerData = value; } }
	public Rigidbody Rigidbody { get => _rb; }
	public CharacterController Controller { get => _controller; }
	public Animator Animator { get => _animator; }
	public AnimationStateController AnimatorStateController { get => _animatorStateController; }
	public CinemachineVirtualCamera Camera { get => _camera; }
	public HeadBobController HeadBob { get => _headBob; }
	public PlayerGunSelector GunSelector { get => _gunSelector; }
	public PlayerHealth Health { get => _health; }
	public PlayerLook Look { get => _look; }
	public float DashCDTimer { get => _dashCDTimer; set { _dashCDTimer = value; } }

	private void Awake()
	{
		_rb = GetComponent<Rigidbody>();
		_controller = GetComponent<CharacterController>();
		_look = GetComponent<PlayerLook>();
		_inputHandler = GetComponent<InputHandler>();
		_animator = GetComponent<Animator>();
		_animatorStateController = GetComponent<AnimationStateController>();
		_gunSelector = GetComponent<PlayerGunSelector>();
		_health = GetComponent<PlayerHealth>();
		//_headBob = GetComponent<HeadBobController>();

		PlayerData.Instance = _playerData;
	}

	private void Start()
	{
		_camera = FindObjectOfType<CinemachineVirtualCamera>();
		
		_health.OnDied += Dead;
	}

	private void Update()
	{
		//if (!IsReadyToJump && IsGrounded) Invoke(nameof(ResetJump), PlayerData.JumpCD);
	}

	private void ResetJump()
	{
		IsReadyToJump = true;
	}
	
	private void Dead() 
	{
		_cameraAnimator.enabled = true;
		
		//Time.timeScale = 0f;
	}
}
