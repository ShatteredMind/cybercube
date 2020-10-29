using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cake : MonoBehaviour
{
    public float startForce = 50f;
    public float lowerBound = 5f;
    public float upperBound = 25f;
    public float rotateSpeed = 50f;
    public Texture2D splashTexture;
    public bool isThrown;

    [SerializeField] protected float throwForceX;
    [SerializeField] protected float throwForceY;
    [SerializeField] protected float throwForceZ;
    [SerializeField] protected float xThreshold;
    [SerializeField] protected float forceThresholdUpper;
    [SerializeField] protected float forceThresholdLower;
    [SerializeField] private float forceThresholdUpperY;
    [SerializeField] protected GameObject fire;
    [SerializeField] protected GameObject spawner;

    protected Rigidbody rb;
    protected Vector3 startPos, endPos, direction, distanceOffset;
    float touchTimeStart, touchTimeFinish, timeInterval; // to calculate swipe time to sontrol throw force in Z direction
    protected float holdingTime;

    private float positionTimer = 0.1f;
    private Vector3 lastPosition;
    private Vector3 preLastPosition;

    #region protected Methods

    protected void Start()
    {
        rb = GetComponent<Rigidbody>();
        preLastPosition = lastPosition = transform.position;
    }
        
    protected void Update()
    {
/*        if (rb.isKinematic)
        {
            gameObject.transform.position = GetMouseWorldPos() + distanceOffset;

            positionTimer -= Time.deltaTime;
            if (positionTimer < 0)
            {
                preLastPosition = lastPosition;
                lastPosition = transform.position;
                positionTimer = 0.1f;
            }

            if (Camera.main.WorldToScreenPoint(gameObject.transform.position).x > xThreshold)
            {
                ThrowCake(transform.position - preLastPosition);
                DestroySelf(2f);
                preLastPosition = lastPosition = spawner.transform.position;
            }
        }*/

    }

    protected void ManageCollision()
    {
        SoundManager.manager.splatSound.Play();
        Destroy(gameObject);
    }

    protected void OnBecameInvisible()
    {
        DestroySelf(1f);
    }

    protected void GrabCake()
    {
        rb.isKinematic = true;
        startPos = Input.mousePosition;
        holdingTime = Time.time;
    }

    public void ThrowCake(Vector3 direction)
    {
        rb.AddForce(direction.x * throwForceZ, direction.y * throwForceY, forceThresholdUpper);
        isThrown = true;
        GameManager.manager.CakeCount -= 1;
        UIManager.manager.UpdateCakeCount();
        DestroySelf(1.5f);
    }

    protected float MinFloat(float a, float b)
    {
        return a > b ? b : a;
    }

    protected Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    protected void DestroySelf(float destroyTime)
    {
        Destroy(gameObject, destroyTime);
        if (GameManager.manager.cakes[0] == gameObject)
        {
/*            GameManager.manager.cakes[0] = null;
            Debug.Log("Dest");*/
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Dummy") || other.gameObject.name.Contains("Spider"))
        {
            Dummy dummyObject = other.gameObject.GetComponent<Dummy>();
            if (!dummyObject.IsDead)
            {
                dummyObject.Die(transform.position);
            }
        }
    }

    #endregion
}
