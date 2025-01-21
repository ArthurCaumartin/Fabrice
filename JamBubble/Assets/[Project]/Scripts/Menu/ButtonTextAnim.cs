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
    }

    public void OnSelect(BaseEventData eventData)
    {
        GetComponent<TextAnimator_TMP>().SetBehaviorsActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        GetComponent<TextAnimator_TMP>().SetBehaviorsActive(false);
    }
}
