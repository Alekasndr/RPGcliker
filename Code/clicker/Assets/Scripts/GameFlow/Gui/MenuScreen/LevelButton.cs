using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public event Action<LevelButton> OnClick;
    [SerializeField] Text textLabel;

    public Button button;
    public int LevelIndex
    {
        get;
        set;
    }


    public string TextLabel
    {
        get
        {
            return textLabel.text;
        }
        set
        {
            textLabel.text = value;
        }
    }



    public void OnClicked()
    {
        OnClick?.Invoke(this);
    }
}
