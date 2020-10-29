using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CakeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] cakes;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float delay;
    private Dictionary<string, Quaternion> rotations;

    void Start()
    {
        rotations = new Dictionary<string, Quaternion>
        {
            ["Sphere"] = Quaternion.Euler(0, -180, 0),
            ["Sphere1"] = Quaternion.Euler(0, -180, 0),
            ["Sphere2"] = Quaternion.Euler(0, -180, 0),
            ["Bomb"] = Quaternion.Euler(0, -90, 0),
            ["Hammer"] = Quaternion.Euler(0, 0, -90),
        };
        StartCoroutine(SpawnCackes());
    }

    IEnumerator SpawnCackes()
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
          
            if (GameManager.manager.cakes[0] == null && GameManager.manager.CakeCount > 0)
            {
                int cakeIndex = Random.Range(0, cakes.Length);
                GameObject projectile = cakes[cakeIndex];
                GameObject cake = Instantiate(projectile, spawnPoint.position, rotations[projectile.name]);
                GameManager.manager.cakes[0] = cake;
                var a = cake.GetComponent<Rigidbody>();
                a.useGravity = false;
            }
        }
    }

}
