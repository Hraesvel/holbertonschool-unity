using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    /// <summary>
    /// Text object that will display the time.
    /// </summary>
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


    /// <summary>
    /// property that can me used start or stop the timer or check if the timer
    /// has stop running.
    /// </summary>
    public bool Run
    {
        get => _runTimer;
        set => _runTimer = value;
    }

    
    /// <summary>
    /// Property to check if the timer has stop because the play has won the match.
    /// </summary>
    public bool HasWon { get =>_hasWon; }

    /// <summary>
    /// Property use to offset the timer
    /// </summary>
    public float TimeOffset
    {
        get => _offset;
        set => _offset = value;
    }


    /// <summary>
    /// method use to stop the timer on a win case and
    /// send the time to the assigned WinMenu
    /// </summary>
    public void Win()
    {
        Run = false;
        _hasWon = true;
        
        gameObject.SetActive(false);
        winMenu.DisplayWinMenu(_curTime);
    }

    private void Update()
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
