using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHellGoons : MonoBehaviour
{
    public Transform[] hellGoonSpawns;
    public GameObject hellGoonPrefab;

    public static SpawnHellGoons SpawnHellGoonsInstance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        SpawnHellGoonsInstance = this;

        foreach (Transform hellGoonSpawn in hellGoonSpawns)
        {
            GameObject hellGoon = Instantiate(hellGoonPrefab, hellGoonSpawn.position, hellGoonSpawn.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Spawn() {
        foreach (Transform hellGoonSpawn in hellGoonSpawns)
        {
            GameObject hellGoon = Instantiate(hellGoonPrefab, hellGoonSpawn.position, hellGoonSpawn.rotation);
        }
    }
}
