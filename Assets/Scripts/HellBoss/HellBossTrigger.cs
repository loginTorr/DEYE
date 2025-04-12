using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellBossTrigger : MonoBehaviour
{
    public GameObject hellBoss;
    public GameObject hellBossDoor;

    private bool hellBossSpawned = false;
    void OnTriggerEnter(Collider trigger)
    {
        if (trigger.CompareTag("Player") && !hellBossSpawned)
        {
            hellBoss.SetActive(true);
            hellBossDoor.SetActive(true);
            hellBossSpawned = true;
        }
    }
}
