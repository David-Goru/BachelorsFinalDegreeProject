using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemsOnWorldData : SaveElement
{
    [Header("Debug")]
    [SerializeField] private List<ItemOnWorldData> items;

    public override SaveElement Save()
    {
        Name = "Items";
        items = new List<ItemOnWorldData>();
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Item"))
        {
            items.Add(new ItemOnWorldData(item.name, item.transform.position, item.transform.eulerAngles));
        }
        return this;
    }

    public override bool Load()
    {
        foreach (ItemOnWorldData item in items)
        {
            item.Load();
        }
        return true;
    }
}