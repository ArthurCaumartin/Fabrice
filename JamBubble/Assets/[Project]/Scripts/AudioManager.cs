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

    public int volumeMasterIndex = 0;
    public int volumeSFXIndex = 0;
    public int volumeMusicIndex = 0;


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

    public void SetVolumeMaster(int volume){
        float newVolume = Mathf.Lerp(-30f, 0f, volume*.1f);
        masterGroup.audioMixer.SetFloat("volumeMaster", newVolume); 
        volumeMasterIndex = volume;
    }

    public void SetVolumeSFX(int volume){
        float newVolume = Mathf.Lerp(-30f, 0f, volume*.1f);
        sfxGroup.audioMixer.SetFloat("volumeSFX", newVolume); 
        volumeSFXIndex = volume;
    }

    public void SetVolumeMusic(int volume){
        float newVolume = Mathf.Lerp(-30f, 0f, volume*.1f);
        musicGroup.audioMixer.SetFloat("volumeMusic", newVolume); 
        volumeMusicIndex = volume;
    }

    public int GetVolumeMaster(){
        return volumeMasterIndex;
    }

    public int GetVolumeSFX(){
        return volumeSFXIndex;
    }

    public int GetVolumeMusic(){
        return volumeMusicIndex;
    }
}
