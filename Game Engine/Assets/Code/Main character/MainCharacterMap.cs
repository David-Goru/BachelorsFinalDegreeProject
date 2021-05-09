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
        if (map.activeSelf) CloseMap();
        else OpenMap();
    }

    public void OpenMap()
    {
        transform.GetComponent<MainCharacter>().CharacterCamera.ChangeState(false);
        UI.Instance.UnlockMouse();
        map.SetActive(true);
    }

    public void CloseMap()
    {
        map.SetActive(false);
        transform.GetComponent<MainCharacter>().CharacterCamera.ChangeState(true);
        UI.Instance.LockMouse();
    }
}
