using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 Offset;

    [SerializeField] private Transform _target;

    private void LateUpdate()
    {
        transform.position = _target.transform.position + Offset;
        transform.localRotation = Quaternion.Euler(new Vector3(0, _target.rotation.y, 0));
    }
}
