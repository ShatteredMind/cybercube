using System.Linq;
using UnityEngine;

public class Bomb : Cake
{
    public float power = 10.0f;
    public float radius = 30.0f;
    public float upforce = 1.0f;

    [SerializeField] private GameObject explosion;

    new protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Dummy") || other.gameObject.name.Contains("Spider") || other.gameObject.name == "Plane" || other.gameObject.name == "Obstacle")
        {
            Detonate();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Detonate();
    }

    private void Detonate()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider hit in colliders)
        {
            ProcessDummyHit(hit);
        }
        GetComponent<BoxCollider>().enabled = false;
        SoundManager.manager.explosionSound.Play();
        Destroy(gameObject);
    }

    private Collider[] Filter(Collider[] colliders)
    {
        return colliders.Where(c => c.name == "Dummy" || c.transform.root.name == "Dummy" || c.name == "Spider" || c.transform.root.name == "Spider").ToArray();
    }

    private void ProcessDummyHit(Collider hit)
    {
        Dummy dum = hit.gameObject.GetComponent<Dummy>();
        BodyPart[] bodyParts = hit.gameObject.GetComponentsInChildren<BodyPart>();
        Rigidbody[] bodies = hit.gameObject.GetComponentsInChildren<Rigidbody>();
        bodies.Concat(hit.gameObject.GetComponents<Rigidbody>());

        if (dum)
        {
            dum.Die(transform.position);
        }

        foreach (BodyPart bodyPart in bodyParts)
        {
            bodyPart.Shatter();
        }

        foreach (Rigidbody body in bodies)
        {
            body.AddExplosionForce(power, transform.position, radius, upforce, ForceMode.Impulse);
        }
    }
}
