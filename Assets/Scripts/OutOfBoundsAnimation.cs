using System.Collections;
using UnityEngine;

public class OutOfBoundsAnimation : MonoBehaviour
{
    [SerializeField]
    private float torqueMultiplier;
    
    private Rigidbody rigidbody3D;
    private MeshRenderer model3D;
    private MeshRenderer[] childModel3D;
    private PlayerController playerController;
    
    private void Start()
    {
        rigidbody3D = GetComponent<Rigidbody>();
        model3D = GetComponent<MeshRenderer>();
        childModel3D = GetComponentsInChildren<MeshRenderer>();
        rigidbody3D.isKinematic = true;
        playerController = transform.parent.GetComponent<PlayerController>();
    }

    public void StartAnimation()
    {
        Debug.Log("Animating Out Of Bounds!");
        Vector2 velocity = playerController.PlayerMovement.Rb.velocity;
        
        rigidbody3D.isKinematic = false;
        rigidbody3D.constraints = RigidbodyConstraints.None;
        model3D.enabled = false;
        foreach (MeshRenderer child in childModel3D) {
            child.enabled = false;
        }


        rigidbody3D.AddTorque(new Vector3(-velocity.y, -velocity.x, 0) * torqueMultiplier, ForceMode.Impulse);

        StartCoroutine(CheckForAnimationEnd());
    }

    private IEnumerator CheckForAnimationEnd()
    {
        yield return new WaitUntil(() => !playerController.IsOutOfBounds);
        EndAnimation();
    }

    private void EndAnimation()
    {
        playerController.PlayerMovement.Rb.velocity = Vector2.zero;
        
        rigidbody3D.isKinematic = true;
        rigidbody3D.constraints = RigidbodyConstraints.FreezeRotation;
        model3D.enabled = true;
        foreach (MeshRenderer child in childModel3D)
        {
            child.enabled = true;
        }


        Transform transform3D = transform;
        Transform transform2D = playerController.transform;
        Debug.Log("Animating Out Of Bounds!");
        transform3D.position = transform2D.position;
        transform3D.rotation = transform2D.rotation;
    }
}