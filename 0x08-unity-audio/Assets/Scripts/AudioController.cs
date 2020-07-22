using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Object = UnityEngine.Object;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource mm_BgmSource;

    void Start()
    {
        var obj = GameObject.FindGameObjectsWithTag("BGM");

        if (obj.Length >= 2)
        {
            Destroy(mm_BgmSource);
            Destroy(this.gameObject);
        }
        
        if (PlayerPrefs.HasKey("bgm_level"))
        {
            var vol = PlayerPrefs.GetFloat("bgm_level");
            audioMixer.SetFloat("BGM_VOL",
                vol > 0 ? 20f * Mathf.Log10(vol) : -80f
                );
        }
        else
        {
            audioMixer.GetFloat("BGM_VOL", out var db);
            PlayerPrefs.SetFloat("bgm_level", Mathf.Pow(10f, db/20f));
        }
        
        if (PlayerPrefs.HasKey("sfx_level"))
        {
            var vol = PlayerPrefs.GetFloat("sfx_level");
            audioMixer.SetFloat("SFX_VOL",
                vol > 0 ? 20f * Mathf.Log10(vol) : -80f
                );
        }
        else
        {
            audioMixer.GetFloat("SFX_VOL", out var db);
            PlayerPrefs.SetFloat("sfx_level", Mathf.Pow(10f , db/20f));
        }
        
        DontDestroyOnLoad(mm_BgmSource);
        Destroy(this.gameObject);



        //
        // if (AudioControlSingleton.IsActive && !AudioControlSingleton.Instance.HasBGMSource)
        //     AudioControlSingleton.Instance.BGMSource = gameObject.GetComponent<AudioSource>();
        // else
        // {
        //     lock (AudioControlSingleton.padlock)
        //     {
        //         AudioControlSingleton.Instance.BGMSource = gameObject.GetComponent<AudioSource>();
        //     }
        // }
    }
}

[Obsolete]
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