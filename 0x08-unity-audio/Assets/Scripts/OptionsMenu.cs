using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private Slider _bgmSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Toggle _invertY;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private EventSystem eventSystem;

    // Start is called before the first frame update
    void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            mainCamera.gameObject.SetActive(true);
            eventSystem.gameObject.SetActive(true);
        }
        else
            lock (PauseSingleton.padlock)
            {
                GameObject firstSelect = transform.Find("BGMSlider").gameObject;
                PauseSingleton.Instance.EventSystem.SetSelectedGameObject(firstSelect);
            }

        if (PlayerPrefs.HasKey("bgm_level"))
            _bgmSlider.value = PlayerPrefs.GetFloat("bgm_level");
        if (PlayerPrefs.HasKey("sfx_level"))
            _sfxSlider.value = PlayerPrefs.GetFloat("sfx_level");
        if (PlayerPrefs.HasKey("invert_Y"))
            _invertY.isOn = PlayerPrefs.GetInt("invert_Y") != 0;
    }

    /// <summary>
    /// Method to handle Apply button functionality
    /// </summary>
    public void ApplyButton()
    {
        PlayerPrefs.SetFloat("bgm_level", _bgmSlider.value);
        PlayerPrefs.SetFloat("sfx_level", _sfxSlider.value);
        PlayerPrefs.SetInt("invert_Y", _invertY.isOn ? 1 : 0);

        if (AudioControlSingleton.IsActive && AudioControlSingleton.Instance.HasBGMSource)
            lock (AudioControlSingleton.padlock)
            {
                AudioControlSingleton.Instance.BGMSource.volume = _bgmSlider.value;
            }
        else
            Debug.Log($"Singleton for audio failed :\n" +
                      $" details instance active? {AudioControlSingleton.IsActive} and Has Source? {AudioControlSingleton.Instance.HasBGMSource}");

        if (PauseSingleton.Active)
            lock (PauseSingleton.padlock)
            {
                PauseSingleton.Instance.Camera.isInverted = _invertY.isOn;
            }

        PlayerPrefs.Save();
    }

    /// <summary>
    /// Method to handle Back button functionality
    /// </summary>
    public void BackButton()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
            SceneManager.LoadScene("Scenes/MainMenu");
        else
        {
            lock (PauseSingleton.padlock)
            {
                PauseSingleton.Instance.Menu.SetActive(true);
                GameObject fistSelect = GameObject.Find("PauseCanvas").transform.Find("ResumeButton").gameObject;
                PauseSingleton.Instance.EventSystem.SetSelectedGameObject(fistSelect);
            }

            SceneManager.UnloadSceneAsync(1);
        }
    }
}