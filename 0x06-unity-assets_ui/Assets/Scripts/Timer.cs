using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;
    [SerializeField] private WinMenu winMenu;

    private string _curTime = "0:00.00";
    // Start is called before the first frame update
    private bool _runTimer;
    private float _offset;
    private bool _hasWon;

    private void Awake()
    {
        Time.timeScale = 1;
       
    }

    private void Start()
    {
        if (winMenu == null)
            winMenu = FindObjectOfType<WinMenu>();
        
        winMenu.gameObject.SetActive(false);
    }


    public bool Run
    {
        get => _runTimer;
        set => _runTimer = value;
    }

    public bool HasWon { get =>_hasWon; }

    public float TimeOffset
    {
        get => _offset;
        set => _offset = value;
    }


    public void Win()
    {
        Run = false;
        _hasWon = true;
        
        gameObject.SetActive(false);
        winMenu.DisplayWinMenu(_curTime);
    }
    
    void Update()
    {
        if (_runTimer)
        {
            _curTime = TimeSpan.FromSeconds(Time.timeSinceLevelLoad - TimeOffset).ToString("h':'mm'.'ss");
            timerText.text = _curTime;
        }
        else
            timerText.text = _curTime;
        
        

    }
}
