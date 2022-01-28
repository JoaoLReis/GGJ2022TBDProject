using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapRotation : MonoBehaviour
{
    Transform parentTransform;
    public Vector3 rotationToKeep;

    void Awake() {
        parentTransform = transform.parent;
    }

    void LateUpdate() {
        transform.rotation = parentTransform.rotation;
        transform.Rotate(rotationToKeep, Space.World);
    }
}
