﻿using UnityEngine;
using System;
using System.Collections;

public static class Utility
{
    public static T GetSafeComponent<T>(GameObject gameObject)
    {
        T component = gameObject.GetComponent<T>();
        if (component == null)
        {
            Debug.LogError("Component requested is null!");
        }
        return component;
    }

    public static T GetSafeComponent<T>(Transform transform)
    {
        T component = transform.GetComponent<T>();
        if (component == null)
        {
            Debug.LogError("Component requested is null!");
        }
        return component;
    }

    public static Coroutine Invoke(this MonoBehaviour monoBehaviour, Action action, float time)
    {
        return monoBehaviour.StartCoroutine(InvokeImpl(action, time));
    }

    private static IEnumerator InvokeImpl(Action action, float time)
    {
        yield return new WaitForSeconds(time);
        action();
    }

    public static void Swap<T>(ref T lhs, ref T rhs)
    {
        T temp;
        temp = lhs;
        lhs = rhs;
        rhs = temp;
    }

    public static void DestroyChildren(Transform parent)
    {
        foreach(Transform transform in parent)
        {
            GameObject.Destroy(transform.gameObject);
        }
    }

    public static void ChangeVector3X(ref Vector3 vec3, float x)
    {
        Vector3 newVec3 = vec3;
        newVec3.x = x;
        vec3 = newVec3;
    }
    
    public static void ChangeVector3Y(ref Vector3 vec3, float y)
    {
        Vector3 newVec3 = vec3;
        newVec3.y = y;
        vec3 = newVec3;
    }

    public static void ChangeVector3Z(ref Vector3 vec3, float z)
    {
        Vector3 newVec3 = vec3;
        newVec3.z = z;
        vec3 = newVec3;
    }
    
    public static string GetGameObjectPath(GameObject obj)
    {
        string path = "/" + obj.name;
        while (obj.transform.parent != null)
        {
            obj = obj.transform.parent.gameObject;
            path = "/" + obj.name + path;
        }
        return path;
    }
}