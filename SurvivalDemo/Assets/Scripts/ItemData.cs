using UnityEngine;

[CreateAssetMenu]

public class ItemData : ScriptableObject
{
    public GameObject model;

    public string itemName;
    public bool canEat;
    public int foodAmount;
    public int damage;
}
