using UnityEngine;

public class PickedUpObjCollisions : MonoBehaviour
{
    private ParticleSystem particles;
    private void OnCollisionEnter(Collision collision)
    {
        particles = GetComponent<ParticleSystem>();
        ParticleSystem.MainModule particles_main = particles.main;
        particles_main.startColor = Color.white;
        particles.Play();
    }
}
