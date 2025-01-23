using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LoopAnim : MonoBehaviour
{
    void Start()
    {
        transform.DOScale(1.1f, 1f).From(1f).SetEase(Ease.InOutElastic).SetLoops(-1, LoopType.Yoyo);
    }

}
