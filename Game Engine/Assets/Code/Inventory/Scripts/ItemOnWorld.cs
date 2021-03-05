using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnWorld : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private Item item = null;

    public void Initialize(Item item)
    {
        this.item = item;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("PlayerItemGatherArea"))
        {
            // Add to inventory
            if (PlayerAndEnemiesPlaytesting.Instance != null) PlayerAndEnemiesPlaytesting.Instance.UpdateStat(item.name, 1);

            Destroy(gameObject);
        }
    }
}