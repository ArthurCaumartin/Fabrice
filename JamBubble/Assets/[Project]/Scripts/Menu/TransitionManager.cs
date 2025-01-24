using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance;

    [SerializeField] private Transform bubbleCanvas;
    [SerializeField] private Image colorScreen;
    [SerializeField] private GameObject transiParent; 
    private bool onTransition = false;

    void Awake(){
        if(Instance != null){
            Destroy(this.gameObject);
        }
        else {
            DontDestroyOnLoad(this.gameObject);
            Instance = this;
        }
        
    }

    public void TransitionToScene(string sceneName){
        StartCoroutine(TransitionToSceneCoroutine(sceneName));
    }

    public IEnumerator TransitionToSceneCoroutine(string sceneName){
        yield return new WaitUntil(() => !onTransition);
        onTransition = true;
        transiParent.SetActive(true);
        colorScreen.DOFade(1f, (.25f + (0.05f * bubbleCanvas.childCount))).From(0f);
        for(int i = 0; i < bubbleCanvas.childCount ; i++){
            bubbleCanvas.GetChild(i).DOLocalMoveY(0,.25f - (i*-0.05f)).From(-2350f);
            yield return new WaitForSeconds(0.05f);
        }
        
        yield return new WaitForSeconds(.25f);
        SceneManager.LoadScene(sceneName);
        colorScreen.DOFade(0f, (0.05f * bubbleCanvas.childCount)).From(1f);
        for(int i = 0; i < bubbleCanvas.childCount ; i++){
            bubbleCanvas.GetChild(i).DOLocalMoveY(2350f,.25f - (i*-0.05f)).From(0f);
            yield return new WaitForSeconds(0.05f);
        }
        
        yield return new WaitForSeconds(.25f + (0.05f * bubbleCanvas.childCount));
        transiParent.SetActive(false);
        onTransition = false;
    }
}
