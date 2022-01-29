using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityWaves : MonoBehaviour
{
    [SerializeField] private ParticleSystem wavesAttraction;
    [SerializeField] private ParticleSystem wavesRepulsion;

    private PlanetStats planetStats;
    private float objectSizeMultiplier;
    private float gravityRadius;

    private bool isPositiveMass;

    private void Awake()
    {
        planetStats = GetComponent<PlanetStats>();
        gravityRadius = transform.GetChild(0).GetComponent<CircleCollider2D>().radius;
    }

    void Start()
    {
        objectSizeMultiplier = transform.localScale.x;
        isPositiveMass = planetStats.mass > 0;

        ChangeSize(ref wavesAttraction);
        ChangeSize(ref wavesRepulsion);
        ApplyDirection(isPositiveMass);

        ApplyGravityFromPlanets.ChangePolarity += InvertDirection;
    }

    private void InvertDirection(bool polarity)
    {
        ApplyDirection(isPositiveMass == polarity);
    }

    public void ApplyDirection(bool isPulling)
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
        sizeOverLifetimeModule.sizeMultiplier = objectSizeMultiplier + gravityRadius;
    }
}
