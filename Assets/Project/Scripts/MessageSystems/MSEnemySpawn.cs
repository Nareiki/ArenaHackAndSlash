using System;
using System.Collections;
using System.Collections.Generic;

public struct MSEnemySpawn
{

    private static Dictionary<Type, List<Action<Object>>> _messageSubscribers = new Dictionary<Type, List<Action<Object>>>();

    public static void Subscribe<T>(Action<T> action)
    {
        Type messageType = typeof(T);
        if (!_messageSubscribers.ContainsKey(messageType))
        {
            _messageSubscribers[messageType] = new List<Action<object>>();
        }

        _messageSubscribers[messageType].Add(obj => action((T)obj));
    }

    public static void Unsubscribe<T>(Action<T> action)
    {
        Type messageType = typeof(T);
        if (_messageSubscribers.ContainsKey(messageType))
        {
            _messageSubscribers[messageType].Remove((obj => action((T)obj)));

            if (_messageSubscribers[messageType].Count == 0)
            {
                _messageSubscribers.Remove(messageType);
            }
        }
    }

    public static void Publish<T>(T message)
    {
        Type messageType = typeof(T);
        if (_messageSubscribers.ContainsKey(messageType))
        {
            foreach (var subscriber in _messageSubscribers[messageType])
            {
                subscriber?.Invoke(message);
            }
        }
    }

}
