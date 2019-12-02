using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchesHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler
{    
    #region IPointerDownHandlerMethods

    public void OnPointerDown(PointerEventData data)
    {
        GameManager.Instance.TouchManager.TouchBegan(data.pointerId, data.position);
    }
    
    #endregion
    
    
    
    #region IDragHandlerMethods
    
    public void OnDrag(PointerEventData data)
    {
        GameManager.Instance.TouchManager.TouchMoved(data.pointerId, data.position);        
    }
    
    #endregion
    
    
    
    #region IPointerUpHandlerMethods
    
    public void OnPointerUp(PointerEventData data)
    {
        GameManager.Instance.TouchManager.TouchEnded(data.pointerId);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // throw new NotImplementedException();
    }

    #endregion
}