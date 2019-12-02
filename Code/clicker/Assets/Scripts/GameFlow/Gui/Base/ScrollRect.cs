using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;


namespace pUI
{
    public class ScrollRect : UnityEngine.UI.ScrollRect 
    {
        public static event Action<ScrollRect, int> OnPressDown;
        public static event Action<ScrollRect, int> OnPressUp;
        private bool routeToParent = false;
        bool IsAllowedClickButton => GameManager.Instance.TouchManager.IsAllowedClickUi;
    
    
        private void DoForParents<T>(Action<T> action) where T:IEventSystemHandler
        {
            Transform parent = transform.parent;
            while(parent != null) {
                foreach(var component in parent.GetComponents<Component>()) {
                    if(component is T)
                        action((T)(IEventSystemHandler)component);
                }
                parent = parent.parent;
            }
        }
    
    
        public override void OnInitializePotentialDrag (PointerEventData eventData)
        {
            DoForParents<IInitializePotentialDragHandler>((parent) => { parent.OnInitializePotentialDrag(eventData); });
            base.OnInitializePotentialDrag (eventData);
        }
    

        public override void OnDrag (UnityEngine.EventSystems.PointerEventData eventData)
        {
            if (IsAllowedClickButton)
            {
                if( routeToParent )
                    DoForParents<IDragHandler>((parent) => { parent.OnDrag(eventData); });
                else
                    base.OnDrag (eventData);
            }
        }
        

        public override void OnBeginDrag (UnityEngine.EventSystems.PointerEventData eventData)
        {
            if (IsAllowedClickButton)
            {
                if(!horizontal && Math.Abs (eventData.delta.x) > Math.Abs (eventData.delta.y))
                    routeToParent = true;
                else if(!vertical && Math.Abs (eventData.delta.x) < Math.Abs (eventData.delta.y))
                    routeToParent = true;
                else
                    routeToParent = false;
        
                if(routeToParent)
                    DoForParents<IBeginDragHandler>((parent) => { parent.OnBeginDrag(eventData); });
                else
                {
                    OnPressDown?.Invoke(this, eventData.pointerId);
                    base.OnBeginDrag (eventData);
                }
            }
        }
    
    
        public override void OnEndDrag (UnityEngine.EventSystems.PointerEventData eventData)
        {
            if (IsAllowedClickButton)
            {
                if(routeToParent)
                    DoForParents<IEndDragHandler>((parent) => { parent.OnEndDrag(eventData); });
                else
                {
                    OnPressUp?.Invoke(this, eventData.pointerId);
                    base.OnEndDrag (eventData);
                }
                routeToParent = false;
            }
        }
    }
}