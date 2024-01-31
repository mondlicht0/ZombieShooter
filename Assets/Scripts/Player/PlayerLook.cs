using Cinemachine;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [Header("Sway Settings")]
    [SerializeField] private float smooth;
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;

    [Header("Other Settings")]
    [SerializeField] private CinemachineVirtualCamera _playerCamera;
    [SerializeField] private CinemachineInputProvider _inputProvider;
    [SerializeField] private Transform _weaponHolder;
    [SerializeField] private Transform _armature;
    [SerializeField] private Transform _camera;

    [SerializeField] private InputHandler _input;

    public Quaternion _orientationRotation;
    private Quaternion _handsRotation;
    private Quaternion _weaponRotation;

    private float xRotation;
    private float yRotation;

    private bool _isLocked;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }


    private void Update()
    {
        if (!_isLocked)
        {
            Rotation();
        }


    }

    private void LateUpdate()
    {
        if (!_isLocked)
        {
            transform.rotation = _orientationRotation;
            _armature.rotation = _weaponRotation;
            _weaponHolder.rotation = _weaponRotation;
        }
    }

    private void Rotation()
    {
        Quaternion playerCameraRotation = _playerCamera.transform.rotation;
        float yRotation = playerCameraRotation.eulerAngles.y;
        float xRotation = playerCameraRotation.eulerAngles.x;

        Vector3 rotate = new Vector3(_input.MouseInput.x, _input.MouseInput.y * 2f, 0);
        transform.eulerAngles = transform.eulerAngles - rotate;


        _orientationRotation = Quaternion.Euler(0, yRotation, 0);
        _weaponRotation = Quaternion.Slerp(_weaponRotation, Quaternion.Euler(xRotation, yRotation, 0), smooth * Time.deltaTime);
    }

    public void LockOnStore()
    {
        _isLocked = true;
        _input.enabled = false;

        Cursor.lockState = CursorLockMode.Confined;
    }

    public void LockOffStore()
    {
        _isLocked = false;
        _input.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
    }
}
