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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Add to inventory

            Destroy(gameObject);
        }
    }
}