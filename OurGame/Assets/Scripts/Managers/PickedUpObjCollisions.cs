using UnityEngine;

public class PickedUpObjCollisions : MonoBehaviour
{
    private ParticleSystem particles;


    private void OnCollisionEnter(Collision collision)
    {
        TryGetComponent<ParticleSystem>(out particles);
        if (particles)
        {
            ParticleSystem.MainModule particles_main = particles.main;
            particles_main.startColor = Color.white;
            particles.Play();
        }
    }


}
