using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterMap : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject map;

    private void Update()
    {
        if (Input.GetButtonDown("Map")) ChangeState();
    }

    public void ChangeState()
    {
        if (map.activeSelf) map.SetActive(false);
        else map.SetActive(true);
    }
}
