using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CameraTouchScreenController : MonoBehaviour
{
    public GraphicRaycaster _raycaster;

    [SerializeField] private CinemachineInputProvider _inputProvider;

    [Header("Input Actions References")]
    [SerializeField] private InputActionReference _lookWithoutJoystick;
    [SerializeField] private InputActionReference _lookWithJoystick;
    

    private void Update()
    {
        if (Touchscreen.current.touches[0].isInProgress)
        {
            if (EventSystem.current.IsPointerOverGameObject(Touchscreen.current.touches[0].touchId.ReadValue()))
            {
                _inputProvider.XYAxis = _lookWithJoystick;
            }

            else
            {
            _inputProvider.XYAxis = _lookWithoutJoystick;

            }

        }
    }
}
