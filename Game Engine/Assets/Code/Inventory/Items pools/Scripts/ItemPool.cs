using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemPool", menuName = "Items/ItemPool", order = 0)]
public class ItemPool : ScriptableObject
{
    [SerializeField] private string poolName = "";
    [SerializeField] private List<Item> items;
}