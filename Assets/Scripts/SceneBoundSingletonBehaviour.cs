using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBoundSingletonBehaviour<T> : MonoBehaviour where T : Component
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance != null) 
                return instance;
                
            instance = FindObjectOfType<T>();
            return instance;
        }
    }

    public virtual void Awake()
    {
        if (instance == null)
            instance = this as T;
    }
}
