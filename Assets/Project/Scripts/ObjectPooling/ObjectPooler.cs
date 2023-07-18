using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem
{
    public GameObject ObjectToPool;

    public int AmountToPool;

    public bool ShouldExpand = true;
}

public class ObjectPooler : MonoBehaviour
{

    public static ObjectPooler SharedInstance;

    private List<GameObject> _pooledObjects;

    private List<ObjectPoolItem> _itemsToPool;

    private void Awake()
    {
        SharedInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _pooledObjects = new List<GameObject>();

        foreach (var item in _itemsToPool)
        {
            for (int i = 0; i < item.AmountToPool; i++)
            {
                GameObject obj = Instantiate(item.ObjectToPool);
                obj.SetActive(false);
                _pooledObjects.Add(obj);

            }
        }
    }

    public GameObject GetPooledObject(string tag)
    {
        //1
        for (int i = 0; i < _pooledObjects.Count; i++)
        {
            if (!_pooledObjects[i].activeInHierarchy || _pooledObjects[i].tag == tag)
            {
                return _pooledObjects[i];
            }
        }
        foreach (var poolItem in _itemsToPool)
        {
            if (poolItem.ObjectToPool.tag == tag)
            {
                if (poolItem.ShouldExpand)
                {
                    GameObject obj = Instantiate(poolItem.ObjectToPool);
                    obj.SetActive(false);
                    _pooledObjects.Add(obj);
                    return obj;
                }
            }
        }

        return null;
    }
}
