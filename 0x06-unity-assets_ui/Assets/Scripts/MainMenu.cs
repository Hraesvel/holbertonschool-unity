using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void Options()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        Application.Quit(1);
    }

    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }
}


