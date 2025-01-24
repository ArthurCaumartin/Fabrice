using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using Rewired;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    bool onSettings = false;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject firstSettingsItem;

    [SerializeField] private List<Button> buttonsToDisable;

    


    void Start(){
        AudioManager.Instance?.StopMusic();
        AudioManager.Instance?.PlayMusic("menu");
    }

    public void LoadScene(string sceneName){
        TransitionManager.Instance.TransitionToScene(sceneName);
        DestroyImmediate(this);
    }

    public void QuitGame(){
        Application.Quit();
    }

    private void Update() {
        if(!ReInput.isReady) return;

        IList<Joystick> joysticks = ReInput.controllers.Joysticks;
        for(int i = 0; i < joysticks.Count; i++) {

            Joystick joystick = joysticks[i];
             if(joystick.GetAnyButtonDown() && onSettings) {
                AudioManager.Instance.PlaySFX("ui_cancel"); 
                ShowSettings(false);
            }
            
        }
    }

    public void ShowSettings(bool show){
        StartCoroutine(ShowSettingsCoroutine(show));
    }

    public IEnumerator ShowSettingsCoroutine(bool show){
        
        foreach(Button button in buttonsToDisable){
            button.interactable = !show;
        }

        if(show){
            settingsPanel.SetActive(true);
            settingsPanel.GetComponent<CanvasGroup>().DOFade(1,.5f).From(0f).SetEase(Ease.OutQuint);
            settingsPanel.transform.DOLocalMoveY(0,.5f).From(-430f).SetEase(Ease.OutBounce);
            yield return new WaitForSeconds(.5f);
            EventSystem.current.SetSelectedGameObject(firstSettingsItem);
        }
        else {
            settingsPanel.GetComponent<CanvasGroup>().DOFade(0,.5f).From(1f).SetEase(Ease.OutQuint);
            settingsPanel.transform.DOLocalMoveY(-430f,.5f).From(0f).SetEase(Ease.OutBounce);
            yield return new WaitForSeconds(.5f);
            EventSystem.current.SetSelectedGameObject(playButton);
            settingsPanel.SetActive(false);
        }
        onSettings = show;
    }
}
