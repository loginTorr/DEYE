using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoonBeam : MonoBehaviour
{
    public float speed = 15f;
    public float lifetime = 2f;
    public Transform target;

    private Rigidbody beamRB;

    // Start is called before the first frame update
    void Start()
    {
        if (target == null) 
        {
            if (GameObject.FindWithTag("Player") != null)
            {
                target = GameObject.FindWithTag("Player").GetComponent<Transform>();
            }
        }

        transform.LookAt(target);
        beamRB = GetComponent<Rigidbody>();
        beamRB.velocity = transform.forward * speed;
        Destroy(gameObject, lifetime);
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Bounds"))
        {
            Destroy(gameObject);
        }
    }
}