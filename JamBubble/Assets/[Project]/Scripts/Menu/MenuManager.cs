using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void LoadScene(string sceneName){
        TransitionManager.Instance.TransitionToScene(sceneName);
        DestroyImmediate(this);
    }
}
