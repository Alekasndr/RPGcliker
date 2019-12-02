using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PopupController : BaseGuiController<PopupController, BasePopup, PopupType>
{

    #region Constants

    [SerializeField] Dictionary<PopupType, BasePopup> popupPrefabs;
    const int DeltaBetweenLayers = 100;

    #endregion



    #region Helpers

    class QueueItem
    {
        public PopupType popupType;
        public bool isAllowedDublicates;
        public bool isImmediately;
        public Action<BasePopup> onInitalizeCallback;
    }

    #endregion



    #region Fields

    List<QueueItem> popupsQueue = new List<QueueItem>();
    List<BasePopup> actualPopups = new List<BasePopup>();

    #endregion


    #region Properties

    public bool IsAnyPopupActive => (actualPopups.Count > 0);


    #endregion

    
    #region Public methods

    public override void Show(PopupType type,
                     bool isImmediately = false, 
                     Action<BasePopup> initializeCallback = null)
    {
        Show(type, isImmediately, false, initializeCallback);
    }


    
    public void Show(PopupType type,
                     bool isImmediately = false, 
                     bool allowDublicates = false, 
                     Action<BasePopup> initializeCallback = null)
    {
        QueueItem item = new QueueItem();
        item.popupType = type;
        item.isImmediately = isImmediately;
        item.isAllowedDublicates = allowDublicates;
        item.onInitalizeCallback = initializeCallback;

        popupsQueue.Add(item);
        TryShow();
    }

    #endregion



    #region Private methods

    void TryShow()
    {
        if (popupsQueue.Count > 0)
        {
            bool isAnyPopupShowed = IsAnyPopupActive;
            if (!isAnyPopupShowed)
            {
                ShowItemFromQueue(popupsQueue[0]);
            }
        }
    }


    void ShowItemFromQueue(QueueItem queueItem)
    {
        RemoveFromQuque(queueItem);
        int maxOrder = int.MinValue;
        foreach (var popup in actualPopups)
        {
            maxOrder = Mathf.Max(popup.Order, maxOrder);
        }

        var popupPrefab = popupPrefabs[queueItem.popupType];
        var popupInstance = ObjectCreator.CreateObject<BasePopup>(popupPrefab.gameObject, root);
        popupInstance.Order = maxOrder + DeltaBetweenLayers;
        popupInstance.RootCanvas.worldCamera = Camera;
        popupInstance.RootCanvas.sortingLayerName = defaultSortingLayerName;
        popupInstance.BaseInitialize(queueItem.popupType, queueItem.onInitalizeCallback);
        popupInstance.Show(queueItem.isImmediately);
    }


    void RemoveFromQuque(QueueItem item)
    {
        popupsQueue.Remove(item);
    }

    #endregion



    #region Event handlers

    void BasePopup_OnHided(BasePopup popup)
    {
        TryShow();
        GameManager.Instance.UnlockTouches(popup);
        actualPopups.Remove(popup);
        Destroy(popup.gameObject);//ToPool
    }


    void BasePopup_OnStartHiding(BasePopup popup)
    {
        GameManager.Instance.LockTouches(popup);
    }


    void BasePopup_OnStartShowing(BasePopup popup)
    {
        actualPopups.Add(popup);
        GameManager.Instance.LockTouches(popup);
    }


    void BasePopup_OnShowed(BasePopup popup)
    {
        GameManager.Instance.UnlockTouches(popup);
    }

    #endregion
}
