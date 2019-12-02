using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class OpacityCell : AnimatedCell
{
    [SerializeField] public List<Graphic> affectedGraphics;

    Dictionary<Graphic, float> initialOpacityValue;


    protected override void Initialize()
    {
        base.Initialize();
        initialOpacityValue = new Dictionary<Graphic, float>();
        foreach (var graphic in affectedGraphics)
        {
            initialOpacityValue.Add(graphic, graphic.color.a);
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
            foreach (var graphic in affectedGraphics)
            {
                Color c = graphic.color;
                graphic.color = new Color(c.r, c.g, c.b, Mathf.Lerp(0f, initialOpacityValue[graphic], factor));
            }
        }
    }
}
