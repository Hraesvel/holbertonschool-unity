using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private List<AudioClip> audioClips;

    [SerializeField] private PlayerController pc;

    [SerializeField] private AudioSource running;
    [SerializeField] private AudioSource landing;

    public string Terrain { get; set; }

    private void Start()
    {
       
    }

    private void Update()
    {
    }


    public void FootStep()
    {
        if (pc.InAir)
            return;


        switch (Terrain)
        {
            case "GrassPlatform":
                running.clip = audioClips.Find((clip) => clip.name == "footsteps_running_grass");
                break;
            case "RockPlatform":
                running.clip = audioClips.Find((clip) => clip.name == "footsteps_running_rock");
                break;
        }

        running.Play();
    }

    public void Landing()
    {
        switch (Terrain)
        {
            case "GrassPlatform":
                landing.clip = audioClips.Find((clip) => clip.name == "footsteps_landing_grass");
                break;
            case "RockPlatform":
                landing.clip = audioClips.Find((clip) => clip.name == "footsteps_landing_rock");
                break;
        }

        landing.Play();
    }
}