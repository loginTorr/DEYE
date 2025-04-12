using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoonMain : MonoBehaviour
{
    public float health = 60f;
    public float speed = 10f;
    public float minDist = 5f;
    public float maxDist = 40f;
    public Transform target;
    public Transform eyeball;
    public GameObject goonBeamPrefab;
    
    private bool beamReady = true;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) {
            return;
        }

        transform.LookAt(target);

        float distance = Vector3.Distance(transform.position, target.position);

        if ((distance > minDist) && (distance < maxDist))
        {
            transform.position += (transform.forward * speed * Time.deltaTime);
        }

        if ((distance < maxDist) && beamReady)
        {
            FireBeam();
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            health -= 10f;
        }
    }

    void FireBeam()
    {
        GameObject goonBeam = Instantiate(goonBeamPrefab, eyeball.position, eyeball.rotation);
        beamReady = false;
        Invoke("ReadyBeam", 5f);
    }

    void ReadyBeam()
    {
        beamReady = true;
    }
}
