using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class SaveGame : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int timeBetweenSaves = 0;

    [Header("Debug")]
    [SerializeField] private int nextSave = 0;

    private void Start()
    {
        nextSave = timeBetweenSaves;
        StartCoroutine(saveTimer());
    }

    private IEnumerator saveTimer()
    {
        yield return new WaitForSeconds(nextSave);

        bool inFight = false;

        if (GameObject.FindGameObjectWithTag("Player").GetComponent<MainCharacter>().IsInFight()) inFight = true;
        else
        {
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                if (enemy.GetComponent<Enemy>().IsOnFight())
                {
                    inFight = true;
                    break;
                }
            }
        }

        if (inFight)
        {
            nextSave = 5;
            StartCoroutine(saveTimer());
        }
        else save();
    }

    private void save()
    {
        string savedGamesPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\The Final Spell\\Saves\\";
        if (Menu.GameName == "Null") Menu.GameName = string.Format("Game ({0})", DateTime.Now).Replace("/", "-").Replace(":", ".");

        // Create path if doesn't exist
        if (!Directory.Exists(savedGamesPath)) Directory.CreateDirectory(savedGamesPath);

        // Main character
        Transform mainCharacter = GameObject.FindGameObjectWithTag("Player").transform;
        MainCharacterData mainCharacterData = new MainCharacterData(mainCharacter.position, mainCharacter.eulerAngles, mainCharacter.GetComponent<MainCharacter>().CurrentHealth);

        // Items on world
        List<ItemOnWorldData> itemsOnWorldData = new List<ItemOnWorldData>();
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Item"))
        {
            itemsOnWorldData.Add(new ItemOnWorldData(item.transform.position, item.transform.eulerAngles, item.GetComponent<ItemOnWorld>().ItemName));
        }

        GameData gameData = new GameData(mainCharacterData, itemsOnWorldData);

        // Create file
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Format("{0}{1}.save", savedGamesPath, Menu.GameName));
        bf.Serialize(file, gameData);
        file.Close();

        nextSave = timeBetweenSaves;
        StartCoroutine(saveTimer());
    }
}