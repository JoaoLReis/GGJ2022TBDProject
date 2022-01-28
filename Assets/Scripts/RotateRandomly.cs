using UnityEngine;

public class RotateRandomly : MonoBehaviour
{
    void Start()
    {
        int random = Mathf.CeilToInt(Random.Range(0, 8));
        Vector3 rotation = new Vector3(
            0,
            0,
            45 * random
        );
        transform.rotation *= Quaternion.Euler(rotation);
        transform.localScale *= Random.Range(0.65f, 1.1f);
    }
}