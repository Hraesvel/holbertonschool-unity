using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// Method to handle Options buttons functionality
    /// </summary>
    public void Options()
    {
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Method to handle Exit button functionality
    /// </summary>
    public void Exit()
    {
        Application.Quit(1);
    }

    /// <summary>
    /// Method to handle level buttons functionality
    /// </summary>
    public void LoadLevel(string name)
    {
        foreach (var bgm in  GameObject.FindGameObjectsWithTag("BGM"))
            Destroy(bgm);
        SceneManager.LoadScene(name);
    }
}


