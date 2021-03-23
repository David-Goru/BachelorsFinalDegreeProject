using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    private MainCharacterData mainCharacterData;
    private List<ItemOnWorldData> itemsOnWorldData;
    private List<AchievementData> achievementsData;

    // Getters
    public MainCharacterData MainCharacterData { get => mainCharacterData; }
    public List<ItemOnWorldData> ItemsOnWorldData { get => itemsOnWorldData; }
    public List<AchievementData> AchievementsData { get => achievementsData; }

    public GameData(MainCharacterData mainCharacterData, List<ItemOnWorldData> itemsOnWorldData, List<AchievementData> achievementsData)
    {
        this.mainCharacterData = mainCharacterData;
        this.itemsOnWorldData = itemsOnWorldData;
        this.achievementsData = achievementsData;
    }
}