using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySFX : MonoBehaviour
{
    [SerializeField] private AudioSource runAudioSource;
    [SerializeField] private AudioSource landAudioSource;
    private string _prevTerrain;

    [Serializable]
    public struct AudioSet
    {
        public string name;
        public AudioClip clip;
    }

    public AudioSet[] audioSets;

    [SerializeField] private Hashtable audioClips = new Hashtable();

    private PlayerController _player;

    private void Awake()
    {
        _player = transform.parent.GetComponent<PlayerController>();
        foreach (var set in audioSets)
        {
            audioClips.Add(set.name, set.clip);
        }

        Debug.Log(audioClips.Count);
    }


    public void PlaySfx(string clip)
    {
        if (!_player.IsGrounded())
            return;
        switch (clip)
        {
            case "landing":
                if (_player.CurrentTerrian != _prevTerrain)
                    landAudioSource.clip = (AudioClip) audioClips[$"{_player.CurrentTerrian}_{clip}"];
                landAudioSource.Play();
                break;
            case "running":
                if (_player.CurrentTerrian != _prevTerrain)
                    runAudioSource.clip = (AudioClip) audioClips[$"{_player.CurrentTerrian}_{clip}"];
                runAudioSource.Play();
                break;
        }
        
    }
}