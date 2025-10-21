using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Pickup : MonoBehaviour
{
    public ItemData item;
    public TMP_Text nameText;

    private void Start()
    {
        nameText.text = item.name;
        nameText.alpha = 0f;
    }

   private void PickUpItem()
    {
        Destroy(gameObject);  
    }

}
