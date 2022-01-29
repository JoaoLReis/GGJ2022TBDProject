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

    public GameObject decal;
    public GameObject collisionParticleSystemPrefab;
    [SerializeField]
    private GameObject puffPrefab;
    public Color playerColor;

    Touch touch;

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
        decal.SetActive(true);
    }

    public void EndTurn()
    {
        playerMovement.Lr.positionCount = 0;
        playerMovement.Lr.enabled = false;
        DisablePlayer();
    }

    public void OnEndTrack()
    {
        Debug.Log("Finished track! " + gameObject.name);
        hasFinishedTrack = true;
        PlayerFinished.Invoke();
    }

    private void Update()
    {
        if (canMove)
        {
            CheckDragInput();
        } else {

		}

        isMoving = playerMovement.Rb.velocity.magnitude > MIN_VELOCITY_EPSILON;
        
        if (isOutOfBounds && !startedRespawning)
        {
            StartCoroutine(nameof(OutOfBoundsRespawn));
        }
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
        playerMovement.Rb.velocity = Vector2.zero;

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
                isTouchZooming = true;
            } else if(!isTouchZooming) {
                CheckDrag();
            }
        }
        else
        {
            isTouchZooming = false;
            if (Input.mouseScrollDelta.y != 0)
                Zoom(Input.mouseScrollDelta.y, -ScrollZoomAmount);

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

        zoomTimer = Mathf.Clamp01(zoomTimer + Time.deltaTime * ScrollZoomSpeed);
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, currentZoom, zoomTimer);
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

    private void CheckClick()
	{
        touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Ended || Input.GetMouseButtonUp(0))
        {
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
        float size = camera.orthographicSize + deltaMagnitudeDiff * speed;
        // set min and max value of Clamp function upon your requirement
        currentZoom = Mathf.Clamp(size, ZoomMinBound, ZoomMaxBound);
        zoomTimer = 0;
    }

    private void LateUpdate()
    {
    }
}

