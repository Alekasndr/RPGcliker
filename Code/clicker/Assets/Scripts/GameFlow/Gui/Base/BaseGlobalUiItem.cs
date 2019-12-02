using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[RequireComponent(typeof(Canvas))]
public class BaseGlobalUiItem<T> : MonoBehaviour
{
    #region Helpers

    class CanvasInfo
    {
        public int layerId;
        public int orderId;
    }

    #endregion



    #region Events

    public static event Action<T> OnStartShowing;
    public static event Action<T> OnShowed;
    public static event Action<T> OnStartHiding;
    public static event Action<T> OnHided;

    #endregion



    #region Fields

    [SerializeField] protected List<Canvas> canvases = null;
    [SerializeField] bool allowOverrideLayerName = true;

    Canvas rootCanvas = null;
    List<Action<T>> startShowingCallbacks = new List<Action<T>>();
    List<Action<T>> showedCallbacks = new List<Action<T>>();
    List<Action<T>> startHidingCallbacks = new List<Action<T>>();
    List<Action<T>> hidedCallbacks = new List<Action<T>>();
    Dictionary<Canvas, CanvasInfo> canvasesOnScreen = new Dictionary<Canvas, CanvasInfo>();
    int order = 0;

    #endregion



    #region Properties

    public int Order
    {
        get
        {
            return order;
        }
        set
        {
            if (order != value)
            {
                int delta = value - order;
                AffectOrder(delta);
            }
        }
    }


    public string CurrentSotringLayerName
    {
        get
        {
            return RootCanvas.sortingLayerName;
        }
        set
        {
            if (allowOverrideLayerName)
            {
                RootCanvas.sortingLayerName = value;
            }
        }
    }


    public Canvas RootCanvas => (rootCanvas ?? (rootCanvas = GetComponent<Canvas>()));

    #endregion



    #region Public methods

    public void AddStartShowingCallback(Action<T> item)
    {
        startShowingCallbacks.Add(item);
    }


    public void AddShowedCallback(Action<T> item)
    {
        showedCallbacks.Add(item);
    }


    public void AddStartHidingCallback(Action<T> item)
    {
        startHidingCallbacks.Add(item);
    }


    public void AddHidedCallback(Action<T> item)
    {
        hidedCallbacks.Add(item);
    }

    #endregion



    #region Protected methods

    protected void InitializeCanvasLogic()
    {
        foreach (var item in canvases)
        {
            canvasesOnScreen.Add(item, new CanvasInfo()
            {
                layerId = item.sortingLayerID,
                orderId = item.sortingOrder,
            });
        }
    }
    

    protected void AffectOrder(int delta)
    {
        foreach (var item in canvases)
        {
            item.sortingOrder += delta;
        }
    }


    protected void _WasStartShowing(T baseItem)
    {
        WasStartShowing();
        foreach (var item in startShowingCallbacks)
        {
            item?.Invoke(baseItem);
        }
        startShowingCallbacks.Clear();
        Send_OnStartShowing(baseItem);
    }


    protected void _WasShowed(T baseItem)
    {
        WasShowed();
        foreach (var item in showedCallbacks)
        {
            item?.Invoke(baseItem);
        }
        showedCallbacks.Clear();
        Send_OnShowed(baseItem);
    }


    protected void _WasStartHiding(T baseItem)
    {
        WasStartHiding();
        foreach (var item in startHidingCallbacks)
        {
            item?.Invoke(baseItem);
        }
        startHidingCallbacks.Clear();
        Send_OnStartHiding(baseItem);
    }


    protected void _WasHided(T baseItem)
    {
        WasHided();
        foreach (var item in hidedCallbacks)
        {
            item?.Invoke(baseItem);
        }
        hidedCallbacks.Clear();
        Send_OnHided(baseItem);

        Destroy(gameObject);
    }


    protected virtual void WasStartShowing()
    {
    }

    
    protected virtual void WasShowed()
    {
    }


    protected virtual void WasStartHiding()
    {
    }


    protected virtual void WasHided()
    {
    }

    #endregion



    #region Protected methods

    protected void Send_OnStartShowing(T item)
    {
        OnStartShowing?.Invoke(item);
    }

    protected void Send_OnShowed(T item)
    {
        OnShowed?.Invoke(item);
    }

    protected void Send_OnStartHiding(T item)
    {
        OnStartHiding?.Invoke(item);
    }

    protected void Send_OnHided(T item)
    {
        OnHided?.Invoke(item);
    }

    #endregion



    #region Editor

    [Sirenix.OdinInspector.Button("Fill canvases")]
    void FillCanvases()
    {
        canvases = GetComponentsInChildren<Canvas>().ToList();
    }

    #endregion
}
