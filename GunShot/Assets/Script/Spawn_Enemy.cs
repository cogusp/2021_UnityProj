using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Enemy : MonoBehaviour
{
    public bool enableSpawn = false;
    public GameObject slime;
    public GameObject turtle;

    void Spawn()
    {
        float rand = Random.Range(-3f, 3f);

        if (enableSpawn)
        {
            GameObject E1 = (GameObject)Instantiate(slime, new Vector3(rand, 1.1f, 0f), Quaternion.identity);
            GameObject E2 = (GameObject)Instantiate(turtle, new Vector3(rand, 1.1f, 0f), Quaternion.identity);
        }
    }

    void Start()
    {
        InvokeRepeating("Spawn", 10, 1);
    }
}
