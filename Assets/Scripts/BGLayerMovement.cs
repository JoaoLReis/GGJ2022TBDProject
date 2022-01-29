using UnityEngine;

public class BGLayerMovement : MonoBehaviour {
    [SerializeField] private float parallaxSpeed;
    private Transform camera;
    private float currDist;

    private Vector3 lastCamPosition;

    private void Start() {
        camera = transform.parent;
        lastCamPosition = camera.position;

    }

    private void FixedUpdate() {
        var camPosition = camera.position;
        var deltaMovement = camPosition - lastCamPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxSpeed * -1, deltaMovement.y * parallaxSpeed * -1);
        lastCamPosition = camPosition;
    }
}