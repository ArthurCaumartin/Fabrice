using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Febucci.UI;
using DG.Tweening;
using TMPro;
using Rewired;
using UnityEngine.Rendering;



public class SettingItem : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public int settingId = 0;

    [SerializeField] private GameObject leftCursor;
    [SerializeField] private GameObject rightCursor;

    [SerializeField] private TMP_Text keyText;

    public List<string> keyButton;
    public int index = 0; 

    bool selected = false;  

    private float timeBeforeChange;
    private bool canChange = true;

    Resolution[] resolutions;

    void Start(){
        transform.GetChild(0).GetComponent<TextAnimator_TMP>().SetBehaviorsActive(false);
        if(settingId == 0){
            index = Screen.fullScreen ? 0 : 1;
        }
        else if(settingId == 1){
            resolutions = Screen.resolutions;
            List<string> options = new List<string>();
            for(int i = 0; i < resolutions.Length; i++){
                keyButton.Add(resolutions[i].width + "x" + resolutions[i].height + " (" + (Mathf.FloorToInt((float)resolutions[i].refreshRateRatio.value)).ToString() + "FPS)");

                if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) index = i;
            }
        }
        else if(settingId == 2){
            index = AudioManager.Instance.GetVolumeMaster();
        }
        else if(settingId == 3){
            index = AudioManager.Instance.GetVolumeSFX();
        }
        else if(settingId == 4){
            index = AudioManager.Instance.GetVolumeMusic();
        }
        ChangeValue(index);
    }

    public void OnSelect(BaseEventData eventData)
    {
        transform.GetChild(0).GetComponent<TextAnimator_TMP>().SetBehaviorsActive(true);
        AudioManager.Instance.PlaySFX("ui_select"); 
        selected = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        transform.GetChild(0).GetComponent<TextAnimator_TMP>().SetBehaviorsActive(false);
        selected = false;
    }

    void Update(){
        if(!canChange && timeBeforeChange < .15f){
            timeBeforeChange += Time.deltaTime;  
            if(timeBeforeChange >= .15f){
                timeBeforeChange = .15f;
                canChange = true;
            } 
        }
        if(canChange && selected){
            for (int playerId = 0; playerId < ReInput.players.playerCount; playerId++)
            {
                Player player = ReInput.players.GetPlayer(playerId);


                if (player.GetAxisRaw("Horizontal") >= .5f) ChangeValue(index+1);
                else if (player.GetAxisRaw("Horizontal") <= -.5f) ChangeValue(index-1);

            }
        }
    }

    public void ChangeValue(int newIndex){
        if(newIndex < 0 || newIndex >= keyButton.Count) return;
        AudioManager.Instance.PlaySFX("ui_select"); 
        index = newIndex;

        canChange = false;
        timeBeforeChange = 0f;  
        keyText.text = keyButton[newIndex];   

        leftCursor.SetActive(newIndex > 0);  
        rightCursor.SetActive(newIndex < keyButton.Count-1);  

        SetValue(newIndex);
    }

    private void SetValue(int index){
        if(settingId == 0){
            Screen.fullScreen = (index == 0);
        }
        else if(settingId == 1){
            Resolution newRes = resolutions[index];
            Screen.SetResolution(newRes.width, newRes.height, Screen.fullScreen);
        }
        else if(settingId == 2){
            AudioManager.Instance?.SetVolumeMaster(index);
        }
        else if(settingId == 3){
            AudioManager.Instance?.SetVolumeSFX(index);
        }
        else if(settingId == 4){
            AudioManager.Instance?.SetVolumeMusic(index);
        }
    }
}
