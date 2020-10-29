using UnityEngine;

public class Dummy : MonoBehaviour
{
    [SerializeField] private GameObject bottomBorder;
    [SerializeField] private GameObject leftBorder;
    [SerializeField] private GameObject rightBorder;
    [SerializeField] private GameObject blood;

    public bool IsDead { get; private set; } = false;
    public bool IsFallen { get; private set; } = false;
    public int Score { get; private set; } = 25;

    public void Die(Vector3 position)
    {
        // GameObject particleSystem = Instantiate(blood, position, transform.rotation);
        // ParticleSystem system = particleSystem.GetComponent<ParticleSystem>();
        // GameManager.manager.ChangeWallParticleSystem(system);

        IsDead = true;
        setRigidbodyState(false);
        setColliderState(true);
        transform.parent = null;
        GameManager.manager.UpdatePlayerScore(Score);
    }

    private void Start()
    {
        setRigidbodyState(true);
        setColliderState(false);
    }

    private void Update()
    {
        Transform[] transforms = GetComponentsInChildren<Transform>();

        foreach (Transform trans in transforms)
        {
            if ((trans.position.y < bottomBorder.transform.position.y || trans.position.x < leftBorder.transform.position.x || trans.position.x > rightBorder.transform.position.x) && !IsFallen)
            {
                GameManager.manager.deadTargets.Add(this);
                IsFallen = true;
            }
        }
    }

    private void setRigidbodyState(bool state)
    {
        Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody body in bodies)
        {
            body.isKinematic = state;
        }

        if (!state)
        {
            Destroy(gameObject.GetComponent<BoxCollider>());
        }
    }

    private void setColliderState(bool state)
    {
        Collider[] bodies = GetComponentsInChildren<Collider>();

        foreach (Collider body in bodies)
        {
            body.enabled = state;
        }

        gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.root.name.Contains("LevelObjects"))
        {
            Die(transform.position);
        }
    }
}
