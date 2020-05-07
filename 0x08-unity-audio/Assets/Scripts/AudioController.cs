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
        // Todo: Track changer and one BGM player for whole project
        // GameObject[] obj = GameObject.FindGameObjectsWithTag("BGM");
        //
        // if (obj.Length > 1)
        // {
        //     Destroy(gameObject);
        //     return;
        // }
        //
        // DontDestroyOnLoad(gameObject);


        ConfigMixer("BGM", "bgm_level", "VolBGM");
        ConfigMixer("SFX", "sfx_level", "VolSFX");


        if (!AudioControlSingleton.IsActive)
        {
            // AudioControlSingleton.Instance.BGMSource = gameObject.GetComponent<AudioSource>();
            AudioControlSingleton.Instance.Mixer = mixer;
        }
        else
        {
            lock (AudioControlSingleton.padlock)
            {
                // if (!AudioControlSingleton.Instance.HasBGMSource)
                //     AudioControlSingleton.Instance.BGMSource = gameObject.GetComponent<AudioSource>();
                if (!AudioControlSingleton.Instance.HasMixerSource)
                    AudioControlSingleton.Instance.Mixer = mixer;
            }
        }
    }

    private void ConfigMixer(string mixerGrp, string playerPrefKey, string mixerParam)
    {
        AudioMixer bgm = mixer.FindMatchingGroups(mixerGrp)[0].audioMixer;

        if (PlayerPrefs.HasKey(playerPrefKey))
        {
            if (PlayerPrefs.GetFloat(playerPrefKey) > 0f)
                bgm.SetFloat(mixerParam, 20f * Mathf.Log10(PlayerPrefs.GetFloat(playerPrefKey)));
            else
                bgm.SetFloat(mixerParam, -80f);
        }
        else
        {
            bgm.GetFloat(mixerParam, out var db);
            PlayerPrefs.SetFloat(playerPrefKey, Mathf.Pow(10f, db / 20f));
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
    
    AudioControlSingleton() { }

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

    public void ChangeBGMTrack(AudioClip track)
    {
        if (!HasBGMSource)
        {
            Debug.Log("BGM audio source was not assigned/found to AudioControlSingleton");
            return;
        }
        BGMSource.clip = track;
        // BGMSource.time = 0f;
    }

    public AudioMixer Mixer { get; set; }

    /// <summary>
    /// Property to check if Instance has audio source assigned
    /// </summary>
    ///
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