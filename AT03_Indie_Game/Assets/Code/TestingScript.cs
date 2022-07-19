using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestingScript : MonoBehaviour
{
    public GameObject helpMenu;
    public void DebugMessage(string message)
    {
        Debug.Log(message);
    }

    public int gameScene = 0;
    public int mainMenu = 0;
    public void StartGame()
    {
        Debug.Log("Starting Game");
        SceneManager.LoadScene(gameScene);
    }
    public void Help()
    {
        if(helpMenu.activeSelf == true)
        {
            helpMenu.SetActive(false);
        }
        else
        {
            helpMenu.SetActive(true);
        }
    }
    public void Quit()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
    public void MainMenu()
    {
        Debug.Log("Retuning to Main Menu");
        SceneManager.LoadScene(mainMenu);
    }
    public void Retry()
    {
        Debug.Log("Reset");
        SceneManager.LoadScene(gameScene);
    }
}