using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard
{
    private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

    public Blackboard(Dictionary<string, object> dict)
    {
        _dataContext = dict;
    }

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
        {
            Debug.LogWarning($"Key: {key} not found!");
            T valueToAdd = default;
            _dataContext.Add(key, valueToAdd);
            return valueToAdd;
        }
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
