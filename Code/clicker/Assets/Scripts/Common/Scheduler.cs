using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Scheduler : MonoBehaviour
{
    #region Helpers

    public class Task
    {
        public System.Object Target { get; private set; }
        public System.Action CallBack { get; private set; }
        public float Delay { get; private set; }
        public float CompletedDelay { get; set; }


        public Task(System.Object target, System.Action callback, float delay)
        {
            Target = target;
            CallBack = callback;
            Delay = delay;
        }
    }

    #endregion



    #region Fields
    
    List<Task> tasksAndTime = new List<Task>();
    List<Task> unscheduledTasks = new List<Task>();

    static Scheduler instance = null;

    #endregion


    #region Properties

    public static Scheduler Instance => instance ?? (instance = new Scheduler());

    #endregion



    #region Static methods

    public static Task CallMethod(System.Object target, System.Action callback, float delay)
    {
        return Instance._CallMethod(target, callback, delay);
    }

    public static void UnscheduleByTask(Task task)
    {
        Instance._UnscheduleByTask(task);
    }


    public static void UnscheduleByTarget(System.Object target)
    {
        Instance._UnscheduleByTarget(target);
    }

    #endregion


    #region Private methods

    Task _CallMethod(System.Object target, System.Action callback, float delay)
    {
        Task task = null;

        if (target != null)
        {
            task = new Task(target, callback, delay);
            tasksAndTime.Add(task);
        }
        return task;
    }

    void _UnscheduleByTask(Task task)
    {
        if (task != null && tasksAndTime.Contains(task))
        {
            tasksAndTime.Remove(task);
        }
    }
   
    void _UnscheduleByTarget(System.Object target)
    {
        var task = tasksAndTime.Find((item) => item.Target == target);
        UnscheduleByTask(task);
    }

    #endregion


    public void CustomUpdate(float deltaTime)
    {
        unscheduledTasks.Clear();

        foreach (var keyPair in tasksAndTime)
        {
            keyPair.CompletedDelay += deltaTime;

            if (keyPair.CompletedDelay >= keyPair.Delay)
            {
                unscheduledTasks.Add(keyPair);
            }
        }

        unscheduledTasks.ForEach((item) => item?.CallBack());

        if (unscheduledTasks.Count > 0)
        {
            tasksAndTime = tasksAndTime.Except(unscheduledTasks).ToList();
        }
    }
}