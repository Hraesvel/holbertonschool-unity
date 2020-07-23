using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Timer timer;
    [SerializeField] private AudioMixer audioMixer;
    private AudioMixerSnapshot[] Snaps;


    private bool _isPaused;

    private void Awake()
    {
        PauseSingleton.Instance.Menu = this.gameObject;
        PauseSingleton.Instance.Camera = FindObjectOfType<CameraController>();
        PauseSingleton.Instance.EventSystem = EventSystem.current;
        Snaps = new AudioMixerSnapshot[]
        {
            audioMixer.FindSnapshot("normal"),
            audioMixer.FindSnapshot("paused")
        };
    }


    /// <summary>
    /// Property to check if pause menu is toggled
    /// </summary>
    public bool IsPaused
    {
        get => _isPaused;
    }

    /// <summary>
    /// method that handle game pause and menu toggle on
    /// </summary>
    public void Pause()
    {
        Time.timeScale = 0;
        _isPaused = true;
        gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(transform.Find("ResumeButton").gameObject);
        audioMixer.TransitionToSnapshots(
            Snaps,
            new[] {0f, 1f},
            0);
    }


    /// <summary>
    /// method that handles resuming the game and menu toggle off
    /// </summary>
    public void Resume()
    {
        _isPaused = false;
        gameObject.SetActive(false);
        Time.timeScale = 1;
        audioMixer.TransitionToSnapshots(
            Snaps,
            new[] {1f, 0f},
            0);
    }

    /// <summary>
    /// Method that will restart the current level
    /// </summary>
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Method that will return player to the main menu
    /// </summary>
    public void MainMenu()
    {
        Time.timeScale = 1;
        PauseSingleton.Instance.Dispose();
        SceneManager.LoadScene(0);
    }


    /// <summary>
    /// Method that will open the Options menu
    /// </summary>
    public void Options()
    {
        gameObject.SetActive(false);
        if (SceneManager.sceneCount < 2)
        {
            SceneManager.LoadScene("Scenes/Options", LoadSceneMode.Additive);
        }
    }
}


/// <summary>
/// class used to have a Singleton instance of Pause so that sub-menus (Options) can be used
/// as a sub-scene
/// </summary>
public sealed class PauseSingleton : IDisposable
{
    private static PauseSingleton _instancce = null;

    /// <summary>
    /// Field for handles Mutex locking.
    /// </summary>
    public static readonly object padlock = new object();

    private GameObject _menu;

    PauseSingleton()
    {
    }

    /// <summary>
    /// Property that is used to get the PauseSingleton
    /// if an instance doesn't exist a new instance will be create and
    /// this instance will be returned until `Disposed()`
    /// </summary>
    public static PauseSingleton Instance
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

    public static bool Active
    {
        get => _instancce != null;
    }


    /// <summary>
    /// Property use to get/set parent Menu
    /// </summary>
    public GameObject Menu
    {
        get => _menu;
        set => _menu = value;
    }

    /// <summary>
    /// Property use to Get/Set main CameraController 
    /// </summary>
    public CameraController Camera { set; get; }

    /// <summary>
    /// Property use to Get/Set main EventSystem
    /// </summary>
    public EventSystem EventSystem { get; set; }


    /// <summary>
    /// Method to dispose PauseSingleton instance.
    /// </summary>
    public void Dispose()
    {
        _instancce = null;
    }
}