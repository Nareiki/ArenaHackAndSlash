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

    public List<GameObject> PooledObjects;

    public List<ObjectPoolItem> itemsToPool;

    private void Awake()
    {
        SharedInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        PooledObjects = new List<GameObject>();

        foreach (var item in itemsToPool)
        {
            for (int i = 0; i < item.AmountToPool; i++)
            {
                GameObject obj = (GameObject)Instantiate(item.ObjectToPool);
                obj.SetActive(false);
                PooledObjects.Add(obj);

            }
        }
    }

    public GameObject GetPooledObject(string tag)
    {
        //1
        for (int i = 0; i < PooledObjects.Count; i++)
        {
            if (!PooledObjects[i].activeInHierarchy || PooledObjects[i].tag == tag)
            {
                return PooledObjects[i];
            }
        }
        foreach (var poolItem in itemsToPool)
        {
            if (poolItem.ObjectToPool.tag == tag)
            {
                if (poolItem.ShouldExpand)
                {
                    GameObject obj = (GameObject)Instantiate(poolItem.ObjectToPool);
                    obj.SetActive(false);
                    PooledObjects.Add(obj);
                    return obj;
                }
            }
        }

        return null;
    }
}
