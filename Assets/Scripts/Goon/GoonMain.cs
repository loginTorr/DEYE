using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoonMain : MonoBehaviour
{
    public float health = 60f;

    public static GoonMain GoonInstance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        GoonInstance = this;
    }

    // Update is called once per frame
    void Update()
    {
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
}
