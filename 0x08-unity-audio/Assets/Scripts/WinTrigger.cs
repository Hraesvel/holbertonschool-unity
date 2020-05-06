using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class WinTrigger : MonoBehaviour
{
    [SerializeField] private Timer _timer;
    [SerializeField] private AudioSource bmg;
    private AudioSource win;


    private void Start()
    {
        if (_timer == null)
            _timer = FindObjectOfType<Timer>();
        win = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _timer.Win();
            bmg.Stop();
            win.Play();
            Time.timeScale = 0;
        }
        
    }
}