using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;


public class GuiVisualController : SerializedMonoBehaviour//, IGuiAnimateable
{
    class ShowInfo
    {
        public IGuiAnimateable item = null;
        public float showDelay = 0f;
        public float showDuration = 0f;
        public float hideDelay = 0f;
        public float hideDuration = 0f;
    }

    // public event Action<IGuiAnimateable> OnStateChanged = null;

    [SerializeField] List<ShowInfo> items = new List<ShowInfo>();

    // ShowingState showingState;

    // public ShowingState ShowingState => showingState;

    HashSet<IGuiAnimateable> showBlockers = new HashSet<IGuiAnimateable>();
    HashSet<IGuiAnimateable> hideBlockers = new HashSet<IGuiAnimateable>();


    void OnEnable() 
    {
        // foreach (var i in items)
        // {
        //     i.item.OnStateChanged += Item_OnStateChanged;
        // }
    }


    void OnDisable()
    {
        // foreach (var i in items)
        // {
        //     i.item.OnStateChanged -= Item_OnStateChanged;
        // }
    }


    public void Hide(bool isImmediately, Action onShowed)
    {
        showBlockers.Clear();
        hideBlockers.Clear();

        foreach (var i in items)
        {
            i.item.OutDuration = i.hideDuration;
            showBlockers.Add(i.item);
        }

        foreach(var i in items)
        {
            if (Mathf.Approximately(i.hideDelay, 0f) || i.hideDelay < 0f)
            {
                ProcessHide(i);
            }
            else
            {
                Scheduler.CallMethod(this, () =>
                {
                    ProcessHide(i);
                }, i.hideDelay);
            }
        }



        void ProcessHide(ShowInfo i)
        {
            i.item.Hide(isImmediately, () =>
            {
                hideBlockers.Remove(i.item);
                if (hideBlockers.Count == 0)
                {
                    onShowed?.Invoke();
                }
            });
        }

    }


    public void Show(bool isImmediately, Action onShowed)
    {
        showBlockers.Clear();
        hideBlockers.Clear();

        foreach (var i in items)
        {
            i.item.InDuration = i.showDuration;
            showBlockers.Add(i.item);
        }

        foreach(var i in items)
        {
            if (Mathf.Approximately(i.showDelay, 0f) || i.showDelay < 0f)
            {
                ProcessShow(i);
            }
            else
            {
                Scheduler.CallMethod(this, () =>
                {
                    ProcessShow(i);
                }, i.showDelay);
            }
        }

        void ProcessShow(ShowInfo i)
        {
            i.item.Show(isImmediately, () =>
            {
                showBlockers.Remove(i.item);
                if (showBlockers.Count == 0)
                {
                    onShowed?.Invoke();
                }
            });
        }
    }

    // void Item_OnStateChanged(IGuiAnimateable item)
    // {
    //     var newState = item.ShowingState;

    // }
}
