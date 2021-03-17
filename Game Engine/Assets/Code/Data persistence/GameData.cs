using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    private MainCharacterData mainCharacterData;
    private List<ItemOnWorldData> itemsOnWorldData;

    // Getters
    public MainCharacterData MainCharacterData { get => mainCharacterData; }
    public List<ItemOnWorldData> ItemsOnWorldData { get => itemsOnWorldData; }

    public GameData(MainCharacterData mainCharacterData, List<ItemOnWorldData> itemsOnWorldData)
    {
        this.mainCharacterData = mainCharacterData;
        this.itemsOnWorldData = itemsOnWorldData;
    }
}