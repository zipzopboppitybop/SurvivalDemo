using UnityEngine;

public class Pickup : MonoBehaviour
{
    public ItemData item;
    private void OnTriggerEnter(Collider other)
    {
        IPickup pickup = other.GetComponent<IPickup>();

        if (pickup != null)
        {
            pickup.GetItemData(item);
            Destroy(gameObject);
        }
    }
}
