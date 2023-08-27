using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSimulation : MonoBehaviour
{

    public ParticleSystem Part;
    public List<ParticleCollisionEvent> CollisionEvents;

    // Start is called before the first frame update
    void Start()
    {
        Part = GetComponent<ParticleSystem>();
        CollisionEvents = new List<ParticleCollisionEvent>();
    }

    private void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = Part.GetCollisionEvents(other, CollisionEvents);

        IFire affectedNPC = other.GetComponent<IFire>();
        if (affectedNPC != null)
        {
            affectedNPC.Burn();
        }
    }
}
