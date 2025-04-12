using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;
    public float lifetime = 5f;
    public GameObject bulletSpawn;

    private Rigidbody bulletRB;

    // Start is called before the first frame update
    void Start()
    {
        bulletRB = GetComponent<Rigidbody>();
        bulletSpawn = GameObject.Find("BulletSpawn");
        bulletRB.velocity = bulletSpawn.transform.forward * speed;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
    }

}
