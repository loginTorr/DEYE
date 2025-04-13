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
    public float followSpeed;
    public int count;
    public int platformCount;
    public int bulletWaveCount;

    public Transform PlayerPos;
    public Transform EyePos;

    public GameObject eye;
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

        if (!isEnviroment)
        {
            transform.LookAt(PlayerPos.transform);
        }

        if (isLaser == false && isWave == false && isEnvironment == false)
        {

            switch (HellState)
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
                    isWave = true;
                    StartCoroutine(WaveAttack());
                    break;

                case HeavenBossState.Envirment:
                    isEnvironment = true;on
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
            eyengelBullet = Instantiate(eyengelBulletPrefab, something.position, something.rotation);
            bulletCount -= 1;
            yield return new WaitForSeconds(0.3f);

        }

        isLaser = false;

        if (bulletWaveCount > 0)
        {
            HeavenState = HeavenBossState.Laser;
        }
        else
        {
            HeavenState = HeavenBossState.Wave;
        }
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
            yield return new WaitForSeconds(1f);
        }
        HellState = HellBossState.Envirnoment;
        isWave = false;
    }

    IEnumerator EnvironmentAttack()
    {
        Debug.Log("Enviro");
        if (platformCount >= 7)
        {
            transform.LookAt(platforms[platformCount])
            yield return new WaitForSeconds(5.0f);
            laser.SetActive(true); Debug.Log("startLaser");
            yield return new WaitForSeconds(7.0f);
            laser.SetActive(false); Debug.Log("endLaser");
            platformCount += 1;
        }
        isEnvironment = false;
        bulletWaveCount = 3;
        HellState = HellBossState.Laser;

    }



    void Spawn()
    {
        anim.enabled = false;
    }

    void StartAttack()
    {
        HeavenState = HeavenBossState.Laser;
    }

    public void HitByRay()
    {
        health -= 5f;
    }

}
