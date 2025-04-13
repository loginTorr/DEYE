using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPVPHall : MonoBehaviour
{
    public Transform[] hellGoonSpawns;
    public GameObject hellGoonPrefab;
    public GameObject doorOne;
    public GameObject doorTwo;

    private bool hasSpawned = false;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") && !hasSpawned)
        {
            doorOne.SetActive(true);
            doorTwo.SetActive(true);
            foreach (Transform hellGoonSpawn in hellGoonSpawns)
            {
                GameObject hellGoon = Instantiate(hellGoonPrefab, hellGoonSpawn.position, hellGoonSpawn.rotation);
            }
            hasSpawned = true;
            InvokeRepeating("CheckEnemy", 20, 3);
        }
    }

    void CheckEnemy()
    {
        if (!GameObject.FindWithTag("Goon"))
        {
            doorOne.SetActive(false);
            doorTwo.SetActive(false);
        }
    }
}