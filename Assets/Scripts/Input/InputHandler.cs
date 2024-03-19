using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour
{
	
	[Header("Platform")]
	public bool IsMobile;
	
	public delegate void StartTouch(Vector2 position, float time);
	public delegate void EndTouch(Vector2 position, float time);

	public event StartTouch OnStartTouch;
	public event EndTouch OnEndTouch;
	

	private Vector2 _movementInput;
	private Vector2 _mouseInput;
	private float _inputX;
	private float _inputY;
	private bool _jumpInput = false;
	private bool _isMovementPressed = false;
	private bool _isShiftPressed = false;
	private bool _isMouseMove = false;
	private bool _isAttack = false;
	private bool _isAim = false;
	private bool _isReload = false;
	private bool _isInteract = false;
	private bool _isDrop = false;

	public PlayerInput _playerInput;

	public InputActionAsset InputActions;
	private InputAction _mouseClick;
	private InputAction _mouseHold;
	public InputAction Interact;

	public Vector2 MovementInput { get => _movementInput; }
	public Vector2 MouseInput { get => _mouseInput; }
	public float InputX { get => _inputX; }
	public float InputY { get => _inputY; }
	public bool JumpInput { get => _jumpInput; set { _jumpInput = value; } }
	public bool IsMovementPressed { get => _isMovementPressed; }
	public bool IsShiftPressed { get => _isShiftPressed; }
	public bool IsAttack { get => _isAttack; }
	public bool IsAim { get => _isAim;  }
	public bool IsReload { get => _isReload;  }
	public bool IsInteract { get => _isInteract; }

	[SerializeField] private Button _attackButton;

	private void Awake()
	{
		_playerInput = new PlayerInput();
   
		_mouseClick = InputActions.FindAction("Attack");
		_mouseHold = InputActions.FindAction("Aim");
		Interact = InputActions.FindAction("Interact");
	}

	private void OnEnable()
	{
		_playerInput.Enable();
		
		_mouseClick.started += OnMouseClickStarted;
		_mouseClick.canceled += OnMouseClickCanceled;
		_mouseHold.started += OnAimClickStarted;
		_mouseHold.canceled += OnAimClickCanceled;

		_mouseClick.Enable();
		_mouseHold.Enable();
	}

	private void OnDisable()
	{
		_mouseClick.started -= OnMouseClickStarted;
		_mouseClick.canceled -= OnMouseClickCanceled;
		_mouseHold.started -= OnAimClickStarted;
		_mouseHold.canceled -= OnAimClickCanceled;

		_mouseClick.Disable();
		_mouseHold.Disable();
		
		_playerInput.Disable();
	}

	private void Start()
	{
		_playerInput.Touch.PrimaryContact.started += ctx => StartTouchPrimary(ctx);
		_playerInput.Touch.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);
	}


#region Touch
	private void StartTouchPrimary(InputAction.CallbackContext context)
	{
		if (OnStartTouch != null)
		{
			OnStartTouch(ScreenToWorld(Camera.main, _playerInput.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)context.startTime);
		}
	}

	private void EndTouchPrimary(InputAction.CallbackContext context)
	{
		if (OnEndTouch != null)
		{
			OnEndTouch(ScreenToWorld(Camera.main, _playerInput.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)context.startTime);
		}
	}

	public Vector2 PrimaryPosition()
	{
		return ScreenToWorld(Camera.main, _playerInput.Touch.PrimaryPosition.ReadValue<Vector2>());
	}
	
	public static Vector3 ScreenToWorld(Camera camera, Vector3 position)
	{
		position.z = camera.nearClipPlane;
		return camera.ScreenToWorldPoint(position);
	}
#endregion

	private void OnMouseClickStarted(InputAction.CallbackContext context)
	{
		_isAttack = true;
	}

	private void OnMouseClickCanceled(InputAction.CallbackContext context)
	{
		_isAttack = false;
	}

	private void OnAimClickStarted(InputAction.CallbackContext context)
	{
		_isAim = true;
	}

	private void OnAimClickCanceled(InputAction.CallbackContext context)
	{
		_isAim= false;
	}

	public void OnJumpInput(InputAction.CallbackContext context)
	{
		_jumpInput = context.performed;
	}

	public void OnReloadInput(InputAction.CallbackContext context)
	{
		_isReload = context.performed;
	}

	public void OnMoveInput(InputAction.CallbackContext context)
	{
		_movementInput = context.ReadValue<Vector2>();
		_inputX = MovementInput.x;
		_inputY = MovementInput.y;

		if (_movementInput != Vector2.zero) 
			_isMovementPressed = true;
		else 
			_isMovementPressed = false;

	}

	public void OnMouseInput(InputAction.CallbackContext context)
	{
		_mouseInput = context.ReadValue<Vector2>();

		if (_mouseInput != Vector2.zero) _isMouseMove = true;
		else _isMouseMove = false;
	}

	public void OnShiftInput(InputAction.CallbackContext context)
	{
		_isShiftPressed = context.performed;
	}

	public void OnInteractInput(InputAction.CallbackContext context)
	{
		_isInteract = _playerInput.OnMove.Interact.triggered;
	}

	public void OnDropInput(InputAction.CallbackContext context)
	{
		_isDrop = context.performed;
	}

	public void OnAttackInput()
	{
		_isAttack = true;
	}

	public void OnAimInput()
	{
		_isAim = true;
	}
}
