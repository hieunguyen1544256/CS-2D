using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TextAnim : MonoBehaviour
{
    private void Start()
    {
        var startScale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DOScale(startScale, 1.2f).SetEase(Ease.OutBounce);
    }
}
