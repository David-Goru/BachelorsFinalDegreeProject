using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class MainCharacterData
{
    [SerializeField] private float xPosition;
    [SerializeField] private float yPosition;
    [SerializeField] private float zPosition;
    [SerializeField] private float yRotation;

    public MainCharacterData(Vector3 position, Vector3 rotation)
    {
        xPosition = position.x;
        yPosition = position.y;
        zPosition = position.z;
        yRotation = rotation.y;
    }

    public bool Load()
    {
        try
        {
            GameObject enemy = MonoBehaviour.Instantiate(LoadGame.Instance.Models.Find(x => x.name == "Main character"), new Vector3(xPosition, yPosition, zPosition), Quaternion.Euler(0.0f, yRotation, 0.0f));
        }
        catch (Exception e)
        {
            // Add to text file?
            Debug.Log("Main character data error: " + e);

            return false;
        }

        return true;
    }
}