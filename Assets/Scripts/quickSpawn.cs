using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quickSpawn : MonoBehaviour
{
    public GameObject spawnThis;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Spawn", 1, 1);
    }

    void Spawn()
    {
        Instantiate(spawnThis, new Vector3(-10f, 1.14f, Random.Range(-2.5f, 2.5f)), Quaternion.Euler(0f, -90f, 0f));
    }
}
