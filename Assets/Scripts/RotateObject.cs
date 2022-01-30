using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float rotationSpeed = 0.05f;
    public Vector3 axis = Vector3.up;
    void Update()
    {
        transform.Rotate(axis * rotationSpeed);
    }
}
