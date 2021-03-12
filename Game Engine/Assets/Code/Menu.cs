using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public static bool LoadingGame = false;
    public static string GameName = "";

    public void NewGame()
    {
        LoadingGame = false;
        GameName = "Game (" + System.DateTime.Now.ToString() + ")";

        startGame();
    }

    public void LoadGame()
    {
        LoadingGame = true;
        //GameName = name from button;

        startGame();
    }

    private void startGame()
    {
        SceneManager.LoadScene("Game");
    }
}