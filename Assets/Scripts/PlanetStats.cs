using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetStats : MonoBehaviour
{
	public float mass;

	public bool IsPositiveMass() 
	{
		return mass > 0;
	}

	public virtual bool CanChangePolarity()
	{
		return true;
	}

	public float GetPolarity(bool isPositivePolarity)
	{
		return mass * (!CanChangePolarity() || isPositivePolarity ? 1 : -1);
	}
}
