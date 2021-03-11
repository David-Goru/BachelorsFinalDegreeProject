using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

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

        // if in fight: nextSave = 5? and StartCoroutine(saveTimer()); 
        // else Save();
        save();
    }

    private void save()
    {
        // Main character
        Transform mainCharacter = GameObject.FindGameObjectWithTag("Player").transform;
        MainCharacterData mainCharacterData = new MainCharacterData(mainCharacter.position, mainCharacter.eulerAngles, mainCharacter.GetComponent<MainCharacter>().CurrentHealth);

        GameData gameData = new GameData(mainCharacterData);

        // Create file
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Format("{0}/The Final Spell/Saves/{1}.save", System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "Test")); // Test should be replaced with something else (Data creation?)
        bf.Serialize(file, gameData);
        file.Close();

        nextSave = timeBetweenSaves;
        StartCoroutine(saveTimer());
    }
}