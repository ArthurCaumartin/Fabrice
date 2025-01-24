using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private Sound[] sfxSound;
    [SerializeField] private Sound[] musicSound;

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    [SerializeField] private AudioMixerGroup masterGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;
    [SerializeField] private AudioMixerGroup musicGroup;


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
                audioSource.outputAudioMixerGroup = sfxGroup;
                Destroy(audioSource, soundToPlay.sound.length);
            }

            audioSource.PlayOneShot(soundToPlay.sound);
        }
    }

    public void PlayMusic(string name){
        Sound soundToPlay = Array.Find(musicSound, sound => sound.soundId == name);
        if(soundToPlay != null){
            musicSource.clip = soundToPlay.sound;
            musicSource.Play();
        }
    }

    public void StopMusic(){
        musicSource.Stop();
    }

    public void SetVolumeMaster(float volume){
        masterGroup.audioMixer.SetFloat("volume", volume); 
    }

    public void SetVolumeSFX(float volume){
        sfxGroup.audioMixer.SetFloat("volume", volume); 
    }

    public void SetVolumeMusic(float volume){
        musicGroup.audioMixer.SetFloat("volume", volume); 
    }

    public int GetVolumeMaster(){
        float volume;
        masterGroup.audioMixer.GetFloat("volume", out volume);
        return (int)(volume*10);
    }

    public int GetVolumeSFX(){
        float volume;
        sfxGroup.audioMixer.GetFloat("volume", out volume);
        return (int)(volume*10);
    }

    public int GetVolumeMusic(){
        float volume;
        musicGroup.audioMixer.GetFloat("volume", out volume);
        return (int)(volume*10);
    }
}
