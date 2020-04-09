using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class AudioController : MonoBehaviour
{
    [Tooltip("Set the default volume of the background music if player's pref are missing, Please set to safe settings")]
    [Range(0, 1)] [SerializeField] private float bgmDefultVolume = .35f;

    void Awake()
    {
        var obj = GameObject.FindGameObjectsWithTag("BGM");

        if (PlayerPrefs.HasKey("bgm_level"))
            gameObject.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("bgm_level");
        else
        {
            gameObject.GetComponent<AudioSource>().volume = bgmDefultVolume;
            PlayerPrefs.SetFloat("bgm_level", bgmDefultVolume);
        }

        if (obj.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);


        if (AudioControlSingleton.IsActive && !AudioControlSingleton.Instance.HasBGMSource)
            AudioControlSingleton.Instance.BGMSource = gameObject.GetComponent<AudioSource>();
        else
        {
            lock (AudioControlSingleton.padlock)
            {
                AudioControlSingleton.Instance.BGMSource = gameObject.GetComponent<AudioSource>();
            }
        }
    }
}

public sealed class AudioControlSingleton : IDisposable
{
    private static AudioControlSingleton _instance = null;

    /// <summary>
    /// Field for handles Mutex locking.
    /// </summary>
    public static readonly object padlock = new object();

    public bool _hasBGMSource;


    AudioControlSingleton()
    {
    }

    public static AudioControlSingleton Instance
    {
        get
        {
            lock (padlock)
            {
                if (_instance == null)
                    _instance = new AudioControlSingleton();

                return _instance;
            }
        }
    }


    public static bool IsActive
    {
        get => _instance != null;
    }

    /// <summary>
    /// Property to get/set Background music source
    /// </summary>
    public AudioSource BGMSource { get; set; }

    /// <summary>
    /// Property to check if Instance has audio source assigned
    /// </summary>
    public bool HasBGMSource
    {
        get => BGMSource != null;
    }

    /// <summary>
    /// Property to get/set Background music source
    /// </summary>
    public AudioSource SFXSource { get; set; }

    /// <summary>
    /// Property to check if Instance has audio source assigned
    /// </summary>
    public bool HasSFXSource
    {
        get => SFXSource != null;
    }

    public void Dispose()
    {
        Debug.Log("whoops");
        _instance = null;
    }
}