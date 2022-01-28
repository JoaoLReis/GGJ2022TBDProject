using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    private const float MIN_VELOCITY_EPSILON = 0.3f;
    public static float RespawnWaitTime = 0.1f;

    public static Action PlayerShoot;

    public Sprite PlayerAvatar;
    public GameObject decal;
    public GameObject ghost;
    public GameObject collisionParticleSystemPrefab;
    [SerializeField]
    private GameObject puffPrefab;
    public Color playerColor;

    Touch touch;

    private PlayerMovement playerMovement;
    public PlayerMovement PlayerMovement => playerMovement;
    
    private PlayerTimer playerTimer;
    private OutOfBoundsAnimation outOfBoundsAnimation;

    [SerializeField]
    private MeshRenderer meshRenderer;
    public MeshRenderer MeshRenderer => meshRenderer;
    
    private bool isMoving;
    public bool IsMoving => isMoving;

    private bool isOutOfBounds;
    public bool IsOutOfBounds => isOutOfBounds;
   
    private bool canMove;

    private bool zooming = false;
    private bool hasFinishedTrack;

    public bool HasFinishedTrack => hasFinishedTrack;

    float TouchZoomSpeed = 0.007f;
    float ZoomMinBound = 2.2f;
    float ZoomMaxBound = 10.0f;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerTimer = GetComponent<PlayerTimer>();
        outOfBoundsAnimation = GetComponentInChildren<OutOfBoundsAnimation>();
    }

    public void StartTurn(float duration)
    {
        ghost.SetActive(false);
        canMove = true;
        decal.SetActive(true);
        playerTimer.startTimer(duration);
    }

    public void EndTurn()
    {
        playerMovement.Lr.positionCount = 0;
        playerMovement.Lr.enabled = false;
        playerTimer.endTimer();
        DisablePlayer();
    }

    public void OnEndTrack()
    {
        Debug.Log("Finished track! " + gameObject.name);
        hasFinishedTrack = true;
        playerTimer.OnEndedTrack();
        GameManager.Instance.SetPlayerFinished(this);
    }

    private void Update()
    {
        if (canMove)
        {
            CheckInput();
        }

        isMoving = playerMovement.Rb.velocity.magnitude > MIN_VELOCITY_EPSILON;
        
        if (isOutOfBounds && !isMoving)
        {
            StartCoroutine(nameof(Respawn));
        }
    }

    private IEnumerator Respawn()
    {
        isOutOfBounds = false;

        Vector3 capPosition = transform.position; 
        Vector3 position = new Vector3(capPosition.x, capPosition.y, -8f);
        GameObject puff = Instantiate(puffPrefab, position, Quaternion.identity);
        puff.GetComponent<SpriteRenderer>().color = playerColor;
        Destroy(puff.gameObject, 1f);
        SoundManager.Instance.PlayRespawnSound();
        
        yield return new WaitForSeconds(RespawnWaitTime);

        ghost.transform.position = playerMovement.movementStartPosition;
        transform.position = playerMovement.movementStartPosition;
        playerMovement.Rb.velocity = Vector2.zero;
    }

    private void CheckInput()
    {
        if (Input.touchCount > 0)
        {
            // Pinch to zoom
            if (Input.touchCount == 2) {
                CheckZoom();
                zooming = true;
            } else if(!zooming) {
                CheckDrag();
            }
        }
        else
        {
            zooming = false;

            if (Input.GetMouseButtonDown(0))
                playerMovement.DragStart(Input.mousePosition);            

            if (Input.GetMouseButton(0))
                playerMovement.Dragging(Input.mousePosition);
            
            if (Input.GetMouseButtonUp(0)) 
            {
                playerMovement.DragRelease(Input.mousePosition);
                Shoot();
            }
        }

        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, ZoomMinBound, ZoomMaxBound);
    }

    private void CheckZoom() {
        playerMovement.DragReset();

        // get current touch positions
        Touch tZero = Input.GetTouch(0);
        Touch tOne = Input.GetTouch(1);
        // get touch position from the previous frame
        Vector2 tZeroPrevious = tZero.position - tZero.deltaPosition;
        Vector2 tOnePrevious = tOne.position - tOne.deltaPosition;

        float oldTouchDistance = Vector2.Distance (tZeroPrevious, tOnePrevious);
        float currentTouchDistance = Vector2.Distance (tZero.position, tOne.position);

        // get offset value
        float deltaDistance = oldTouchDistance - currentTouchDistance;
        Zoom (deltaDistance, TouchZoomSpeed);
    }

    private void CheckDrag() {
        touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began) {
            playerMovement.DragStart(touch.position);
        }

        if (touch.phase == TouchPhase.Moved) {
            playerMovement.Dragging(touch.position);
        }

        if (touch.phase == TouchPhase.Ended) {
            if(playerMovement.DragRelease(touch.position)) {
                Shoot();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Map"))
        {
            if (!isOutOfBounds)
            {
                isOutOfBounds = true;
                outOfBoundsAnimation.StartAnimation();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!IsMoving)
            return;
        
        GameObject ps = Instantiate(collisionParticleSystemPrefab, other.GetContact(0).point, Quaternion.identity);
        ps.GetComponent<SpriteRenderer>().color = playerColor;
        Destroy(ps.gameObject, 0.5f);
    }

    private void Shoot()
    {
        DisablePlayer();
        isMoving = true;
        PlayerShoot.Invoke();
    }

    private void DisablePlayer()
    {
        canMove = false;
        decal.SetActive(false);
    }

    void Zoom(float deltaMagnitudeDiff, float speed)
    {
        Debug.Log(deltaMagnitudeDiff);
        float size = Camera.main.orthographicSize + deltaMagnitudeDiff * speed;
        // set min and max value of Clamp function upon your requirement
        Camera.main.orthographicSize = Mathf.Clamp(size, ZoomMinBound, ZoomMaxBound);
    }

    private void LateUpdate()
    {
        ghost.SetActive(transform.position != playerMovement.movementStartPosition);

        ghost.transform.position = playerMovement.movementStartPosition;
    }
}

