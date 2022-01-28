using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameManager gameManager;
    public float speed;

    private GameObject targetPlayer;
    private InterpolatedTransform interpolatedTransform;
    private InterpolatedTransformUpdater interpolatedTransformUpdater;

    private void Start()
    {
        interpolatedTransform = GetComponent<InterpolatedTransform>();
        interpolatedTransformUpdater = GetComponent<InterpolatedTransformUpdater>();
        interpolatedTransform.enabled = true;
        interpolatedTransformUpdater.enabled = true;
    }

    public void SetTarget(GameObject targetToFollow)
    {
        targetPlayer = targetToFollow;
    }

    private void FixedUpdate()
    {
        if (targetPlayer == null)
            return;

        Vector3 targetPosition = targetPlayer.transform.position;
        Vector3 position = transform.position;
        position = Vector3.Lerp(
            position,
            new Vector3(targetPosition.x, targetPosition.y, position.z),
            speed * Time.deltaTime
        );
        transform.position = position;
    }
}