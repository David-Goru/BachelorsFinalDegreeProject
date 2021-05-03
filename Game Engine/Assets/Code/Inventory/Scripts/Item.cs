using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/Item", order = 0)]
public class Item : ScriptableObject
{
    [SerializeField] private GameObject itemModel = null;
    [SerializeField] private Sprite itemIcon = null;

    public GameObject ItemModel { get => itemModel; set => itemModel = value; }
    public Sprite ItemIcon { get => itemIcon; set => itemIcon = value; }
}