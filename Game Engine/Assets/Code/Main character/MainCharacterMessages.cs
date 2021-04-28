using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterMessages : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private GameObject messageNotResources = null;
    [SerializeField] private GameObject messageMainCharacterDead = null;

    [Header("References")]
    [SerializeField] private Transform messagesList = null;

    private void Start()
    {
        try
        {
            messagesList = GameObject.Find("UI").transform.Find("Messages").Find("Scroll View").Find("Viewport").Find("Messages list");
        }
        catch (UnityException e)
        {
            Debug.Log("MainCharacterMessages references not found. Disabling script. Error: " + e);
            enabled = false;
        }
    }

    private GameObject GetObjectFromType(MessageType type)
    {
        switch (type)
        {
            case MessageType.NOTRESOURCES:
                return messageNotResources;
            case MessageType.DEAD:
                return messageMainCharacterDead;
            default:
                return null;
        }
    }

    public void ShowMessage(MessageType type)
    {
        GameObject message = Instantiate(GetObjectFromType(type));
        message.transform.SetParent(messagesList, false);
    }
}

public enum MessageType
{
    NOTRESOURCES,
    DEAD
}