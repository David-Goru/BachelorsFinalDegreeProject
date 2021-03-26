using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IsUsingButton", menuName = "Spells/Conditions/IsUsingButton", order = 0)]
public class IsUsingButton : ICondition
{
    [SerializeField] private string buttonName = "";

    public override bool MeetsCondition()
    {        
        return Input.GetButton(buttonName);
    }
}