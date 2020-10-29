using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private GameObject target;
    private Rigidbody rb;

    public void EnableTarget()
    {
        target.GetComponent<Dummy>().Die(transform.position);
    }

    private void Start()
    {
        target.transform.parent = transform;
    }
}
