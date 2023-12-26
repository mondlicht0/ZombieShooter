using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapSettings : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private bool _rotateWithTarget = true;

    public Transform Target { get => _target; }
    public bool RotateWithTarget { get => _rotateWithTarget;}
}
