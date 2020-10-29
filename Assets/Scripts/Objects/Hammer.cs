using UnityEngine;

public class Hammer : Cake
{
    new protected void Start()
    {
        base.Start();
        Transform childTransform = transform.GetChild(1);
        rb.centerOfMass = childTransform.position;
    }
}
