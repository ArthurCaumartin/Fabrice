using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Febucci.UI;
using DG.Tweening;
using TMPro;

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
        transform.DOScale(1f,.5f).From(0.6f).SetEase(Ease.OutBounce);
        GetComponent<TMP_Text>().color = Color.white;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        GetComponent<TextAnimator_TMP>().SetBehaviorsActive(false);
        transform.DOScale(.6f,.5f).From(1f).SetEase(Ease.OutBounce);
        GetComponent<TMP_Text>().color = Color.gray;
    }

    public void OnClick()
    {
        AudioManager.Instance.PlaySFX("ui_click"); 
    }
}
