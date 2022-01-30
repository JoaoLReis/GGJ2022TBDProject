using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITarget : MonoBehaviour
{
    public float MinX = -480;
    public float MaxX = 480;
    public float MinY = -440;
    public float MaxY = -100;
    [Range(-60,60)] public float angle = 0;

    private RectTransform targetImage;
    private float posX, posY;

    private Transform playerCam;
    private Transform target;
    
    private void Awake()
    {
        targetImage = GetComponent<RectTransform>();
    }

    void Start()
    {
        posX = 0;
        posY = 0;
        playerCam = Camera.main.transform;
        target = GameObject.FindWithTag("EndTarget").transform;
    }

    void Update()
    {
        if (playerCam && target)
        {
            var direction = (target.position - playerCam.position);
            angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

            if (angle < -60 || angle > 60)
            {
                targetImage.localScale = Vector3.Lerp(targetImage.localScale, Vector3.zero, 6 * Time.deltaTime);
                angle = Mathf.Clamp(angle, -60, 60);
            }
            else
            {
                targetImage.localScale = Vector3.Lerp(targetImage.localScale, Vector3.one, 6 * Time.deltaTime);
            }
            
            var radians = (-angle + 90) * Mathf.Deg2Rad;
            
            posX = Mathf.Cos(radians) * MaxX;
            posY = MinY + (Mathf.Sin(radians) * Mathf.Abs(MaxY - MinY));
            targetImage.anchoredPosition = new Vector2(posX, posY);
            targetImage.eulerAngles = new Vector3(0, 0, -angle * .95f);
        }
    }
}
