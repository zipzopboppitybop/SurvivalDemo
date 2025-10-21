using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance;

    [SerializeField] GameObject player;
    [SerializeField] PlayerController playerController;

    public Image playerHealth;
    public Image playerStamina;
    public GameObject playerHealthBar;
    public GameObject playerStaminaBar;
    public bool showStamina;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;

        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        ShowStamina();
    }

    void ShowStamina()
    {
        playerStaminaBar.gameObject.SetActive(showStamina);
    }
}
