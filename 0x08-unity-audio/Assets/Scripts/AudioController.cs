using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Object = UnityEngine.Object;

public class AudioController : MonoBehaviour
{
    [Tooltip(
        "Set the default volume of the background music if player's pref are missing, Please set to safe settings")]
    [Range(0, 1)]
    [SerializeField]
    private float bgmDefultVolume = .35f;

    private float sfxDefultVolume = .85f;

    [SerializeField] private AudioMixer mixer;

    void Start()
    {
        // Todo: Track changer

        AudioMixer bgm = mixer.FindMatchingGroups("BGM")[0].audioMixer;

        if (PlayerPrefs.HasKey("bgm_level"))
        {
            if (PlayerPrefs.GetFloat("bgm_level") > 0f)
                bgm.SetFloat("VolBGM", 20f * Mathf.Log10(PlayerPrefs.GetFloat("bgm_level")));
            else
                bgm.SetFloat("VolBGM", -80f);
        }
        else
        {
            bgm.GetFloat("VolBGM", out var db);
            PlayerPrefs.SetFloat("bgm_level", Mathf.Pow(10f, db / 20f));
        }


        AudioMixer sfx = mixer.FindMatchingGroups("SFX")[0].audioMixer;
        if (PlayerPrefs.HasKey("sfx_level"))
        {
            if (PlayerPrefs.GetFloat("sfx_level") > 0f)
                sfx.SetFloat("VolSFX", 20f * Mathf.Log10(PlayerPrefs.GetFloat("sfx_level")));
            else
                sfx.SetFloat("VolSFX", -80f);
        }
        else
        {
            sfx.SetFloat(
                "VolSFX",
                20f * Mathf.Log10(sfxDefultVolume)
            );
            sfx.GetFloat("VolSFX", out var db);
            PlayerPrefs.SetFloat("sfx_level", Mathf.Pow(10f, db / 20f));
        }

        //
        // if (obj.Length > 1)
        // {
        //     Destroy(gameObject);
        //     return;
        // }
        //
        // DontDestroyOnLoad(gameObject);


        // if (AudioControlSingleton.IsActive && !AudioControlSingleton.Instance.HasMixerSource)
        //     AudioControlSingleton.Instance.Mixer = mixer;
        // else
        // {
        //     lock (AudioControlSingleton.padlock)
        //     {
        //         // AudioControlSingleton.Instance.BGMSource = gameObject.GetComponent<AudioSource>();
        //         AudioControlSingleton.Instance.Mixer = mixer;
        //     }
        // }


        if (!AudioControlSingleton.IsActive)
            AudioControlSingleton.Instance.Mixer = mixer;
        else
        {
            lock (AudioControlSingleton.padlock)
            {
                // AudioControlSingleton.Instance.BGMSource = gameObject.GetComponent<AudioSource>();
                if (!AudioControlSingleton.Instance.HasMixerSource)
                    AudioControlSingleton.Instance.Mixer = mixer;
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
    [Obsolete("Direct audio source manipulation is being replace with Mixer board")]
    public AudioSource BGMSource { get; set; }

    public AudioMixer Mixer { get; set; }

    /// <summary>
    /// Property to check if Instance has audio source assigned
    /// </summary>
    ///
    [Obsolete("Direct audio source manipulation is being replace with Mixer board")]

    public bool HasBGMSource
    {
        get => BGMSource != null;
    }

    /// <summary>
    /// Property to get/set Master Mixer board 
    /// </summary>
    public bool HasMixerSource
    {
        get => Mixer != null;
    }

    /// <summary>
    /// Property to get/set Background music source
    /// </summary>
    [Obsolete("Direct audio source manipulation is being replace with Mixer board")]
    public AudioSource SFXSource { get; set; }

    /// <summary>
    /// Property to check if Instance has audio source assigned
    /// </summary>
    [Obsolete("Direct audio source manipulation is being replace with Mixer board")]
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