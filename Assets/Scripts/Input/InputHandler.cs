using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
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

    public InputActionAsset InputActions;
    private InputAction _mouseClick;
    private InputAction _mouseHold;

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

    private void Awake()
    {
        _mouseClick = InputActions.FindAction("Attack");
        _mouseHold = InputActions.FindAction("Aim");
    }

    private void OnEnable()
    {
        _mouseClick.started += OnMouseClickStarted;
        _mouseClick.canceled += OnMouseClickCanceled;
        _mouseHold.started += OnMouseHoldStarted;
        _mouseHold.canceled += OnMouseHoldCanceled;

        _mouseClick.Enable();
        _mouseHold.Enable();
    }

    private void OnDisable()
    {
        _mouseClick.started -= OnMouseClickStarted;
        _mouseClick.canceled -= OnMouseClickCanceled;
        _mouseHold.started -= OnMouseHoldStarted;
        _mouseHold.canceled -= OnMouseHoldCanceled;

        _mouseClick.Disable();
        _mouseHold.Disable();
    }

    private void OnMouseClickStarted(InputAction.CallbackContext context)
    {
        _isAttack = true;
    }


    private void OnMouseClickCanceled(InputAction.CallbackContext context)
    {
        _isAttack = false;
    }

    private void OnMouseHoldStarted(InputAction.CallbackContext context)
    {
        _isAim = true;
    }

    private void OnMouseHoldCanceled(InputAction.CallbackContext context)
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

        if (_movementInput != Vector2.zero) _isMovementPressed = true;
        else _isMovementPressed = false;

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
        _isInteract = context.performed;
    }

/*    public void OnAttackInput(InputAction.CallbackContext context)
    {
        _isAttack = context.performed;
    }

    public void OnAimInput(InputAction.CallbackContext context)
    {
        _isAim = context.performed;
    }*/
}
