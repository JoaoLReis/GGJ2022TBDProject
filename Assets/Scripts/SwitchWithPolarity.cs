using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchWithPolarity : MonoBehaviour
{
    Image img;
    [SerializeField]
    Sprite onSprite;
    [SerializeField]
    Sprite offSprite;

    void Awake()
    {
        img = GetComponent<Image>();
        ApplyGravityFromPlanets.ChangePolarity += OnChangePolarity;
        PlayerController.PlayerShoot += OnPlayerShoot;
        PlayerController.PlayerRespawn += OnPlayerRespawn;
    }

	private void OnDestroy()
    {
        ApplyGravityFromPlanets.ChangePolarity -= OnChangePolarity;
        PlayerController.PlayerShoot -= OnPlayerShoot;
        PlayerController.PlayerRespawn -= OnPlayerRespawn;
    }

	public void OnChangePolarity(bool isPositive)
	{
        img.sprite = isPositive ? onSprite : offSprite;
    }

    public void OnPlayerShoot()
    {
        if (img != null)
            img.enabled = true;
    }
    public void OnPlayerRespawn()
    {
        if(img != null)
            img.enabled = false;
    }


    private void Update()
	{
		if(img.enabled && !GameManager.Instance.IsPlayerMoving())
		{
            img.enabled = false;
		}
	}
}
