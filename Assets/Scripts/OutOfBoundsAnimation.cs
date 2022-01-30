using System.Collections;
using UnityEngine;

public class OutOfBoundsAnimation : MonoBehaviour
{
    [SerializeField]
    private float torqueMultiplier;
    
    private Rigidbody2D rigidbody;
    private MeshRenderer[] childModel3D;
    private PlayerController playerController;
    
    private void Start()
    {
        childModel3D = GetComponentsInChildren<MeshRenderer>();
        playerController = transform.GetComponent<PlayerController>();
    }

    public void StartAnimation()
    {
        foreach (MeshRenderer child in childModel3D) {
            child.enabled = false;
        }

        StartCoroutine(CheckForAnimationEnd());
    }

    private IEnumerator CheckForAnimationEnd()
    {
        yield return new WaitUntil(() => !playerController.IsOutOfBounds);
        EndAnimation();
    }

    private void EndAnimation()
    {
        foreach (MeshRenderer child in childModel3D)
        {
            child.enabled = true;
        }
    }
}