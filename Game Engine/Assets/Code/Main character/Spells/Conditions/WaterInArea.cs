using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaterInArea", menuName = "Spells/Conditions/WaterInArea", order = 0)]
public class WaterInArea : ICondition
{
    [SerializeField] private int waterAmount = 0;

    public override bool MeetsCondition()
    {
        Debug.Log("Checking for " + waterAmount + " units of water");
        return true;
    }
}