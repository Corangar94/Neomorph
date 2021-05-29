using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]

public class RotateObject : MonoBehaviour
{
    [SerializeField]
    private float xRotation;
    void Update()
    {
        transform.Rotate(Vector3.right * (xRotation * Time.deltaTime));
    }
}
