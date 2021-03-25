using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    private List<SaveElement> saveElements;

    // Getters
    public List<SaveElement> SaveElements { get => saveElements; set => saveElements = value; }

    public GameData()
    {
        saveElements = new List<SaveElement>();
    }
    public GameData(List<SaveElement> saveElements)
    {
        this.saveElements = saveElements;
    }
}