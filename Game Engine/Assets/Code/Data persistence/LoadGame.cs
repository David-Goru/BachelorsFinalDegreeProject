using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

public class LoadGame : MonoBehaviour
{
    [SerializeField] private List<GameObject> models;

    // Getters
    public List<GameObject> Models { get => models; }

    // Singleton
    public static LoadGame Instance;

    private void Start()
    {
        Instance = this;

        //StartCoroutine(loadGame());
    }

    private IEnumerator loadGame()
    {
        setText("game data file");
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(string.Format("{0}/The Final Spell/Saves/{1}.save", System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "Test"), FileMode.Open);
        GameData gameData = (GameData)bf.Deserialize(file);
        file.Close();
        yield return new WaitForSeconds(0.05f);

        setText("main character");
        yield return new WaitUntil(() => gameData.MainCharacterData.Load());
        yield return new WaitForSeconds(0.05f);
    }

    private void setText(string loadingData)
    {
        //loadingText.text = string.Format("Loading {0}...", loadingData);
    }
}