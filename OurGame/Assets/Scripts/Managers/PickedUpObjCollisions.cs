using UnityEngine;

public class PickedUpObjCollisions : MonoBehaviour
{
    private ParticleSystem particles; // reference to particles on this object if present

    private void OnCollisionEnter(Collision collision)
    {
        // Attempt to get any ParticleSystem component on this object
        TryGetComponent<ParticleSystem>(out particles);

        // If a particle system exists, trigger it when object hits something
        if (particles)
        {
            ParticleSystem.MainModule particles_main = particles.main; // access main particle settings
            particles_main.startColor = Color.white; // set particles to white
            particles.Play(); // play impact effect
        }
    }
}
