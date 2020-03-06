using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityScript.Steps;

public class MainMenu : MonoBehaviour
{
    public Material trapMat;
    public Material goalMat;
    public Toggle colorblindMode;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("colorblindMode"))
            colorblindMode.isOn = ( PlayerPrefs.GetInt("colorblindMode")  > 0 ? true : false);
        else
        {
            PlayerPrefs.SetInt("colorblindMode", 0);
            colorblindMode.isOn = false;
        }
    }

    public void PlayMaze()
    {
        PlayerPrefs.SetInt("colorblindMode", colorblindMode.isOn ? 1: 0);
       
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
}