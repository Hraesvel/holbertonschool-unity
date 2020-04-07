using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinTrigger : MonoBehaviour
{
    [SerializeField] private Timer _timer;


    private void Awake()
    {
        if (_timer == null)
            _timer = FindObjectOfType<Timer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _timer.Win();
            Time.timeScale = 0;
        }
        
    }
}