using UnityEngine;

public class AddItem : IEvent
{
    [Header("Attributes")]
    [SerializeField] private Item item;
    [SerializeField] private int amount;

    public override void Run()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<MainCharacterInventory>().AddItem(item, amount);
    }
}