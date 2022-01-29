using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityWaves : MonoBehaviour
{
    [SerializeField] private ParticleSystem wavesAttraction;
    [SerializeField] private ParticleSystem wavesRepulsion;

    private OrbitalController orbitalController;
    private float objectSizeMultiplier;

    private void Awake()
    {
        orbitalController = GetComponent<OrbitalController>();
    }

    void Start()
    {
        objectSizeMultiplier = transform.localScale.x;

        ChangeSize(ref wavesAttraction);
        ChangeSize(ref wavesRepulsion);
        ChangeDirection(true);
        
    }

    // // For testing purposes
    // void Update()
    // {
    //     if (Input.GetButtonDown("Fire1"))
    //     {
    //         ChangeDirection(true);
    //     }
    //     if (Input.GetButtonDown("Fire2"))
    //     {
    //         ChangeDirection(false);
    //     }
    // }

    public void ChangeDirection(bool isPulling)
    {
        if (isPulling)
        {
            wavesAttraction.gameObject.SetActive(true);
            wavesRepulsion.gameObject.SetActive(false);
        }
        else
        {
            wavesAttraction.gameObject.SetActive(false);
            wavesRepulsion.gameObject.SetActive(true);
        }
    }

    private void ChangeSize(ref ParticleSystem waves)
    {
        waves.transform.localScale *= objectSizeMultiplier;
        
        var sizeOverLifetimeModule = waves.sizeOverLifetime;
        sizeOverLifetimeModule.sizeMultiplier = objectSizeMultiplier + orbitalController.orbitDistance;
    }
}
