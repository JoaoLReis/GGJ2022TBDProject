using System.Collections;
using System;
using UnityEngine;

public class ApplyGravityFromPlanets : MonoBehaviour
{
    // orbits around a "star" at the origin with fixed mass
    public float starMass = 1000f;
    private Vector2 anchorPoint = Vector2.zero;
    private Transform closestPlanet;

    public static Action PlayerCrash;
    public static Action PlayerOrbit;

    private bool inOrbit = false;
    Rigidbody2D rb2d;

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(anchorPoint != Vector2.zero)
		{
            Vector2 v2Pos = new Vector2(transform.position.x, transform.position.y);
            Vector2 dir = (anchorPoint - v2Pos);
            if (!inOrbit)
            {
                if(closestPlanet != null && rb2d.velocity.magnitude < 2.5f)
				{
                    //enter orbit
                    closestPlanet.GetComponent<HingeJoint2D>().enabled = true;
                    PlayerOrbit.Invoke();
                    inOrbit = true;
                    return;
                }

                if (Vector2.Dot(rb2d.velocity.normalized, dir.normalized) < 0.1f)
			    {
                    PlayerCrash.Invoke();
                    Debug.Log("Kaboom");
                    return;
			    }

                //apply gravitational force
                float r = dir.magnitude;
                Debug.Log("r= " + r);
                float totalForce = -(starMass) / (r * r);
                Vector2 force = dir.normalized * totalForce;
                Debug.Log("force= " + force);
                rb2d.AddForce(force);
            }
        }
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.gameObject.layer == LayerMask.NameToLayer("Planet"))
		{
            anchorPoint = collision.transform.position;
            closestPlanet = collision.transform;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (closestPlanet == collision.transform)
        {
            anchorPoint = Vector2.zero;
            closestPlanet = null;
            inOrbit = false;
        }
    }
}
