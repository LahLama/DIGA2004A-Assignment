using UnityEngine;

public class PickedUpObjCollisions : MonoBehaviour
{
    private ParticleSystem particles;
    private void OnCollisionEnter(Collision collision)
    {
        particles = GetComponent<ParticleSystem>();

        particles.Play();
    }
}
