[System.Serializable]
public class GameData
{
    private MainCharacterData mainCharacterData;

    // Getters
    public MainCharacterData MainCharacterData { get => mainCharacterData; }

    public GameData(MainCharacterData mainCharacterData)
    {
        this.mainCharacterData = mainCharacterData;
    }
}