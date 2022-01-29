using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalController : MonoBehaviour
{
    private bool isPulling = true;
    private PlayerController _playerController;
    public float orbitDistance = 1.0f;
    private PointEffector2D _gravityEffector;
    private bool playerInRange = false;
    private bool playerInOrbit = false;


    public Transform objectToOrbit; //Object To Orbit
    public Vector3 orbitAxis = Vector3.up; //Which vector to use for Orbit
    public float orbitRadius = 75.0f; //Orbit Radius
    public float orbitRadiusCorrectionSpeed = 0.5f; //How quickly the object moves to new position
    public float orbitRoationSpeed = 10.0f; //Speed Of Rotation arround object
    public float orbitAlignToDirectionSpeed = 0.5f; //Realign speed to direction of travel

    private Vector3 orbitDesiredPosition;
    private Vector3 previousPosition;
    private Vector3 relativePos;
    private Quaternion rotation;

    void Start()
    {
        _playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        _gravityEffector = GetComponent<PointEffector2D>();
    }

    public void ChangeDirection()
    {
        isPulling = !isPulling;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D");
        if(!isPulling)
            return;

        if(other.gameObject == _playerController.gameObject)
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("OnTriggerExit2D");
        if(other.gameObject == _playerController.gameObject)
        {
            playerInRange = false;
        }
    }

    private void HandleOrbit()
    {
        playerInOrbit = true;

        Vector2 vectorToCenter = transform.position - _playerController.transform.position;
        Vector2 currentUp = _playerController.transform.up;
        Vector2 tangent = new Vector2(vectorToCenter.y, -vectorToCenter.x);
        if(Vector2.Dot(currentUp, tangent) < 0 )
        {
            tangent = new Vector2(-vectorToCenter.y, vectorToCenter.x);
        }
        _playerController.transform.LookAt(_playerController.transform.position + new Vector3(tangent.x, tangent.y, _playerController.transform.position.z), -_playerController.transform.forward);

        _gravityEffector.enabled = false;
    }

    private void Release()
    {
        playerInOrbit = false;
        _gravityEffector.enabled = true;
    }
 
    void Update() {
        if(!playerInRange)
            return;

        if(playerInOrbit)
        {
            objectToOrbit = transform;
            Transform thisTransform = _playerController.transform;


            thisTransform.RotateAround (objectToOrbit.position, orbitAxis, orbitRoationSpeed * Time.deltaTime);
            orbitDesiredPosition = (thisTransform.position - objectToOrbit.position).normalized * orbitRadius + objectToOrbit.position;
            thisTransform.position = Vector3.Slerp(thisTransform.position, orbitDesiredPosition, Time.deltaTime * orbitRadiusCorrectionSpeed);
        
            // relativePos = thisTransform.position - previousPosition;
            // relativePos.z = 0;
            // rotation = Quaternion.LookRotation(relativePos);
            // thisTransform.rotation = Quaternion.Slerp(thisTransform.rotation, rotation, orbitAlignToDirectionSpeed * Time.deltaTime);
            // previousPosition = thisTransform.position;
        }
        else
        {
            Vector2 playerPosition = new Vector2(_playerController.transform.position.x, _playerController.transform.position.y);
            Vector2 thisPosition = new Vector2(transform.position.x, transform.position.y);
            if((playerPosition - thisPosition).magnitude <= orbitDistance)
            {
                HandleOrbit();
            }
        }
    }
}
