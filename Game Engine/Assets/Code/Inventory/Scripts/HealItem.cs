using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealItem", menuName = "Items/HealItem", order = 0)]
public class HealItem : Item
{
    [SerializeField] private int healAmount = 0;

    public int HealAmount { get => healAmount; }
}