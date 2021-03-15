using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using System.IO;

public class Menu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform savedGamesList = null;
    [SerializeField] private GameObject savedGameButtonPrefab = null;

    public static bool LoadingGame = false;
    public static string GameName = "Null";

    private void Start()
    {
        if (savedGamesList == null || savedGameButtonPrefab == null)
        {
            Debug.Log("Menu references not found. Disabling script.");
            enabled = false;
        }

        GetGames();
    }

    public void NewGame()
    {
        LoadingGame = false;
        GameName = "Null";

        startGame();
    }

    public void GetGames()
    {
        DirectoryInfo saves = new DirectoryInfo(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/The Final Spell/Saves");
        FileInfo[] savedGames = saves.GetFiles();

        foreach (FileInfo savedGame in savedGames)
        {
            Transform savedGameButton = Instantiate(savedGameButtonPrefab).transform;
            savedGameButton.Find("Name").GetComponent<Text>().text = Path.GetFileNameWithoutExtension(savedGame.Name);
            savedGameButton.GetComponent<Button>().onClick.AddListener(delegate () { LoadGame(savedGame); });

            savedGameButton.SetParent(savedGamesList);
        }
    }

    public void LoadGame(FileInfo game)
    {
        LoadingGame = true;
        GameName = Path.GetFileNameWithoutExtension(game.Name);

        startGame();
    }

    private void startGame()
    {
        SceneManager.LoadScene("Game");
    }
}