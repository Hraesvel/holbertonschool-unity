using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private Slider _bgmSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Toggle _invertY;

    // Start is called before the first frame update
    void Awake()
    {
       
        
        if (PlayerPrefs.HasKey("bgm_level"))
            _bgmSlider.value = PlayerPrefs.GetFloat("bgm_level");
        if (PlayerPrefs.HasKey("sfx_level"))
            _sfxSlider.value = PlayerPrefs.GetFloat("sfx_level");
        if (PlayerPrefs.HasKey("invert_Y"))
            _invertY.isOn = PlayerPrefs.GetInt("invert_Y") != 0;
    }

    public void ApplyButton()
    {
        PlayerPrefs.SetFloat("bgm_level", _bgmSlider.value);
        PlayerPrefs.SetFloat("sfx_level", _sfxSlider.value);
        PlayerPrefs.SetInt("invert_Y", _invertY.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }


    public void BackButton()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
            SceneManager.LoadScene("Scenes/MainMenu");
        else
            SceneManager.UnloadSceneAsync(1);
    }

    // Update is called once per frame
}