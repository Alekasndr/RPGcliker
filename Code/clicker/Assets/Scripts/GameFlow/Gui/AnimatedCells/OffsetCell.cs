using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetCell : AnimatedCell
{
    [SerializeField] Vector2 offset = Vector2.zero;


    protected override void Initialize()
    {
        base.Initialize();
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
                ApplyValue(1f - outCurve.Evaluate(x));
            }, 1f, OutDuration).SetTarget(gameObject).OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        }
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


    protected override void ApplyValue(float factor)
    {
        if (this != null)
        {
            //Debug.Log(factor);
            Vector3 beginValue = new Vector3(offset.x, offset.y, transform.position.z);
            transform.localPosition = Vector3.LerpUnclamped(beginValue, Vector3.zero, factor);
        }
    }

}