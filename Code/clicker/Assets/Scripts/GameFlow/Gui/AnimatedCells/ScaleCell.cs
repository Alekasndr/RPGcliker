using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleCell : AnimatedCell
{
    [SerializeField] Vector3 minScale = Vector3.zero;
    [SerializeField] Vector3 endScale = Vector3.one;



    protected override void Initialize()
    {
        base.Initialize();
    }


    protected override void PlayShowAnimation(bool isImmediately, Action onComplete)
    {
        if (Mathf.Approximately(InDuration, 0f))
        {
            isImmediately = true;
        }

        if (isImmediately)
        {
            ApplyValue(1f);
            onComplete?.Invoke();
        }
        else
        {
            DOTween.To(() => 0f, (x) =>
            {
                ApplyValue(inCurve.Evaluate(x));
            }, 1f, InDuration).SetTarget(gameObject).OnComplete(() => onComplete?.Invoke());
        }
    }


    protected override void PlayHidesAnimation(bool isImmediately, Action onComplete)
    {
        if (Mathf.Approximately(OutDuration, 0f))
        {
            isImmediately = true;
        }

        if (isImmediately)
        {
            ApplyValue(0f);
            onComplete?.Invoke();
        }
        else
        {
            DOTween.To(() => 0f, (x) =>
            {
                ApplyValue(1 - outCurve.Evaluate(x));
            }, 1f, OutDuration).SetTarget(gameObject).OnComplete(() => onComplete?.Invoke());
        }
    }


    protected override void ApplyValue(float factor)
    {
        if (this != null)
        {
            transform.localScale = Vector3.LerpUnclamped(minScale, endScale, factor);
        }
    }

}
