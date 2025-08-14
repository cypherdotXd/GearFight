using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    [SerializeField] private List<AudioKeyPair> _audioKeyPairs;
    
    private AudioSource _bgmAudioSource;
    private AudioSource _primaryAudioSource;
    private Dictionary<string, AudioClip> _audioClips = new();
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        _bgmAudioSource = gameObject.AddComponent<AudioSource>();
        _primaryAudioSource = gameObject.AddComponent<AudioSource>();
        
        _audioClips = _audioKeyPairs.ToDictionary(keyPair1 => keyPair1.key, keyPair2 => keyPair2.audioClip);
    }

    public bool PlayBgm(string key)
    {
        if (!_audioClips.TryGetValue(key, out var audioClip)) return false;
        _bgmAudioSource.resource = audioClip;
        _bgmAudioSource.Play();
        return true;
    }
    
    public bool PlaySfx(string key)
    {
        if (!_audioClips.TryGetValue(key, out var audioClip)) return false;
        _primaryAudioSource.resource = audioClip;
        _primaryAudioSource.Play();
        return true;
    }

    public void StopAllSounds()
    {
        _primaryAudioSource.Stop();
        _bgmAudioSource.Stop();
    }

    public void SetVolume(float volume)
    {
        _bgmAudioSource.volume = volume;
        _primaryAudioSource.volume = volume;
    }

    public void SetBgmVolume(float volume)
    {
        _bgmAudioSource.volume = volume;
    }
}

[Serializable]
public class AudioKeyPair
{
    public string key;
    public AudioClip audioClip;
}
