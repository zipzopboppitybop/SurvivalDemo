using System.Diagnostics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] Camera playerCamera;
    [SerializeField] float speed;
    [SerializeField] float sprintMod;
    [SerializeField] float crouchMod;
    [SerializeField] float cameraCrouchSpeed;
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpCountMax;
    [SerializeField] int gravity;

    private Vector3 moveDir;
    private Vector3 playerVel;
    private Vector3 playerOriginalCenter;
    private Vector3 playerCrouchingCenter;
    private Vector3 cameraOriginalPos;
    private Vector3 cameraCrouchingPos;

    bool isSprinting;
    bool isCrouching;

    int jumpCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerOriginalCenter = controller.center;
        playerCrouchingCenter = new Vector3(0, -.5f, 0);
        cameraOriginalPos = playerCamera.transform.localPosition;
        cameraCrouchingPos = cameraOriginalPos - new Vector3(0, 0.5f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        Sprint();

        Crouch();
    }

    void Movement()
    {
        // If the character is grounded we make sure their jump count is reset and gravity doesn't affect them
        // Otherwise apply gravity
        if (controller.isGrounded)
        {
            playerVel = Vector3.zero;
            jumpCount = 0;
        }
        else
        {
            playerVel.y -= gravity * Time.deltaTime;
        }

        moveDir = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        controller.Move(moveDir * speed * Time.deltaTime);

        Jump();
        controller.Move(playerVel * Time.deltaTime);
    }

    void Sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            isSprinting = true;
            speed *= sprintMod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            isSprinting = false;
            speed /= sprintMod;
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpCountMax)
        {
            playerVel.y = jumpSpeed;
            jumpCount++;
        }
    }

    void Crouch()
    {
        if (Input.GetButtonDown("Crouch"))
        {
            isCrouching = true;
            speed /= crouchMod;
            controller.height /= 2;
            controller.center = playerCrouchingCenter; 
            playerCamera.transform.localPosition = cameraCrouchingPos;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            isCrouching = false;
            speed *= crouchMod;
            controller.height *= 2;
            controller.center = playerOriginalCenter;
            playerCamera.transform.localPosition = cameraOriginalPos;
        }
    }
}
