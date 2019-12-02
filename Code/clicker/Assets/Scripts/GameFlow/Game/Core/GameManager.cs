using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using UnityEngine.EventSystems;


public class GameManager : SingletoneMonoBehaviour<GameManager>
{
    public static event Action<bool> OnTouchLockChanged;
    
    [SerializeField] TouchManager touchManager;

    [SerializeField] EventSystem eventSystem;


    public TouchManager TouchManager => touchManager;

    HashSet<System.Object> lockTouchObjects = new HashSet<System.Object>();

    bool isTouchLocked = false;

    public float Coins
    {
        get
        {
            return CustomPlayerPrefs.GetFloat("PLAYER_VALUTA", 0f);
        }
        set
        {
            CustomPlayerPrefs.SetFloat("PLAYER_VALUTA", value,  true);
        }
    }


    public bool IsTouchLocked 
    {
        get 
        {
            return isTouchLocked;
        }
        private set
        {
            if (isTouchLocked != value)
            {
                isTouchLocked = value;
                eventSystem.gameObject.SetActive(!value);
                OnTouchLockChanged?.Invoke(value);
            }
        }
    }



    void Awake() 
    {
        GuiManager.Instance.ScreenController.Show(ScreenType.Menu);
    }


    private void OnApplicationQuit() 
    {
        CustomPlayerPrefs.Save();    
    }

    
    #region Public methods

    public void LockTouches(System.Object target)
    {
        if (!lockTouchObjects.Contains(lockTouchObjects))
        {
            lockTouchObjects.Add(target);
        }
        IsTouchLocked = true;
    }


    public void UnlockTouches(System.Object target)
    {
        lockTouchObjects.RemoveWhere((obj) => obj == null);

        lockTouchObjects.Remove(target);

        if (lockTouchObjects.Count == 0)
        {
            IsTouchLocked = false;
        }
    }


    public void ForceUnlockTouches()
    {
        lockTouchObjects.Clear();
        IsTouchLocked = false;
    }

    #endregion
}
