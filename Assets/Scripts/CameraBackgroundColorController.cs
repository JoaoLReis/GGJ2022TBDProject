using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBackgroundColorController : MonoBehaviour
{
	Camera cam;
	[SerializeField]
	Color positiveColor;
	[SerializeField]
	Color negativeColor;

	private void Awake()
	{
		cam = GetComponent<Camera>();
	}

	private void Start()
	{
		ApplyGravityFromPlanets.ChangePolarity += OnChangePolarity;
	}

	private void OnDestroy()
	{
		ApplyGravityFromPlanets.ChangePolarity -= OnChangePolarity;
	}

	public void OnChangePolarity(bool positivePolarity)
	{
		cam.backgroundColor = positivePolarity ? positiveColor : negativeColor;
	}
}
