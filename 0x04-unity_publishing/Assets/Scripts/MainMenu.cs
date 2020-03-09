using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Toggle colorblindMode;
    public Material goalMat;
    public Slider touchSensitivity;
    public Text touchValue;
    public Material trapMat;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("colorblindMode"))
        {
            colorblindMode.isOn = PlayerPrefs.GetInt("colorblindMode") > 0 ? true : false;
        }
        else
        {
            PlayerPrefs.SetInt("colorblindMode", 0);
            colorblindMode.isOn = false;
        }

        if (PlayerPrefs.HasKey("touchSensitivity"))
        {
            touchSensitivity.value = PlayerPrefs.GetFloat("touchSensitivity");
        }
        else
        {
            touchSensitivity.value = 1.0f;
            PlayerPrefs.SetFloat("touchSensitivity", touchSensitivity.value);
        }
    }

    public void PlayMaze()
    {
        PlayerPrefs.SetInt("colorblindMode", colorblindMode.isOn ? 1 : 0);

        if (colorblindMode.isOn)
        {
            trapMat.color = new Color32(255, 112, 0, 1);
            goalMat.color = Color.blue;
        }
        else
        {
            trapMat.color = Color.red;
            goalMat.color = Color.green;
        }

        SceneManager.LoadScene("maze");
    }

    public void QuitMaze()
    {
        Debug.Log("Quit Game");
    }

    public void SensitivityChange()
    {
        // if (touchSensitivity.value.ToString() != touchValue.text) return;

        PlayerPrefs.SetFloat("touchSensitivity", touchSensitivity.value);
        touchValue.text = touchSensitivity.value.ToString();
    }
}