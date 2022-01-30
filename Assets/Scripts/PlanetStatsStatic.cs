using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetStatsStatic : PlanetStats
{
	public override bool CanChangePolarity()
	{
		return false;
	}
}
