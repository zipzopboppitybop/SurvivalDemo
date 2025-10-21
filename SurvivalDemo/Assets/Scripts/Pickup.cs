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

    //private void OnTriggerEnter(Collider other)
    //{
    //    IPickup pickup = other.GetComponent<IPickup>();

    //    if (pickup != null)
    //    {
    //        pickup.GetItemData(item);
    //        Destroy(gameObject);
    //    }
    //}
}
