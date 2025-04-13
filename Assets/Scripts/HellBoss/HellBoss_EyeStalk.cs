using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using UnityEngine;

enum HellBossState { Wait, Laser, Wave, Envirnoment }


public class HellBoss_EyeStalk : MonoBehaviour
{
    public Animator anim;

    public float health = 500f;
    public float followSpeed;
    public int count;

    public Transform PlayerPos;
    public Transform EyePos;

    public GameObject eye;
    public GameObject laser;
    public GameObject goomba;


    private bool isLaser;
    private bool isWave;
    private bool isEnvirnoment;

    private HellBossState HellState;

    public static HellBoss_EyeStalk HellBossInstance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        isLaser = false; isWave = false; isEnvirnoment = false;
        HellBossInstance = this;
        Invoke("Spawn", 1f);
        HellState = HellBossState.Laser;
    }

    // Update is called once per frame
    void Update()
    {
        var step = followSpeed * Time.deltaTime;
        Quaternion rot = transform.rotation;

        transform.LookAt(PlayerPos.transform);
        transform.rotation = Quaternion.Lerp(rot, transform.rotation, step);

        if (health <= 0f) { Destroy(gameObject); }

        if (isLaser == false && isWave == false && isEnvirnoment == false)
        {

            switch (HellState)
            {
                case HellBossState.Wait:
                    Invoke("StartAttack", 2f); Debug.Log("StartAtack");
                    break;

                case HellBossState.Laser:
                    Debug.Log("LaserState");
                    isLaser = true;
                    StartCoroutine(LaserAttack());
                    //call laser function
                    //set case.Wait
                    break;

                case HellBossState.Wave:
                    Debug.Log("Wave");
                    isWave = true;
                    StartCoroutine(WaveAttack());
                    break;

                case HellBossState.Envirnoment:
                    StartCoroutine(EnvironemntAttack());
                    break;
            }
        }
    }


    IEnumerator LaserAttack()
    {
        yield return new WaitForSeconds(5.0f);
        followSpeed = 0.8f;
        laser.SetActive(true); Debug.Log("startLaser");
        yield return new WaitForSeconds(10.0f);
        followSpeed = 1f;
        laser.SetActive(false); Debug.Log("endLaser");
        isLaser = false; 
        HellState = HellBossState.Wave;

    }

    IEnumerator WaveAttack()
    {
        yield return new WaitForSeconds(3f);
        while (count <= 10)
        {
            float randomX = UnityEngine.Random.Range(0f, 70f);
            float RandomZ = UnityEngine.Random.Range(150f, 210f);
            Instantiate(goomba, new Vector3(randomX, 5, RandomZ), Quaternion.identity);
            count++;
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(5.0f);
        HellState = HellBossState.Envirnoment;
        isWave = false;
    }

    IEnumerator EnvironemntAttack()
    {
        Debug.Log("Enviro");
        yield return new WaitForSeconds(3.0f);

    }



    void Spawn()
    {
        anim.enabled = false;
    }

    void StartAttack()
    {
        HellState = HellBossState.Laser;
    }

    public void EyeHit()
    {
        Debug.Log("Eye Health Drop");
        health -= 20f;
    }

    public void StalkHit()
    {
        Debug.Log("Stalk Health Drop");
        health -= 10f;
    }

}
