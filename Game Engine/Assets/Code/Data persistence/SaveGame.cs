using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

public class SaveGame : MonoBehaviour
{
    public static void Save()
    {
        // Main character
        Transform mainCharacter = GameObject.FindGameObjectWithTag("Player").transform;
        MainCharacterData mainCharacterData = new MainCharacterData(mainCharacter.position, mainCharacter.eulerAngles);

        // Enemies
        List<EnemyData> enemyDataList = new List<EnemyData>();
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemyDataList.Add(new EnemyData(enemy.name, enemy.transform.position, enemy.transform.eulerAngles));
        }

        GameData gameData = new GameData(mainCharacterData, enemyDataList);

        // Create file
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Format("{0}/The Final Spell/Saves/{1}.save", System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "Test")); // Test should be replaced with something else (Data creation?)
        bf.Serialize(file, gameData);
        file.Close();
    }
}