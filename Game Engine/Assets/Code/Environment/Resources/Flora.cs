using System.Collections;
using UnityEngine;

public class Flora : MonoBehaviour
{
    public void Burn()
    {
        gameObject.layer = 2;
        transform.Find("Burn effect").gameObject.SetActive(true);
        StartCoroutine(destroyFlora(Random.Range(0.75f, 1.25f)));
    }

    private IEnumerator destroyFlora(float timeToDestroy)
    {
        yield return new WaitForSeconds(timeToDestroy);
        Destroy(gameObject);
    }
}