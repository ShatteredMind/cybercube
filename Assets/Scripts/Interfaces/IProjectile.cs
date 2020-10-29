using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile
{   
    GameObject Projectile { get; set; }

    void AddForceOnStart();
}
