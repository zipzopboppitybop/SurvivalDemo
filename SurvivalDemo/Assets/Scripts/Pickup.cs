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
    }

   private void PickUpItem()
    {
        Destroy(gameObject);  
    }

}
