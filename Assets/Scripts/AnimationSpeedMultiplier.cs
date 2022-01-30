using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSpeedMultiplier : MonoBehaviour
{
    public float speedMultiplier = 1f;
    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
        anim.SetFloat("Speed", speedMultiplier);
    }
}
