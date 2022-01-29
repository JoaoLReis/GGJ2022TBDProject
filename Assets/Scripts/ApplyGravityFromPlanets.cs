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

    private bool inOrbit = false;
    Rigidbody2D rb2d;

    // Use this for initialization
    void Start()
    {
        PlayerController.PlayerRespawn += TryToDetach;
        PlayerController.OnClick += TryToDetach;
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
                Debug.Log("Kaboom"); 
                return;
			}

            Debug.Log("Applying Gravity");
            //apply gravitational force
            float r = dir.magnitude;
            float totalForce = -(starMass) / (r * r);
            Vector2 force = dir.normalized * totalForce;
            rb2d.AddForce(force);
        }
    }

    public void TryToDetach()
	{
        Debug.Log("Trying to Detach");
        if (inOrbit && closestPlanet != null)
        {
            Detach();
            PlayerExitOrbit.Invoke();
        }
        positivePolarity = !positivePolarity;
        starMass *= -1;
    }

    void Detach()
    {
        Debug.Log("Detaching");
        closestPlanet.GetComponent<HingeJoint2D>().enabled = false;
        anchorPoint = Vector2.zero;
        closestPlanet = null;
        inOrbit = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlanetOrbit"))
        {
            if(closestPlanet != null && rb2d.velocity.magnitude < 4f)
            {
                //enter orbit
                Debug.Log("Entering Orbit with planet" + collision.gameObject.name + "!");

                PlayerOrbit.Invoke();
                closestPlanet.GetComponent<HingeJoint2D>().enabled = true;
                inOrbit = true;
            }
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("PlanetGravity"))
        {
            closestPlanet = collision.transform.parent;
            anchorPoint = closestPlanet.position;
            starMass = closestPlanet.GetComponent<PlanetStats>().mass * (positivePolarity ? 1 : -1);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlanetGravity") && closestPlanet == collision.transform.parent)
        {
            Debug.Log("Exiting gravity field for planet " + collision.transform.parent.name + "!");
            anchorPoint = Vector2.zero;
            closestPlanet = null;
            inOrbit = false;
        }
    }
}
