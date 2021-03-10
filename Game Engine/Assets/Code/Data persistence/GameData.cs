using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    private MainCharacterData mainCharacterData;
    private List<EnemyData> enemyDataList;

    // Getters
    public MainCharacterData MainCharacterData { get => mainCharacterData; }
    public List<EnemyData> EnemyDataList { get => enemyDataList; }

    public GameData(MainCharacterData mainCharacterData, List<EnemyData> enemyDataList)
    {
        this.mainCharacterData = mainCharacterData;
        this.enemyDataList = enemyDataList;
    }
}