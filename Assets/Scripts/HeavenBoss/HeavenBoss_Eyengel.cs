using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using UnityEngine;

enum HeavenBossState { Wait, Laser, Wave, Environment }


public class HeavenBoss_Eyengel : MonoBehaviour
{
    //public Animator anim;
    //public Animator tendrilanim;

    public float health = 1500f;
    public int count;
    public int platformCount;
    public int bulletWaveCount;

    public AudioSource audioSrc;

    public Transform PlayerPos;
    public Transform EyePos;
    public Transform bulletSpawn;
    public Transform[] platformPositions;

    public GameObject laser;
    public GameObject goomba;
    public GameObject eyengelBulletPrefab;

    public GameObject[] platforms;

    private bool isLaser;
    private bool isWave;
    private bool isEnvironment;

    private HeavenBossState HeavenState;

    public static HeavenBoss_Eyengel HeavenBossInstance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        audioSrc.volume = 0.3f;
        if (PlayerPos == null) 
        {
            if (GameObject.FindWithTag("Player") != null)
            {
                PlayerPos = GameObject.FindWithTag("Player").GetComponent<Transform>();
            }
        }
        isLaser = false; isWave = false; isEnvironment = false;
        HeavenBossInstance = this;
        Invoke("Spawn", 1f);
        HeavenState = HeavenBossState.Laser;
        platformCount = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (health <= 0f) { Destroy(gameObject); }

        if (!isEnvironment)
        {
            transform.LookAt(PlayerPos.transform);
        }

        if (isLaser == false && isWave == false && isEnvironment == false)
        {

            switch (HeavenState)
            {
                case HeavenBossState.Wait:
                    Invoke("StartAttack", 2f); Debug.Log("StartAtack");
                    break;

                case HeavenBossState.Laser:
                    Debug.Log("LaserState");
                    isLaser = true;
                    StartCoroutine(LaserAttack());
                    //call laser function
                    //set case.Wait
                    break;

                case HeavenBossState.Wave:
                    Debug.Log("Wave");
                    count = 0;
                    isWave = true;
                    StartCoroutine(WaveAttack());
                    break;

                case HeavenBossState.Environment:
                    isEnvironment = true;   
                    StartCoroutine(EnvironmentAttack());
                    break;
            }
        }
    }


    IEnumerator LaserAttack()
    {
        yield return new WaitForSeconds(2.0f);
        int bulletCount = 5;
        while (bulletCount > 0)
        {
            audioSrc.Play();
            GameObject eyengelBullet = Instantiate(eyengelBulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
            bulletCount -= 1;
            yield return new WaitForSeconds(0.3f);

        }

        isLaser = false;

        if (bulletWaveCount > 0)
        {
            HeavenState = HeavenBossState.Laser;
            bulletWaveCount -= 1;
        }
        else
        {
            HeavenState = HeavenBossState.Wave;
        }
    }

    IEnumerator WaveAttack()
    {
        yield return new WaitForSeconds(3f);
        while (count <= 5)
        {
            float randomX = UnityEngine.Random.Range(30f, 100f);
            float RandomZ = UnityEngine.Random.Range(30f, 100f);
            Instantiate(goomba, new Vector3(randomX, 10, RandomZ), Quaternion.identity);
            count++;
            yield return new WaitForSeconds(1f);
        }
        HeavenState = HeavenBossState.Environment;
        isWave = false;
    }

    IEnumerator EnvironmentAttack()
    {
        Debug.Log("Enviro");
        if (platformCount < platforms.Length)
        {
            transform.LookAt(platformPositions[platformCount]);
            yield return new WaitForSeconds(5.0f);
            laser.SetActive(true); Debug.Log("startLaser");
            yield return new WaitForSeconds(7.0f);
            laser.SetActive(false); Debug.Log("endLaser");
            platforms[platformCount].SetActive(false);
            platformCount += 1;
        }
        isEnvironment = false;
        bulletWaveCount = 3;
        HeavenState = HeavenBossState.Laser;

    }



    /*void Spawn()
    {
        anim.enabled = false;
    }*/

    void StartAttack()
    {
        HeavenState = HeavenBossState.Laser;
    }

    public void HitByRay()
    {
        health -= 5f;
    }


}
