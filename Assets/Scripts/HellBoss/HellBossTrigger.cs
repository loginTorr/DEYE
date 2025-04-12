using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellBossTrigger : MonoBehaviour
{
    public GameObject hellBoss;

    private bool hellBossSpawned = false;
    void OnTriggerEnter(Collider trigger)
    {
        if (trigger.CompareTag("Player") && !hellBossSpawned)
        {
            hellBoss.SetActive(true);
            hellBossSpawned = true;
        }
    }
}
