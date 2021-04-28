using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageTimer : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float timeToDestroy = 0.0f;

    private void Start()
    {
        StartCoroutine(startTimer());
    }

    private IEnumerator startTimer()
    {
        yield return new WaitForSeconds(timeToDestroy);
        Destroy(gameObject);
    }
}