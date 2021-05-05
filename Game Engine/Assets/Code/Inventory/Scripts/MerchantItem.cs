using UnityEngine;

[CreateAssetMenu(fileName = "Merchant item", menuName = "Items/Merchant item", order = 0)]
public class MerchantItem : ScriptableObject
{
    [Header("Attributes")]
    [SerializeField] [Tooltip("Item that will be bought/sold")] private Item item = null;
    [SerializeField] [Tooltip("Price for selling/buying")] private int price = 0;
    [SerializeField] [Tooltip("True for merchant item, false for player item")] private bool buying = false;

    [Header("Debug")]
    [SerializeField] private int amount = 0;
    [SerializeField] private int maxAmount = 0;

    public Item Item { get => item; }
    public int Price { get => price; }
    public bool Buying { get => buying; }
    public int Amount { get => amount; set => amount = value; }
    public int MaxAmount { get => maxAmount; set => maxAmount = value; }
}