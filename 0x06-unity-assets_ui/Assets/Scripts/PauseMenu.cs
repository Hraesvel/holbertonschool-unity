using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Timer timer;
    

    private bool _isPaused;

    private void Awake()
    {
        PauseSingleton.Instancce.Menu = this.gameObject;
    }

    public bool IsPaused
    {
        get => _isPaused;
    }

    public void Pause()
    {
        Time.timeScale = 0;
        _isPaused = true;
        gameObject.SetActive(true);
    }

   

    public void Resume()
    {
        _isPaused = false;
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        PauseSingleton.Instancce.Dispose();
        SceneManager.LoadScene(0);
    }

    public void Options()
    {
        gameObject.SetActive(false);
        if (SceneManager.sceneCount < 2)
        {
            SceneManager.LoadScene("Scenes/Options", LoadSceneMode.Additive);
        }
    }
}

public sealed class PauseSingleton : IDisposable
{
    private static PauseSingleton _instancce = null;
    public static readonly object padlock = new object();

    private GameObject _menu;

    PauseSingleton()
    {
    }

    public static PauseSingleton Instancce
    {
        get
        {
            lock (padlock)
            {
                if (_instancce == null)
                    _instancce = new PauseSingleton();
                return _instancce;
            }
        }
    }


    public GameObject Menu
    {
        get => _menu;
        set => _menu = value;
    }


    public void Dispose()
    {
        _instancce = null;
    }
}