using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public abstract class BaseGuiController<ControllerItem, BaseItem, Type> : SerializedMonoBehaviour where ControllerItem : MonoBehaviour
{
    [SerializeField] protected Transform root;
    [SerializeField] protected string defaultSortingLayerName;
    protected Camera Camera { get; private set; }


    public void Initialize(Camera uiCamera)
    {
        Camera = uiCamera;
    }

    public abstract void Show(Type type,
                     bool isImmediately = false,
                     Action<BaseItem> initializeCallback = null);
                     

}
