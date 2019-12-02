using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiManager : SingletoneMonoBehaviour<GuiManager>, IInitialziable
{
    [SerializeField] Camera uiCamera = null;
    [SerializeField] PopupController popupController = null;
    [SerializeField] ScreenController screenController = null;

    public PopupController PopupController => popupController;
    public ScreenController ScreenController => screenController;

    public void Initialize()
    {
        PopupController.Initialize(uiCamera);
        ScreenController.Initialize(uiCamera);
    }

}
