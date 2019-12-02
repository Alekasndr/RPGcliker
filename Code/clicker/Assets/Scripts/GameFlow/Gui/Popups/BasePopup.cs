using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[RequireComponent(typeof(Canvas))]
public abstract class BasePopup : BaseGlobalUiItem<BasePopup>
{
    #region Fields

    [SerializeField] GuiVisualController visualController = null;

    #endregion



    #region Properties

    public PopupType PopupType { get; private set; }

    #endregion




    #region Public methods
    
    public void BaseInitialize(PopupType popupType, Action<BasePopup> initializeCallback = null)
    {
        PopupType = popupType;
        OnInitialize();
        initializeCallback?.Invoke(this);
    }


    public void Show(bool isImmediately = false)
    {
        visualController.Hide(true, null);
        _WasStartShowing(this);

        visualController.Show(isImmediately, () =>
        {
            _WasShowed(this);
        });
    }


    public void Hide(bool isImmediately)
    {
        _WasStartHiding(this);

        visualController.Hide(isImmediately, () =>
        {
            _WasHided(this);
        });
    }

    #endregion



    #region Protected methods

    protected virtual void OnInitialize()
    {
    }

    #endregion
}
