using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public Animator anim;

    public bool KeyPiece1;
    public float moveSpeed = 15f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float doubleJumpHeight = 2f;
    public Transform cam;
    public float mouseSensitivity = 100f;
    public float playerHealth = 100f;
    public float range = 100f;
    public LayerMask hitMask;
    public TextMeshProUGUI txtHealth;
    public Transform[] hitScanOrigins;
    public ParticleSystem muzzleFlash;
    public GameObject hitIndicate;
    public bool hasHealed = false;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool DoubleJumpReady;
    private bool gunCanShoot = true;
    private float xRotation = 0f;

    public Material HeavenBox;
    public GameObject HeavenRoom;
    public GameObject HellRoom;

    // Start is called before the first frame update
    void Start()
    {
        KeyPiece1 = false;
        CameraFade.fadeInstance.FadeIn();
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //Invoke("KillPlayer", 3f);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth > 0)
        {
            handleMouse();
            handleMovement();
            Shoot();
        }

        if (playerHealth <= 0)
        {
            playerHealth = 0;
            Die();
        }

        txtHealth.text = playerHealth.ToString("000");
    }

    void handleMouse()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void handleMovement()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded)
            DoubleJumpReady = true;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = (cam.forward * v + cam.right * h).normalized;
        move.y = 0;

        controller.Move(move * moveSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isGrounded && DoubleJumpReady && KeyPiece1)
        {
            moveSpeed = 8f;

            velocity.y = Mathf.Sqrt(doubleJumpHeight * -2f * gravity);
            DoubleJumpReady = false;
        }

        if (!isGrounded && DoubleJumpReady)
        {
            moveSpeed = 12f;
        }

        if (isGrounded)
        {
            moveSpeed = 15f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && gunCanShoot)
        {
            anim.SetTrigger("Fire");
            int ShootClip = Random.Range(1, 5);
            switch (ShootClip)
            {
                case 1:
                    SoundManager.Play(SoundType.SHOTGUN_PUNCH_1);
                    break;
                case 2:
                    SoundManager.Play(SoundType.SHOTGUN_PUNCH_2);
                    break;
                case 3:
                    SoundManager.Play(SoundType.SHOTGUN_PUNCH_3);
                    break;
                case 4:
                    SoundManager.Play(SoundType.SHOTGUN_PUNCH_4);
                    break;
            }

            foreach (Transform hitScanOrigin in hitScanOrigins)
            {
                Ray ray = new Ray(hitScanOrigin.transform.position, hitScanOrigin.transform.forward);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, range, hitMask))
                {
                    GoonMain goon = hit.collider.GetComponent<GoonMain>();
                    if (goon != null)
                    {
                        goon.HitByRay();
                        hitIndicate.SetActive(true);
                        Invoke("deactiveHitIndicate", 0.2f);
                    }
                    HellBoss_EyeStalk hellBoss = hit.collider.GetComponent<HellBoss_EyeStalk>();
                    if (hellBoss != null)
                    {
                        hellBoss.HitByRay();
                        hitIndicate.SetActive(true);
                        Invoke("deactiveHitIndicate", 0.2f);
                    }
                    HeavenBoss_Eyengel heavenBoss = hit.collider.GetComponent<HeavenBoss_Eyengel>();
                    if (heavenBoss != null)
                    {
                        heavenBoss.HitByRay();
                        hitIndicate.SetActive(true);
                        Invoke("deactiveHitIndicate", 0.2f);
                    }
                }
            }
            muzzleFlash.Play();
            gunCanShoot = false;
            Invoke("GunTimer", 0.5f);
        }
    }

    void GunTimer()
    {
        int loadClip = Random.Range(1, 5);
        switch (loadClip)
        {
            case 1:
                SoundManager.Play(SoundType.SHOTGUN_LOAD_1);
                break;
            case 2:
                SoundManager.Play(SoundType.SHOTGUN_LOAD_2);
                break;
            case 3:
                SoundManager.Play(SoundType.SHOTGUN_LOAD_3);
                break;
            case 4:
                SoundManager.Play(SoundType.SHOTGUN_LOAD_4);
                break;
        }

        gunCanShoot = true;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("GoonBeam"))
        {
            Destroy(collision.gameObject);
            playerHealth -= 5f;
        }

        if (collision.CompareTag("HeavenBeam"))
        {
            Destroy(collision.gameObject);
            playerHealth -= 10f;
        }

        if (collision.CompareTag("Laser"))
        {
            playerHealth -= 15f;
        }

        if (collision.CompareTag("Lava"))
        {
            playerHealth -= 10f;

        }

        if (collision.CompareTag("Tendril"))
        {
            playerHealth -= 5f;
        }

        if (collision.CompareTag("LevelTrans"))
        {
            //set PlayerPos to First Platform
            //fade in - fade out
            CameraFade.fadeInstance.FadeOut();
            StartCoroutine("SwitchRoom");
            HellRoom.SetActive(false);
            HeavenRoom.SetActive(true);
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("HealFountain") && !hasHealed)
        {
            playerHealth += 50f;
            if (playerHealth > 100f)
            {
                playerHealth = 100f;
            }
            hasHealed = true;
        }

        if (collision.CompareTag("InstaDeath"))
        {
            KillPlayer();
        }
    }


    void Die()
    {
        cam.localRotation = Quaternion.Euler(0f, 0f, 90f);
        cam.localPosition = new Vector3(0f, -0.5f, 0f);
        CameraFade.fadeInstance.FadeOut();
        Invoke("LoadGameOver", 2f);
    }

    void KillPlayer()
    {
        playerHealth -= 100f;
    }

    void deactiveHitIndicate()
    {
        hitIndicate.SetActive(false);
    }

    public void LoadGameOver()
    {
        SceneManager.LoadScene("GameOverScene");
    }

    public void LoadWin()
    {
        CameraFade.fadeInstance.FadeOut();
        Invoke("LoadWinScene", 2f);
    }

    public void LoadWinScene()
    {
        SceneManager.LoadScene("WinScene");
    }

    IEnumerator SwitchRoom()
    {
        //teleport player
        yield return new WaitForSeconds(4f);
        RenderSettings.skybox = HeavenBox;
        HeavenRoom.SetActive(true);
        HellRoom.SetActive(false);
        yield return new WaitForSeconds(2f);
        CameraFade.fadeInstance.FadeIn();


    }

}
