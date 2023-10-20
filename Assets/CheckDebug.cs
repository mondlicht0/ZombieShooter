using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckDebug : MonoBehaviour
{
    public GameObject Cube;
    public GameObject Sphere;

    public Vector3 pos;

    private void Update()
    {
        Cube.transform.position = pos;
    }
}
