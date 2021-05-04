using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HasItem", menuName = "Spells/Conditions/HasItem", order = 0)]
public class HasItem : ICondition
{
    [SerializeField] private Item item = null;
    [SerializeField] private int amount = 0;

    public override bool MeetsCondition()
    {
        return GameObject.FindGameObjectWithTag("Player").GetComponent<MainCharacterInventory>().MainCharacterItems.Exists(x => x.Item == item && x.Amount >= amount);
    }
}