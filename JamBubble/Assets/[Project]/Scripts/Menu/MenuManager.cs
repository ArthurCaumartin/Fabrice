using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    void Start(){
        AudioManager.Instance?.StopMusic();
        AudioManager.Instance?.PlayMusic("menu");
    }

    public void LoadScene(string sceneName){
        TransitionManager.Instance.TransitionToScene(sceneName);
        DestroyImmediate(this);
    }
}
