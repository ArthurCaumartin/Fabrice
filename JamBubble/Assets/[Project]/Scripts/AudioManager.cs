using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private Sound[] sfxSound;
    [SerializeField] private Sound[] musicSound;

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;


    void Awake(){
        if(Instance != null){
            Destroy(this.gameObject);
        }
        else {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void PlaySFX(string name){
        Sound soundToPlay = Array.Find(sfxSound, sound => sound.soundId == name);
        if(soundToPlay != null){
            sfxSource.PlayOneShot(soundToPlay.sound);
        }
    }

    public void PlayMusic(string name){
        Sound soundToPlay = Array.Find(musicSound, sound => sound.soundId == name);
        if(soundToPlay != null){
            musicSource.PlayOneShot(soundToPlay.sound);
        }
    }
}
