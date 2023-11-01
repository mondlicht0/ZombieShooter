using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _camera;
    [SerializeField] private float _interactDistance = 3f;
    [SerializeField] private LayerMask _interactLayers;

    private InputHandler _inputHandler;
    private PlayerUI _playerUI;

    private void Awake()
    {
        _inputHandler = GetComponent<InputHandler>();
        _playerUI = GetComponent<PlayerUI>();
    }

    private void Update()
    {
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
        RaycastHit hitInfo;

        if(Physics.Raycast(ray, out hitInfo, _interactDistance, _interactLayers))
        {
            if (hitInfo.collider.TryGetComponent(out Interactable interact))
            {
                _playerUI.UpdateText(interact.text);

                if (_inputHandler.IsInteract) interact.BaseInteract();
            }
        }
    }
}
