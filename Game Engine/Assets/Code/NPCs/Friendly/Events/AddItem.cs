using UnityEngine;

public class AddItem : IEvent
{
    [Header("Attributes")]
    [SerializeField] private Item item;

    public override void Run()
    {
        // Add item to inventory
        Debug.Log("Adding " + item.name + " to inventory");
    }
}