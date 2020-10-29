using UnityEngine;

public class Body : MonoBehaviour
{
    public GameObject leftBorder;
    public GameObject rightBorder;

    private void Awake()
    {
        // GetComponent<Rigidbody>().velocity = Vector3.zero;
        // GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        int rand = Random.Range(0, SoundManager.manager.shardSounds.Length);
        int a = Random.Range(0, 1);
        
        if (InBounds() && a == 0 && collision.gameObject.name != "Cube")
        {
            SoundManager.manager.shardSounds[rand].Play();
        }
    }

    private bool InBounds()
    {
        return transform.position.x > leftBorder.transform.position.x &&
            transform.position.x < rightBorder.transform.position.x &&
            transform.position.z > Camera.main.transform.position.z;
    }
}
