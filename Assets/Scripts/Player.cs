using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public float moveSpeed = 15f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float doubleJumpHeight = 2f;
    public Transform cam;
    public float mouseSensitivity = 100f;
    public float playerHealth = 100f;
    public float range = 100f;
    public LayerMask hitMask;
    public float radius = 0.75f;
    public TextMeshProUGUI txtHealth;
    public GameObject fireIndicator;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool DoubleJumpReady;
    private bool gunCanShoot = true;
    private float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
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
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
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

        if (Input.GetKeyDown(KeyCode.Space) && !isGrounded && DoubleJumpReady)
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

            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            RaycastHit hit;

            if (Physics.SphereCast(ray, radius, out hit, range, hitMask))
            {
                GoonMain goon = hit.collider.GetComponent<GoonMain>();
                if (goon != null)
                {
                    goon.HitByRay();
                }
                HellBossStalkShot hellBossStalk = hit.collider.GetComponent<HellBossStalkShot>();
                if (hellBossStalk != null)
                {
                    hellBossStalk.HitByRay();
                }
                HellBossEyeShot hellBossEye = hit.collider.GetComponent<HellBossEyeShot>();
                if (hellBossEye != null)
                {
                    hellBossEye.HitByRay();
                }

            }

            fireIndicator.SetActive(false);
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
        fireIndicator.SetActive(true);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("GoonBeam"))
        {
            Destroy(collision.gameObject);
            playerHealth -= 5f;
        }

        if (collision.CompareTag("Laser"))
        {
            playerHealth -= 15;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Laser"))
        {
            playerHealth -= 5;
        }
    }

    void Die() {
        cam.localRotation = Quaternion.Euler(0f, 0f, 90f);
        cam.localPosition = new Vector3(0f, -0.5f, 0f);
        CameraFade.fadeInstance.FadeOut();
        Invoke("LoadGameOver", 2f);
    }

    void KillPlayer() {
        playerHealth -= 100f;
    }

    public void LoadGameOver() {
        SceneManager.LoadScene("GameOverScene");    
    }
}
