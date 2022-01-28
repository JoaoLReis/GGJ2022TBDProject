using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    public GameObject finishParticleSystem;

    void OnTriggerEnter2D(Collider2D col)
    {
        PlayerController controller = col.GetComponent<PlayerController>();
        ParticleSystem ps = Instantiate(finishParticleSystem, col.transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
        ps.Play();
        Destroy(ps.gameObject, ps.main.duration + 0.1f);
        if (controller != null)
        {
            controller.OnEndTrack();
        }
    }
}