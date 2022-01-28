using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float power = 10f;
    public float maxDrag = 5f;
    public float minForce = 2f;
    private Rigidbody2D rb;
    private LineRenderer lr;
    private float capPosZ;
    
    public Vector3 dragStartPos;
    public Vector3 movementStartPosition;

    public CapStats capStats;
    
    public Rigidbody2D Rb
    {
        get => rb;

        set => rb = value;
    }
    
    public LineRenderer Lr
    {
        get => lr;

        set => lr = value;
    }
    
    private void Awake()
    { 
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
        lr.enabled = false;

        capPosZ = transform.position.z;
        movementStartPosition = transform.position;

        rb.mass = capStats.mass;
        rb.drag = capStats.linearDrag;
        GetComponent<CircleCollider2D>().sharedMaterial = capStats.physicsMaterial;
        maxDrag = capStats.maxDrag;
        power = capStats.power;
        transform.GetChild(0).gameObject.GetComponent<Renderer>().material = capStats.capMaterial;
    }

    public void DragStart(Vector3 touchPosition)
    {
        movementStartPosition = transform.position;
        dragStartPos = Camera.main.ScreenToWorldPoint(touchPosition);
        dragStartPos.z = capPosZ;
        
        lr.positionCount = 1;
        lr.SetPosition(0, movementStartPosition);
    }

    public void Dragging(Vector3 touchPosition)
    {
        Vector3 draggingPos = Camera.main.ScreenToWorldPoint(touchPosition);
        draggingPos.z = capPosZ;

        Vector3 force = dragStartPos - draggingPos;
        Vector3 clampedForce = Vector3.ClampMagnitude(force, maxDrag) * power;
        if (clampedForce.magnitude >= minForce) 
        {
            lr.enabled = false;
        }
        
        lr.enabled = true;

        lr.positionCount = 2;
        Vector3 newVector = Vector3.ClampMagnitude((dragStartPos - draggingPos), maxDrag);
        lr.SetPosition(1, (newVector * 0.33f) + movementStartPosition);
    }

    public bool DragRelease(Vector3 touchPosition)
    {
        bool shot = false;
        lr.positionCount = 0;
        
        Vector3 dragReleasePos = Camera.main.ScreenToWorldPoint(touchPosition);
        dragReleasePos.z = capPosZ;

        Vector3 force = dragStartPos - dragReleasePos;
        Vector3 clampedForce = Vector3.ClampMagnitude(force, maxDrag) * power;

        if(clampedForce.magnitude >= minForce) {
            rb.AddForce(clampedForce, ForceMode2D.Impulse);
            playCapSound();
            shot = true;
        }
        
        DragReset();
        return shot;
    }

    public void DragReset() {
        dragStartPos = Vector3.zero;
        lr.enabled = false;
    }

    public void ApplyDragModifier(float modifier) {
        rb.drag *= modifier;
    }

    void OnCollisionEnter2D(Collision2D col) {

        playCapSound();

        if(col.collider.tag == "CanMovableObject") {
            playCanHittingSound();
        }
        
    }

    void playCapSound() 
    {
        SoundManager.Instance.playCapSound(capStats.capType);
    }

    void playCanHittingSound() 
    {
        SoundManager.Instance.playCanHittingSound();
    }
}