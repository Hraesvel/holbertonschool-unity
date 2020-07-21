using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class WinTrigger : MonoBehaviour
{
    [SerializeField] private Timer _timer;
    [SerializeField] private AudioSource BGM;
    [SerializeField] private AudioClip WinClip;


    private void Start()
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
           BGM.Stop();
           BGM.time = 0;
           BGM.clip = WinClip;
           BGM.Play();
        }
        
    }
}