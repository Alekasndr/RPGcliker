using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[RequireComponent(typeof(Canvas))]
public abstract class BaseScreen : BaseGlobalUiItem<BaseScreen>
{

    #region Fields

    [SerializeField] GuiVisualController visualController = null;

    #endregion


    #region Properties

    public ScreenType ScreenType { get; private set; }

    #endregion



    #region Public methods
    
    public void BaseInitialize<T>(ScreenType screenType, Action<T> initializeCallback = null) where T : BaseScreen
    {
        ScreenType = screenType;
        InitializeCanvasLogic();
        OnInitialize();
        initializeCallback?.Invoke(this as T);
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
