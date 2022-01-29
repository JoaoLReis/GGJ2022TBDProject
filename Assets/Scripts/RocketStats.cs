using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RocketType {
    Plastic,
    Metal
}

[CreateAssetMenu(fileName = "New RocketStats", menuName = "RocketStats")]
public class RocketStats : ScriptableObject
{
    public string rocketName;
    public float mass;
    public float maxInputDrag;
    public float power;
    public RocketType rocketType;
}
