using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class EnemyData
{
    [SerializeField] private string name;
    [SerializeField] private float xPosition;
    [SerializeField] private float yPosition;
    [SerializeField] private float zPosition;
    [SerializeField] private float yRotation;

    public EnemyData(string name, Vector3 position, Vector3 rotation)
    {
        this.name = name;
        xPosition = position.x;
        yPosition = position.y;
        zPosition = position.z;
        yRotation = rotation.y;
    }

    public bool Load()
    {
        try
        {
            GameObject enemy = MonoBehaviour.Instantiate(LoadGame.Instance.Models.Find(x => x.name == this.name), new Vector3(xPosition, yPosition, zPosition), Quaternion.Euler(0.0f, yRotation, 0.0f));
        }
        catch (Exception e)
        {
            // Add to text file?
            Debug.Log("Enemy data error: " + e);

            return false;
        }

        return true;
    }
}