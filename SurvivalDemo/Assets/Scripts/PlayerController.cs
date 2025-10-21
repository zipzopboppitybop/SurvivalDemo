using System.Diagnostics;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;

public class PlayerController : MonoBehaviour, IPickup
{
    [SerializeField] CharacterController controller;
    [SerializeField] Camera playerCamera;
    [SerializeField] float health;
    [SerializeField] float stamina;
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
    private Coroutine staminaRechargeCoroutine;
    private Pickup lastLookedAtItem;

    bool isSprinting;
    bool isCrouching;
    bool isRecharging;
    bool isJumping;

    int jumpCount;
    float healthOrig;
    float staminaOrig;
    float baseSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthOrig = health;
        staminaOrig = stamina;
        baseSpeed = speed;
        playerOriginalCenter = controller.center;
        playerCrouchingCenter = new Vector3(0, -.5f, 0);
        cameraOriginalPos = playerCamera.transform.localPosition;
        cameraCrouchingPos = cameraOriginalPos - new Vector3(0, 0.5f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        HandeItemLook();

        Movement();

        Sprint();

        Crouch();

        UpdateCameraPosition();
    }

    void HandeItemLook()
    {
        RaycastHit hit;
        UnityEngine.Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 20f, Color.red);

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 20f))
        {
            Pickup item = hit.collider.GetComponent<Pickup>();

            if (item != null)
            {
                if (item != lastLookedAtItem)
                {
                    if (lastLookedAtItem != null)
                        lastLookedAtItem.nameText.gameObject.SetActive(false);

                    item.nameText.gameObject.SetActive(true);
                    GetItemData(item.item);

                    lastLookedAtItem = item;
                }

                if (Input.GetButtonDown("Interact"))
                {
                    Destroy(item.gameObject);
                }
            }
            else
            {
                if (lastLookedAtItem != null)
                {
                    lastLookedAtItem.nameText.gameObject.SetActive(false);
                    lastLookedAtItem = null;
                }
            }
        }
        else
        {
            if (lastLookedAtItem != null)
            {
                lastLookedAtItem.nameText.gameObject.SetActive(false);
                lastLookedAtItem = null;
            }
        }
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
            isJumping = false;
        }

        moveDir = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        controller.Move(moveDir * speed * Time.deltaTime);

        Jump();
        controller.Move(playerVel * Time.deltaTime);
    }

    void Sprint()
    {
        if (Input.GetButton("Sprint") && stamina > 0)
        {
            isSprinting = true;
            stamina -= 20f * Time.deltaTime;

            if (staminaRechargeCoroutine != null)
            {
                StopCoroutine(staminaRechargeCoroutine);
                staminaRechargeCoroutine = null;
            }

            isRecharging = false;
        }
        else
        {
            isSprinting = false;

            if (!isRecharging && stamina < staminaOrig)
            {
                staminaRechargeCoroutine = StartCoroutine(RechargeStamina());
            }
        }

        speed = baseSpeed;
        if (isSprinting) speed *= sprintMod;
        if (isCrouching) speed /= crouchMod;
        UpdatePlayerUI();
    }

    IEnumerator RechargeStamina()
    {
        isRecharging = true;

        float waitTime = stamina <= 0 ? 4f : 2f;
        yield return new WaitForSeconds(waitTime);
        while (stamina < staminaOrig)
        {
            if (isSprinting || isJumping)
            {
                isRecharging = false;
                staminaRechargeCoroutine = null;
                yield break;
            }

            stamina += 25f * Time.deltaTime;
            UpdatePlayerUI();
            yield return null;
        }

        isRecharging = false;
        stamina = staminaOrig;
        staminaRechargeCoroutine = null;
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpCountMax && stamina > 9)
        {
            playerVel.y = jumpSpeed;
            jumpCount++;
            stamina -= 10f;
            isJumping = true;

            if (staminaRechargeCoroutine != null)
            {
                StopCoroutine(staminaRechargeCoroutine);
                staminaRechargeCoroutine = null;
            }

            isRecharging = false;
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
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            isCrouching = false;
            speed *= crouchMod;
            controller.height *= 2;
            controller.center = playerOriginalCenter;
        }
    }

    void UpdateCameraPosition()
    {
        Vector3 targetPos = isCrouching ? cameraCrouchingPos : cameraOriginalPos;
        playerCamera.transform.localPosition = Vector3.Lerp(
            playerCamera.transform.localPosition,
            targetPos,
            Time.deltaTime * cameraCrouchSpeed
        );
    }

    void UpdatePlayerUI()
    {
        Gamemanager gamemanager = Gamemanager.instance;
        gamemanager.playerHealth.fillAmount = (float)health / healthOrig;
        gamemanager.playerStamina.fillAmount = (float)stamina / staminaOrig;

        if (isSprinting || isJumping || isRecharging)
        {
            gamemanager.showStamina = true;
        }
        else
        {
            gamemanager.showStamina = false;
        }
    }

    public void GetItemData(ItemData item)
    {
        UnityEngine.Debug.Log(item.name);
    }
}
