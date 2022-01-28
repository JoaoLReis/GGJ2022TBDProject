using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CapType {
    Plastic,
    Metal
}

[CreateAssetMenu(fileName = "New CapStats", menuName = "CapStats")]
public class CapStats : ScriptableObject
{
    public string capName;
    public float mass;
    public PhysicsMaterial2D physicsMaterial;
    public float linearDrag;
    public float maxDrag;
    public float power;
    public Material capMaterial;
    public CapType capType;
}
