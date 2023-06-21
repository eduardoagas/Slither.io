using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ClientMusicPlayer : Singleton<ClientMusicPlayer>
{
    [SerializeField] private AudioClip chompAudioClip;
    private AudioSource _audioSource;
    
    public override void Awake()
    {
        base.Awake();
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayChompAudioClip(){
        _audioSource.clip = chompAudioClip;
        _audioSource.Play();
    }
}
