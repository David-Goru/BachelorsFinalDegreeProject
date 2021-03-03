using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemPool", menuName = "Items/ItemPool", order = 0)]
public class ItemPool : ScriptableObject
{
    [SerializeField] private List<Item> items;

    // Getters
    public List<Item> Items { get => items; }
}