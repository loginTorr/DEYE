using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellBoss_EyeStalk : MonoBehaviour
{
    public float health = 500f;

    public static HellBoss_EyeStalk HellBossInstance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        HellBossInstance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void EyeHit()
    {
        health -= 20f;
    }

    public void StalkHit()
    {
        health -= 10f;
    }

}
