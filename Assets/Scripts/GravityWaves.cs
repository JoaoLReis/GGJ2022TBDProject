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

    public bool shouldAttachToRocket = true;

    private void Awake()
    {
        planetStats = GetComponent<PlanetStats>();
        gravityRadius = transform.GetChild(0).GetComponent<CircleCollider2D>().radius;
        if(shouldAttachToRocket && GetComponent<HingeJoint2D>() != null)
            GetComponent<HingeJoint2D>().connectedBody = FindObjectOfType<PlayerController>().GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        objectSizeMultiplier = transform.localScale.x;
        isPositiveMass = planetStats.IsPositiveMass();

        ChangeSize(ref wavesAttraction);
        ChangeSize(ref wavesRepulsion);
        ApplyDirection(isPositiveMass);

        if(planetStats.CanChangePolarity())
            ApplyGravityFromPlanets.ChangePolarity += InvertDirection;
    }

	private void OnDestroy()
    {
        if (planetStats.CanChangePolarity())
            ApplyGravityFromPlanets.ChangePolarity -= InvertDirection;
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
        sizeOverLifetimeModule.sizeMultiplier = gravityRadius * 0.9f;
    }
}
