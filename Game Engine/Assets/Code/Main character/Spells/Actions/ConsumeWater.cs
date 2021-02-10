using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConsumeWater", menuName = "Spells/Actions/ConsumeWater", order = 0)]
public class ConsumeWater : IAction
{
    [SerializeField] private int waterAmount = 0;

    public override void DoAction()
    {
        Debug.Log("Consuming " + waterAmount + " units of water");
    }
}