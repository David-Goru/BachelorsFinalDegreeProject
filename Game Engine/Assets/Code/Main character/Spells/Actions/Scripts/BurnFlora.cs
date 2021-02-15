using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BurnFlora", menuName = "Spells/Actions/BurnFlora", order = 0)]
public class BurnFlora : IAction
{
    [SerializeField] private int area = 0;

    public override void DoAction()
    {
        Debug.Log("Burning flora behind player within " + area + " meters");
    }
}