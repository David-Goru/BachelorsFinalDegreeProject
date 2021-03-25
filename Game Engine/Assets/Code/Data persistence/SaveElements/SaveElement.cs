using UnityEngine;

[System.Serializable]
public abstract class SaveElement
{
    public string Name;
    public abstract SaveElement Save();
    public abstract bool Load();
}