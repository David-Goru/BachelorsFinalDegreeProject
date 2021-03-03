using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/Item", order = 0)]
public class Item : ScriptableObject
{
    [SerializeField] private GameObject itemModel = null;

    public GameObject ItemModel { get => itemModel; set => itemModel = value; }
}