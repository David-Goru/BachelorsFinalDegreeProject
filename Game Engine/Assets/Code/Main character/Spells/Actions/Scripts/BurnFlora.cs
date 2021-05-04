using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "BurnFlora", menuName = "Spells/Actions/BurnFlora", order = 0)]
public class BurnFlora : IAction
{
    [SerializeField] private int area = 0;

    public override void DoAction()
    {
        foreach (Collider plant in Physics.OverlapSphere(GameObject.FindGameObjectWithTag("Player").transform.position, area, 1 << LayerMask.NameToLayer("Flora")))
        {
            plant.GetComponent<Flora>().Burn();
        }
    }
}