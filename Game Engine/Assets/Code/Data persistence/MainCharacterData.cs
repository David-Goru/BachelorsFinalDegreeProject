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
    [SerializeField] private int currentHealth;

    public MainCharacterData(Vector3 position, Vector3 rotation, int currentHealth)
    {
        xPosition = position.x;
        yPosition = position.y;
        zPosition = position.z;
        yRotation = rotation.y;
        this.currentHealth = currentHealth;
    }

    public bool Load()
    {
        try
        {
            GameObject mainCharacter = GameObject.FindGameObjectWithTag("Player");
            mainCharacter.transform.position = new Vector3(xPosition, yPosition, zPosition);
            mainCharacter.transform.rotation = Quaternion.Euler(0.0f, yRotation, 0.0f);
            mainCharacter.GetComponent<MainCharacter>().CurrentHealth = currentHealth;
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