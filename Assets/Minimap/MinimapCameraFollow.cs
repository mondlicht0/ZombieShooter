using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraFollow : MonoBehaviour
{
    [SerializeField] private MinimapSettings _settings;
    [SerializeField] private float _cameraHeight;

    private void Awake()
    {
        _settings = GetComponentInParent<MinimapSettings>();
        _cameraHeight = transform.position.y;
    }

    private void Update()
    {
        Vector3 targetPosition = _settings.Target.position;
        
        transform.position = new Vector3(targetPosition.x, _cameraHeight, targetPosition.z);

        if (_settings.RotateWithTarget)
        {
            Quaternion targetRotation = _settings.Target.transform.rotation;

            transform.rotation = Quaternion.Euler(90, targetRotation.y, 0);
        }
    }
}
