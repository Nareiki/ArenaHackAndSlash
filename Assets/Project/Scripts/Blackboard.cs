using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard
{
    private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

    public void SetData(string key, object value)
    {
        _dataContext[key] = value;
    }

    public T GetData<T>(string key)
    {
        T val = default;

        if (_dataContext.ContainsKey(key))
            return (T)_dataContext[key];
        else
            Debug.LogError($"Key: {key} not found!");

        return val;
    }

    public object GetData(string key)
    {
        object val = null;

        if (_dataContext.TryGetValue(key, out val))
            return val;

        return val;
    }

    public bool ClearData(string key)
    {
        bool cleared = false;
        if (_dataContext.ContainsKey(key))
        {
            _dataContext.Remove(key);
            return true;
        }

        return cleared;
    }

    public bool ResetData(string key)
    {
        bool cleared = false;
        if (_dataContext.ContainsKey(key))
        {
            _dataContext[key] = default;
            return true;
        }
        return cleared;
    }
}
