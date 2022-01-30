using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    private const float MIN_VELOCITY_EPSILON = 0.3f;
    public static float RespawnWaitTime = 0.2f;
    public static float TimeOutWhenFlying = 5.0f;

    public static Action PlayerShoot;
    public static Action PlayerFinished;
    public static Action PlayerRespawn;
    public static Action OnClick;

    public GameObject collisionParticleSystemPrefab;
    [SerializeField]
    private GameObject puffPrefab;
    public Color playerColor;

    Touch touch;

    [SerializeField]
    private TrailRenderer trail;
    private Camera camera;
    
    private PlayerMovement playerMovement;
    public PlayerMovement PlayerMovement => playerMovement;
    
    private OutOfBoundsAnimation outOfBoundsAnimation;
    
    private bool isMoving;
    public bool IsMoving => isMoving;

    private bool isOutOfBounds;
    public bool IsOutOfBounds => isOutOfBounds;
    private bool startedRespawning = false;

    private bool canMove;

    private bool isTouchZooming = false;
    private bool hasFinishedTrack;

    private float currentZoom;
    private float zoomTimer;

    public bool HasFinishedTrack => hasFinishedTrack;

    float TouchZoomSpeed = 0.007f;
    float ScrollZoomSpeed = 1.0f;
    float ScrollZoomAmount = 2.0f;
    float ZoomMinBound = 2.2f;
    float ZoomMaxBound = 10.0f;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        outOfBoundsAnimation = GetComponentInChildren<OutOfBoundsAnimation>();
        camera = Camera.main;
    }

    private void Start()
    {
        ApplyGravityFromPlanets.PlayerCrash += Puff;
        ApplyGravityFromPlanets.PlayerCrash += Respawn;
        currentZoom = camera.orthographicSize;
    }

	private void OnDestroy()
    {
        ApplyGravityFromPlanets.PlayerCrash -= Puff;
        ApplyGravityFromPlanets.PlayerCrash -= Respawn;
    }

	public void StartTurn()
    {
        canMove = true;
    }

    public void EndTurn()
    {
        playerMovement.Lr.positionCount = 0;
        playerMovement.Lr.enabled = false;
        DisablePlayer();
    }

    public void OnEndTrack()
    {
        //Debug.Log("Finished track! " + gameObject.name);
        hasFinishedTrack = true;
        PlayerFinished.Invoke();
    }

    private void Update()
    {
        if (canMove)
        {
            CheckDragInput();
        } else {
            if (Input.touchCount == 2)
            {
                CheckZoom();
            }
            CheckClick();
		}

        if (Input.mouseScrollDelta.y != 0)
            Zoom(Input.mouseScrollDelta.y, -ScrollZoomAmount);

        isMoving = playerMovement.Rb.velocity.magnitude > MIN_VELOCITY_EPSILON;
        
        if(isMoving)
            transform.up = playerMovement.Rb.velocity;

        if (isOutOfBounds && !startedRespawning)
        {
            StartCoroutine(nameof(OutOfBoundsRespawn));
        }

        zoomTimer = Mathf.Clamp01(zoomTimer + Time.deltaTime * ScrollZoomSpeed);
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, currentZoom, zoomTimer);
    }

    private IEnumerator OutOfBoundsRespawn()
    {
        Debug.Log("Respawning!");
        startedRespawning = true;

        yield return new WaitForSeconds(1.0f);

        outOfBoundsAnimation.StartAnimation();
        Puff();

        yield return new WaitForSeconds(RespawnWaitTime);

        Respawn();
    }

    public void Puff()
    {
        trail.forceRenderingOff = true;
        trail.emitting = false;
        Vector3 rocketPosition = transform.position;
        Vector3 position = new Vector3(rocketPosition.x, rocketPosition.y, -8f);
        GameObject puff = Instantiate(puffPrefab, position, Quaternion.identity);
        puff.GetComponent<SpriteRenderer>().color = playerColor;
        Destroy(puff.gameObject, 1f);
        SoundManager.Instance.PlayRespawnSound();
    }

    public void Respawn()
    {
        transform.position = playerMovement.movementStartPosition;
        transform.rotation = Quaternion.identity;
        playerMovement.Rb.velocity = Vector2.zero;

        trail.forceRenderingOff = false;
        trail.emitting = true;
        trail.Clear();
        isOutOfBounds = false;
        startedRespawning = false;

        PlayerRespawn.Invoke();
    }

    private void CheckDragInput()
    {
        if (Input.touchCount > 0)
        {
            // Pinch to zoom
            if (Input.touchCount == 2) {
                CheckZoom();
            } else if(!isTouchZooming) {
                CheckDrag();
            }
        }
        else
        {
            isTouchZooming = false;

            if (Input.GetMouseButtonDown(0))
                playerMovement.DragStart(Input.mousePosition);            

            if (Input.GetMouseButton(0))
                playerMovement.Dragging(Input.mousePosition);
            
            if (Input.GetMouseButtonUp(0)) 
            {
                if (playerMovement.DragRelease(Input.mousePosition))
                {
                    Shoot();
                }
            }
        }
    }

    private void CheckZoom()
    {
        isTouchZooming = true;
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

    private void CheckClick()
	{
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended || Input.GetMouseButtonUp(0))
            {
                OnClick.Invoke();
            }
        } 
        else if (Input.GetMouseButtonUp(0))
        {
            //Debug.Log("Clicking");
            OnClick.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("OutOfBounds"))
        {
            if (!isOutOfBounds)
            {
                isOutOfBounds = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Planet"))
        {
            Puff();
            Respawn();
        }
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
    }

    void Zoom(float deltaMagnitudeDiff, float speed)
    {
        float size = camera.orthographicSize + deltaMagnitudeDiff * speed;
        // set min and max value of Clamp function upon your requirement
        currentZoom = Mathf.Clamp(size, ZoomMinBound, ZoomMaxBound);
        zoomTimer = 0;
    }

    private void LateUpdate()
    {
    }
}

