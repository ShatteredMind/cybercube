using TMPro;
using UnityEngine;

public class Target : MonoBehaviour
{
    public HealthBar bar;

    public int cakesRequired = 10;
    public float xspeed = 0.01f;
    public float yspeed = 0.01f;
    public float damageTime = 0.3f;
    public bool isFull;

    private SkinnedMeshRenderer meshFilter;
    private int cakesEaten = 0;
    private Renderer render;
    private float timer;
    // bool update problem: https://forum.unity.com/threads/solved-boolean-change-not-being-reflected-in-update.795528/
    private bool isFeed;
    [SerializeField] private Animator anim;
    [SerializeField] private Platform platform;
    [SerializeField] private Mesh[] meshes;

    public void ManageCollision()
    {
        cakesEaten += 1;
        meshFilter.sharedMesh = meshes[cakesEaten];

    }

    private void Start()
    {
        meshFilter = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
    }

/*    private void OnTriggerEnter(Collider other)
    {
        animationController.SetTrigger("CakeCaught");
    }*/

    void Update()
    {
        if (cakesEaten >= cakesRequired) 
        {
            isFull = true;
            if (isFull)
            {
                Destroy(gameObject);
                Destroy(platform.gameObject);
            }
        }

        if (isFeed)
        {
            timer += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Projectile"))
        {
            transform.localScale += new Vector3(0.1f, 0.1f, transform.localScale.z);
            anim.SetTrigger("Sample");
        }
    }

/*    private void ManageMaterialState()
    {
        timer += Time.deltaTime;
        if (timer > damageTime)
        {
            EndTimer();
        }
    }

    private void StartTimer() 
    {
        isFeed = true;
    }

    private void EndTimer()
    {
        isFeed = false;
        timer = 0f;
    }

    private void ChangeColor(Color color)
    {
        render = GetComponent<Renderer>();
        render.material.color = color;
    }*/
}
