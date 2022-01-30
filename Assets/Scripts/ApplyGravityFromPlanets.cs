using System.Collections;
using System;
using UnityEngine;

public class ApplyGravityFromPlanets : MonoBehaviour
{
    // orbits around a "star" at the origin with fixed mass
    public float starMass = 1000f;
    private Vector2 anchorPoint = Vector2.zero;
    private Transform closestPlanet;

    private bool positivePolarity = true;

    public static Action PlayerCrash;
    public static Action PlayerOrbit;
    public static Action PlayerExitOrbit;
    public static Action<bool> ChangePolarity;

    private bool inOrbit = false;
    Rigidbody2D rb2d;

    public bool PositivePolarity => positivePolarity;

    // Use this for initialization
    void Start()
    {
        PlayerController.PlayerRespawn += TryToDetach;
        PlayerController.OnClick += OnClick;
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(closestPlanet != null && !inOrbit)
		{
            Vector2 v2Pos = new Vector2(transform.position.x, transform.position.y);
            Vector2 dir = (anchorPoint - v2Pos);

            if (dir.magnitude < closestPlanet.localScale.x * 0.5f + 0.1f)
            {
                Detach();
                PlayerCrash.Invoke();
                //Debug.Log("Kaboom"); 
                return;
			}

            //apply gravitational force
            float r = dir.magnitude;
            float totalForce = -(starMass) / (r * r);
            Vector2 force = dir.normalized * totalForce;
            rb2d.AddForce(force);
        }
    }

    public void OnClick()
	{
        TryToDetach();
        positivePolarity = !positivePolarity;
        if(closestPlanet != null) starMass = closestPlanet.GetComponent<PlanetStats>().GetPolarity(PositivePolarity); 
        ChangePolarity.Invoke(positivePolarity);
    }

    public void TryToDetach()
	{
        if (inOrbit && closestPlanet != null)
        {
            Detach();
            PlayerExitOrbit.Invoke();
        }
    }

    void Detach()
    {
        if(closestPlanet.GetComponent<HingeJoint2D>() != null)
            closestPlanet.GetComponent<HingeJoint2D>().enabled = false;
        anchorPoint = Vector2.zero;
        closestPlanet = null;
        inOrbit = false;
        GameManager.Instance.Camera.SetTarget(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlanetOrbit"))
        {
            if (closestPlanet != null)
            {
                starMass = closestPlanet.GetComponent<PlanetStats>().GetPolarity(PositivePolarity);
                if(starMass > 0)
                    return;

                Vector2 vectorToCenter = transform.position - closestPlanet.position;
                Vector2 forward = transform.up;
                Vector3 tangent = new Vector3(vectorToCenter.y, -vectorToCenter.x, 0);
                int floatMultiplier = 1;
                if (Vector3.Dot(forward, tangent) < 0)
                {
                    tangent = new Vector2(-vectorToCenter.y, vectorToCenter.x);
                }
                else
                {
                    floatMultiplier = -1;
                }

                if (Mathf.Abs(Vector3.Dot(forward, tangent)) < 0.5f)
                {
                    return;
                }

                if (rb2d.velocity.magnitude < 10f)
                {
                    //enter orbit
                    //Debug.Log("Entering Orbit with planet" + collision.gameObject.name + "!");

                    PlayerOrbit.Invoke();
                    HingeJoint2D joint = closestPlanet.GetComponent<HingeJoint2D>();
                    JointMotor2D motorRef = joint.motor;
                    motorRef.motorSpeed *= floatMultiplier;
                    joint.motor = motorRef;
                    joint.enabled = true;
                    inOrbit = true;
                    GameManager.Instance.Camera.SetTarget(closestPlanet.gameObject);
                }
            }
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("PlanetGravity"))
        {
            closestPlanet = collision.transform.parent;
            anchorPoint = closestPlanet.position;
            starMass = closestPlanet.GetComponent<PlanetStats>().GetPolarity(PositivePolarity);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            PlayerCrash.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlanetGravity") && closestPlanet == collision.transform.parent)
        {
            //Debug.Log("Exiting gravity field for planet " + collision.transform.parent.name + "!");
            Detach();
        }
    }
}
