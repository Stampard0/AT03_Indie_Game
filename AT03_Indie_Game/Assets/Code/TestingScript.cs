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

    public int sceneIndex = 0;
    public void StartGame()
    {
        SceneManager.LoadScene(sceneIndex);
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
}