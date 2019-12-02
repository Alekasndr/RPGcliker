using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenController : BaseGuiController<ScreenController, BaseScreen, ScreenType>
{

    #region Constants

    const int DeltaBetweenLayers = 100;

    #endregion



    #region Fields

    [SerializeField] Dictionary<ScreenType, BaseScreen> screenPrefabs;
    List<BaseScreen> actualScreens = new List<BaseScreen>();

    #endregion


    #region Properties

    public bool IsAnyScreenActive => (actualScreens.Count > 0);

    #endregion


    
    #region Public methods

    public override void Show(ScreenType type,
                     bool isImmediately = false,
                     Action<BaseScreen> initializeCallback = null)
    {
        int maxOrder = int.MinValue;
        foreach (var popup in actualScreens)
        {
            maxOrder = Mathf.Max(popup.Order, maxOrder);
        }

        var screenPrefab = screenPrefabs[type];
        var screenInstance = ObjectCreator.CreateObject<BaseScreen>(screenPrefab.gameObject, root);
        
        screenInstance.RootCanvas.worldCamera = Camera;
        screenInstance.RootCanvas.sortingLayerName = defaultSortingLayerName;

        screenInstance.Order = maxOrder + DeltaBetweenLayers;
        screenInstance.BaseInitialize(type, initializeCallback);
        screenInstance.Show(isImmediately);
    }
    

    #endregion



    #region Event handlers

    void GuiScreen_OnHided(BaseScreen screen)
    {
        GameManager.Instance.UnlockTouches(screen);
        actualScreens.Remove(screen);
        Destroy(screen.gameObject);//ToPool
    }


    void GuiScreen_OnStartHiding(BaseScreen screen)
    {
        GameManager.Instance.LockTouches(screen);
    }


    void GuiScreen_OnStartShowing(BaseScreen screen)
    {
        actualScreens.Add(screen);
        GameManager.Instance.LockTouches(screen);
    }


    void GuiScreen_OnShowed(BaseScreen screen)
    {
        GameManager.Instance.UnlockTouches(screen);
    }

    #endregion
}
