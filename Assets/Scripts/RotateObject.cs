using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float rotationSpeed = 0.05f;
    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed);
    }
}
