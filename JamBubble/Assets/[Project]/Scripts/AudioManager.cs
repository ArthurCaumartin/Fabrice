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

    public void PlaySFX(string name, float volume = -1, float pitch = -1){
        Sound soundToPlay = Array.Find(sfxSound, sound => sound.soundId == name);
        if(soundToPlay != null){
            AudioSource audioSource = sfxSource;
            if(audioSource.isPlaying || volume != -1 || pitch != -1){
                audioSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
                if(volume != -1) audioSource.volume = volume;
                if(pitch != -1)audioSource.pitch = pitch;
                Destroy(audioSource, soundToPlay.sound.length);
            }

            audioSource.PlayOneShot(soundToPlay.sound);
        }
    }

    public void PlayMusic(string name){
        Sound soundToPlay = Array.Find(musicSound, sound => sound.soundId == name);
        if(soundToPlay != null){
            musicSource.PlayOneShot(soundToPlay.sound);
        }
    }

    public void StopMusic(){
        musicSource.Stop();
    }
}
