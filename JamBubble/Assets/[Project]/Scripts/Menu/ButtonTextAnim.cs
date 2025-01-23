using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Febucci.UI;

public class ButtonTextAnim : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    void Start(){
        GetComponent<TextAnimator_TMP>().SetBehaviorsActive(false);
		GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void OnSelect(BaseEventData eventData)
    {
        GetComponent<TextAnimator_TMP>().SetBehaviorsActive(true);
        AudioManager.Instance.PlaySFX("ui_select"); 
    }

    public void OnDeselect(BaseEventData eventData)
    {
        GetComponent<TextAnimator_TMP>().SetBehaviorsActive(false);
    }

    public void OnClick()
    {
        AudioManager.Instance.PlaySFX("ui_click"); 
    }
}
