using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 15f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;
    public Transform cam;
    public float mouseSensitivity = 100f;
    public GameObject bulletPrefab;
    public Transform bulletSpawn1;
    public Transform bulletSpawn2;
    public float playerHealth = 100f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool DoubleJumpReady;
    private bool gunCanShoot = true;
    private float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
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
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            DoubleJumpReady = false;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && gunCanShoot)
        {
            GameObject bullet1 = Instantiate(bulletPrefab, bulletSpawn1.position, bulletSpawn1.rotation);
            GameObject bullet2 = Instantiate(bulletPrefab, bulletSpawn2.position, bulletSpawn2.rotation);
            gunCanShoot = false;
            Invoke("GunTimer", 0.3f);
        }
    }

    void GunTimer()
    {
        gunCanShoot = true;
    }

    /*void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Beam"))
        {
            playerHealth -= 5f;
        }
    }*/

    void Die() {
        cam.localRotation = Quaternion.Euler(0f, 0f, 90f);
        cam.localPosition = new Vector3(0f, -0.5f, 0f);
        Invoke("LoadGameOver", 2f);
    }

    void KillPlayer() {
        playerHealth -= 100f;
    }

    void LoadGameOver() {
        SceneManager.LoadScene("GameOverScene");    
    }
}
