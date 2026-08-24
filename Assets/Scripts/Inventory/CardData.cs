using UnityEngine;

public enum CardType
{
    Monster,
    Spell
}

[CreateAssetMenu(fileName = "New Card", menuName = "Inventory/Card")]
public class CardData : ItemData
{
    [Header("Card")]
    public CardType cardType;
    public int attackPower;
    public int defensePower;
    public int Rarity;
}