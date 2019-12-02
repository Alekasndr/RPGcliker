using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;


public abstract class AnimatedCell : MonoBehaviour, IGuiAnimateable
{
    bool isInitialized = false;
    float inDuration = 0.0f;
    float outDuration = 0.0f;

    [SerializeField] protected AnimationCurve inCurve;
    [SerializeField] protected AnimationCurve outCurve;
    
    public float InDuration
    {
        get
        {
            return inDuration;
        }
        set
        {
            inDuration = Mathf.Abs(value);
        }
    }


    public float OutDuration
    {
        get
        {
            return outDuration;
        }
        set
        {
            outDuration = Mathf.Abs(value);
        }
    }


    ShowingState currentState = ShowingState.None;

    public event Action<IGuiAnimateable> OnStateChanged;

    public ShowingState CurrentState
    {
        get
        {
            return currentState;
        }
        set
        {
            if (currentState != value)
            {
                currentState = value;
                OnStateChanged?.Invoke(this);
            }
        }
    }

    // public event Action<IGuiAnimateable> OnStateChanged;

    protected virtual void OnEnable()
    {
        DOTween.Kill(gameObject, true);
    }


    protected virtual void OnDisable()
    {
        DOTween.Kill(gameObject);
    }


    protected virtual void Initialize()
    {

    }


    public void Show(bool isImmediately, Action onCompleted)
    {
        if (!isInitialized)
        {
            Initialize();
            isInitialized = true;
        }
        bool shouldUseLogic = (CurrentState == ShowingState.Showing && isImmediately) ||
                              (CurrentState == ShowingState.Hiding || CurrentState == ShowingState.Hided) ||
                              CurrentState == ShowingState.None;
        if (shouldUseLogic)
        {
            currentState = ShowingState.Showing;
            onCompleted += () =>
            {
                currentState = ShowingState.Showed;
            };
            PlayShowAnimation(isImmediately, onCompleted);
        }
    }


    public void Hide(bool isImmediately, Action onCompleted)
    {
        if (!isInitialized)
        {
            Initialize();
            isInitialized = true;
        }

        bool shouldUseLogic = (CurrentState == ShowingState.Hiding && isImmediately) ||
                              (CurrentState == ShowingState.Showing || CurrentState == ShowingState.Showed) ||
                              CurrentState == ShowingState.None;
        if (shouldUseLogic)
        {
            currentState = ShowingState.Hiding;
            onCompleted += () =>
            {
                currentState = ShowingState.Hided;
            };
            PlayHidesAnimation(isImmediately, onCompleted);
        }
    }


    protected abstract void PlayShowAnimation(bool isImmediately, Action onComplete);
    protected abstract void PlayHidesAnimation(bool isImmediately, Action onComplete);




    protected abstract void ApplyValue(float factor);
}