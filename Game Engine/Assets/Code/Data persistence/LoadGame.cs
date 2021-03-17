using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class LoadGame : MonoBehaviour
{
    [SerializeField] private List<Item> items;

    // Getters
    public List<Item> Items { get => items; }

    // Singleton
    public static LoadGame Instance;

    private void Start()
    {
        if (!Menu.LoadingGame) return;

        Instance = this;
        StartCoroutine(loadGame());
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

        setText("main character");
        yield return new WaitUntil(() => gameData.MainCharacterData.Load());
        yield return new WaitForSeconds(0.05f);

        for (int i = 0; i < gameData.ItemsOnWorldData.Count; i++)
        {
            setText(string.Format("items on floor ({0}/{1})", i, gameData.ItemsOnWorldData.Count));
            yield return new WaitUntil(() => gameData.ItemsOnWorldData[i].Load());
            yield return new WaitForSeconds(0.025f);
        }
    }

    private void setText(string loadingData)
    {
        //loadingText.text = string.Format("Loading {0}...", loadingData);
    }
}