using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnWorld : MonoBehaviour, IEntity
{
    [Header("Debug")]
    [SerializeField] private Item item = null;
    [SerializeField] private bool ignoreCollisions = false;

    public void Spawn(Item item)
    {
        GetComponent<Rigidbody>().AddForce(Vector3.right * Random.Range(-5f, 5f) + Vector3.forward * Random.Range(-5f, 5f), ForceMode.Impulse);
        Initialize(item);
    }

    public void Initialize(Item item)
    {
        this.item = item;
        gameObject.name = item.name;
        ignoreCollisions = false;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!ignoreCollisions && col.gameObject.CompareTag("PlayerItemGatherArea")) StartCoroutine(addToInventory(col.transform));
    }

    private IEnumerator addToInventory(Transform character)
    {
        ignoreCollisions = true;
        yield return new WaitUntil(() => magnetToPlayer(character.position));

        character.parent.GetComponent<MainCharacterInventory>().AddItem(item, 1);
        Destroy(gameObject);
    }

    private bool magnetToPlayer(Vector3 playerPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, playerPosition, Time.deltaTime * 10.0f);        

        return Vector3.Distance(transform.position, playerPosition) < 1;
    }

    public void ReceiveDamage(int damageAmount)
    {
        // Nothing
    }
}