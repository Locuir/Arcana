using UnityEngine;

public enum ItemType
{
    Card,
    Item
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public int ID;
    public string ItemName;
    public Sprite Icon;
    [TextArea] public string Description;

    public ItemType Type;

    public bool Stackable;
    public int MaxStack = 99;
}