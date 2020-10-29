using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyLater : MonoBehaviour
{
    [SerializeField] private float destroyTimeout;
    void Start()
    {
        Destroy(gameObject, destroyTimeout);
    }
}
