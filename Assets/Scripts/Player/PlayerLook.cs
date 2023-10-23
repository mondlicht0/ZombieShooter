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
    [SerializeField] private Transform _weaponHolder;
    [SerializeField] private Transform _armature;
    [SerializeField] private Transform _camera;

    [SerializeField] private InputHandler _input;

    public Quaternion _orientationRotation;
    private Quaternion _handsRotation;
    private Quaternion _weaponRotation;

    private float xRotation;
    private float yRotation;

    /*    private void Update()
        {
            float mouseX = _input.MouseInput.x * Time.deltaTime * sensX;
            float mouseY = _input.MouseInput.y * Time.deltaTime * sensY;

            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

            _weaponRotation = Quaternion.Slerp(_weaponRotation, Quaternion.Euler(xRotation, yRotation, 0), smooth * Time.deltaTime);
            _weaponHolder.rotation = _weaponRotation;
        }

        private void LateUpdate()
        {
            //_orientationRotation.rotation = Quaternion.Euler(0, yRotation, 0);

        }*/


    private void Update()
    {
        Quaternion playerCameraRotation = _playerCamera.transform.rotation;
        float yRotation = playerCameraRotation.eulerAngles.y;
        float xRotation = playerCameraRotation.eulerAngles.x;

        Vector3 rotate = new Vector3(_input.MouseInput.x, _input.MouseInput.y * 2f, 0);
        transform.eulerAngles = transform.eulerAngles - rotate;


        _orientationRotation = Quaternion.Euler(0, yRotation, 0);
        _weaponRotation = Quaternion.Slerp(_weaponRotation, Quaternion.Euler(xRotation, yRotation, 0), smooth * Time.deltaTime);


    }

    private void LateUpdate()
    {
        transform.rotation = _orientationRotation;
        _armature.rotation = _weaponRotation;
        _weaponHolder.rotation = _weaponRotation;
    }
}
