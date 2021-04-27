using UnityEngine;

public class AddItem : IEvent
{
    [Header("Attributes")]
    [SerializeField] private Item item;
    [SerializeField] private int amount;

    public override void Run()
    {
        // Add item to inventory
        Debug.Log("Adding " + amount + "x" + item.name + " to inventory");
    }
}