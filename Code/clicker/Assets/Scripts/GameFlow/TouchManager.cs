using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using pUI;

public class TouchManager : SimpleManager
{
    public static event Action<int, TouchInfo> OnTouchBegan;
    public static event Action<int, TouchInfo> OnTouchEnded;
    public static event Action<int, TouchInfo> OnTouchMove;

    Dictionary<int, TouchInfo> touchInfo = new Dictionary<int, TouchInfo>();

    HashSet<int> uiTouches = new HashSet<int>();
    HashSet<int> ingameTouches = new HashSet<int>();
    public bool IsAllowedClickUi => uiTouches.Count < 1;

    public int TouchesCount
    {
        get
        {
            return touchInfo.Count;
        }
    }
    


    void OnEnable() 
    {
        ScrollRect.OnPressUp += Button_OnPressDown;
        ScrollRect.OnPressUp += Button_OnPressUp;
    }

    void OnDisable() 
    {
        ScrollRect.OnPressUp -= Button_OnPressDown;
        ScrollRect.OnPressUp -= Button_OnPressUp;
    }


    public void TouchBegan(int fingerId, Vector2 screenPosition)
    {
        if (!touchInfo.ContainsKey(fingerId))
        {
            var fingerInfo = new List<TouchInfo.FingerPositionInfo>();
            fingerInfo.Add(new TouchInfo.FingerPositionInfo(screenPosition, Time.time));
            var currentTouchInfo = new TouchInfo()
            {
                fingerPositionsInfo = fingerInfo
            };
            touchInfo.Add(fingerId, currentTouchInfo);

            OnTouchBegan?.Invoke(fingerId, currentTouchInfo);
        }
    }


    public void TouchMoved(int fingerId, Vector2 screenPosition)
    {
        if (touchInfo.ContainsKey(fingerId))
        {
            var currentTouchInfo = touchInfo[fingerId];
            Vector2 prevPosition = currentTouchInfo.fingerPositionsInfo.Last().position;
            currentTouchInfo.fingerPositionsInfo.Add(new TouchInfo.FingerPositionInfo(screenPosition, Time.time));

            OnTouchMove?.Invoke(fingerId, touchInfo[fingerId]);
        }
    }


    public void TouchEnded(int fingerId)
    {
        if (touchInfo.ContainsKey(fingerId))
        {
            OnTouchEnded?.Invoke(fingerId, touchInfo[fingerId]);
            touchInfo.Remove(fingerId);
        }
    }


    public void EmulateDragBegan(int fingerId, Vector2 postion)
    {
        if (!touchInfo.ContainsKey(fingerId))
        {
            TouchBegan(fingerId, postion);
        }
    }


    public void EmulateDrag(int fingerId, Vector2 postion)
    {
        if (touchInfo.ContainsKey(fingerId))
        {
            TouchMoved(fingerId, postion);
        }
    }


    public void EmulateDragEnd(int fingerId)
    {
        if (touchInfo.ContainsKey(fingerId))
        {
            TouchEnded(fingerId);
        }
    }


    Dictionary<int, MonoBehaviour> pushedBehaviours = new Dictionary<int, MonoBehaviour>();
    void Button_OnPressDown(MonoBehaviour item, int finger)
    {
        uiTouches.Add(finger);
        pushedBehaviours.Add(finger, item);
    }
    

    void Button_OnPressUp(object button, int finger)
    {
        uiTouches.Remove(finger);
        pushedBehaviours.Remove(finger);
    }


}


public class TouchInfo
{
    public List<FingerPositionInfo> fingerPositionsInfo;

    public class FingerPositionInfo
    {
        public Vector2 position { get; private set; }
        public float time { get; private set; }

        public FingerPositionInfo(Vector2 position, float time)
        {
            this.position = position;
            this.time = time;
        }
    }
}
