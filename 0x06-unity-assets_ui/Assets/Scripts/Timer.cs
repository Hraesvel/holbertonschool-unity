using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;

    private string _curTime;
    // Start is called before the first frame update
    private bool _runTimer;
    private float _offset = 0;
    private bool _isPaused;

    public bool Run
    {
        get => _runTimer;
        set => _runTimer = value;
    }

    public float TimeOffset
    {
        get => _offset;
        set => _offset = value;
    }

    public bool IsPaused
    {
        get => _isPaused;
        set => _isPaused = value;
    }

    private void Awake()
    {
        _curTime = "0:00.00";
    }

    // Update is called once per frame
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
