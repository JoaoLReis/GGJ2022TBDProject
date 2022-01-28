using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsAnimation : MonoBehaviour
{

    [SerializeField]
    public float rotatingAngle;

    // Start is called before the first frame update
    void Start()
    {
        rotatingAngle = 40f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotatingAngle * Time.deltaTime);
    }
}
