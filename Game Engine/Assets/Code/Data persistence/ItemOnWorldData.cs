using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ItemOnWorldData
{
    [SerializeField] private float xPosition;
    [SerializeField] private float yPosition;
    [SerializeField] private float zPosition;
    [SerializeField] private float yRotation;
    [SerializeField] private string name;

    public ItemOnWorldData(Vector3 position, Vector3 rotation, string name)
    {
        xPosition = position.x;
        yPosition = position.y;
        zPosition = position.z;
        yRotation = rotation.y;
        this.name = name;
    }

    public bool Load()
    {
        try
        {
            Item item = LoadGame.Instance.Items.Find(x => x.name == name);
            MonoBehaviour.Instantiate(item.ItemModel, new Vector3(xPosition, yPosition, zPosition), Quaternion.Euler(0.0f, yRotation, 0.0f)).GetComponent<ItemOnWorld>().Load(item);
        }
        catch (UnityException e)
        {
            // Add to text file?
            Debug.Log("Item on world (" + name + ") data error: " + e);

            return false;
        }

        return true;
    }
}