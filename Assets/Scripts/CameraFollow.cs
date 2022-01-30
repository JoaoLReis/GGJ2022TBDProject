using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameManager gameManager;
    public float speed;
    public Vector3 offset;

    private Camera cam;
    private GameObject targetPlayer;
    private InterpolatedTransform interpolatedTransform;
    private InterpolatedTransformUpdater interpolatedTransformUpdater;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    public void SetTarget(GameObject targetToFollow)
    {
        targetPlayer = targetToFollow;
    }

    private void FixedUpdate()
    {
        if (targetPlayer == null)
            return;
        Vector3 targetPosition = targetPlayer.transform.position + calculateCamOffset();
        Vector3 position = transform.position;
        position = Vector3.Lerp(
            position,
            new Vector3(targetPosition.x, targetPosition.y, position.z),
            speed * Time.deltaTime
        );
        transform.position = position;
    }

    Vector3 calculateCamOffset()
	{
        return Vector3.up * (cam.orthographicSize - 2);
	}
}