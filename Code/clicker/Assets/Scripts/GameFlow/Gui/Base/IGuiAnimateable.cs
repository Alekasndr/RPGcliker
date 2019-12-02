using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ShowingState
{
    None = 0,
    Hided = 1,
    Showing = 2,
    Showed = 3,
    Hiding = 4,
}
public interface IGuiAnimateable 
{
    event Action<IGuiAnimateable> OnStateChanged;
    ShowingState CurrentState { get; set; }

    float InDuration {get; set; }
    float OutDuration {get; set; }
    void Show(bool isImmediately, Action onShowed);
    void Hide(bool isImmediately, Action onShowed);
}
