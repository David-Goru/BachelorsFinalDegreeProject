using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class LoadGame : MonoBehaviour
{
    [Header("Lists")]
    [SerializeField] private List<Item> items;

    // Getters
    public List<Item> Items { get => items; }

    // Singleton
    public static LoadGame Instance;

    private void Start()
    {
        Instance = this;

        if (Menu.LoadingGame) StartCoroutine(loadGame());
    }

    private IEnumerator loadGame()
    {
        setText("game data file");
        BinaryFormatter bf = new BinaryFormatter();
        string savedGamePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\The Final Spell\\Saves\\";
        FileStream file = File.Open(string.Format("{0}{1}.save", savedGamePath, Menu.GameName), FileMode.Open);
        GameData gameData = (GameData)bf.Deserialize(file);
        file.Close();
        yield return new WaitForSeconds(0.05f);

        foreach(SaveElement element in gameData.SaveElements)
        {
            setText(string.Format(element.Name));
            yield return new WaitUntil(() => element.Load());
            yield return new WaitForSeconds(0.025f);
        }
    }

    private void setText(string loadingData)
    {
        Debug.Log(string.Format("Loading {0}...", loadingData));
        //loadingText.text = string.Format("Loading {0}...", loadingData);
    }
}