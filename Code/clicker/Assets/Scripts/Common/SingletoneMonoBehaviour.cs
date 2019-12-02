using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SingletoneMonoBehaviour<T> : SerializedMonoBehaviour where T : MonoBehaviour
{
    static T instance;
    public static T Instance 
    {
        get
        {
            if (instance == null)
            {
                instance = (FindObjectOfType(typeof(T)) as T);
                if (instance is IInitialziable init)
                {
                    init.Initialize();
                }
            }
            return instance;
        }
    }
}


public interface IInitialziable
{
    void Initialize();
}