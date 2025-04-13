using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulPiece : MonoBehaviour
{
    public Transform Player;
    public Player PlayerScript;
    public GameObject BossDoor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(transform.position, Player.position);  

        if (dist < 5 && Input.GetKeyDown(KeyCode.E))
        {
            PlayerScript.KeyPiece1 = true;
            BossDoor.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
