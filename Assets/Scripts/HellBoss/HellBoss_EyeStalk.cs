using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellBoss_EyeStalk : MonoBehaviour
{
    public float health = 500f;
    public float followSpeed;

    public Transform PlayerPos;
    public Transform EyePos;

    public GameObject eye;


    public static HellBoss_EyeStalk HellBossInstance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var step = followSpeed * Time.deltaTime;
        Quaternion rot = transform.rotation;

        transform.LookAt(PlayerPos.transform);
        transform.rotation = Quaternion.Lerp(rot, transform.rotation, step);


        if (health <= 0)
        {
            Destroy(gameObject);
        }

        //Ray ray = new Ray()
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
