using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryData : SaveElement
{
    [Header("Debug")]
    [SerializeField] private List<MainCharacterItem> characterItems;

    public override SaveElement Save()
    {
        Name = "InventoryData";
        characterItems = GameObject.FindGameObjectWithTag("Player").GetComponent<MainCharacterInventory>().MainCharacterItems;
        return this;
    }

    public override bool Load()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<MainCharacterInventory>().MainCharacterItems = characterItems;
        return true;
    }
}