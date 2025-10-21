using UnityEngine;
using TMPro;

public class FollowPlayerText : MonoBehaviour
{
    [SerializeField] bool faceCamera;

    void LateUpdate()
    {
        if (faceCamera)
        {
            transform.LookAt(Camera.main.transform);
            transform.Rotate(0, 180f, 0);
        }
            
    }
}
