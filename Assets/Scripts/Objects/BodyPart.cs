using UnityEngine;

public class BodyPart : MonoBehaviour
{
    [SerializeField] private int score;
    [SerializeField] private int health;
    [SerializeField] private GameObject bodyPart;
    private Rigidbody rb;

    public void Shatter()
    {
        Instantiate(bodyPart, transform.position, transform.rotation);
        GameManager.manager.UpdatePlayerScore(score);
        Destroy(gameObject);
        if (!SoundManager.manager.punchSound.isPlaying)
        {
            SoundManager.manager.punchSound.Play();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Floor" && Mathf.Abs(rb.velocity.y) > 5.0f)
        {
            Shatter();
        }
        if (collision.gameObject.name.Contains("Sphere") &&
             Mathf.Abs(collision.gameObject.GetComponent<Rigidbody>().velocity.z) > 10.0f)
        {
            Shatter();
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void RandomizeSlowMotion()
    {
        int random = Random.Range(0, 4);
        if (random == 0)
        {
            GameManager.manager.EnableSlowMotion();
        }
    }
}
