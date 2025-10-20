using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] int sens;
    [SerializeField] int lockVertMin, lockVertMax;
    [SerializeField] bool invertY;

    float rotX;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Get input
        float mouseX = Input.GetAxisRaw("Mouse X") * sens * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sens * Time.deltaTime;
        // Use invertY
        if (invertY)
            rotX += mouseY;
        else
            rotX -= mouseY;

        // Clamp camera on x axis
        rotX = Mathf.Clamp(rotX, lockVertMin, lockVertMax);
        // Rotate camera on x axis
        transform.localRotation = Quaternion.Euler(rotX, 0, 0);
        // Rotate the player on y axis
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}