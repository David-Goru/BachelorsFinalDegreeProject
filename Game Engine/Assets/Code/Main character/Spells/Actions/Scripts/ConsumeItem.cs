using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConsumeItem", menuName = "Spells/Actions/ConsumeItem", order = 0)]
public class ConsumeItem : IAction
{
    [SerializeField] private string itemName = "";
    [SerializeField] private int amount = 0;

    public override void DoAction()
    {
        Debug.Log("Consuming " + amount + " units of " + itemName);
    }
}