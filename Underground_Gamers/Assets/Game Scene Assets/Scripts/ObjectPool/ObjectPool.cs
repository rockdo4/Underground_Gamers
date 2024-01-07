using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool<T> where T : Object
{
    private Queue<T> pool = new Queue<T>();
    private IObjectFactory<T> objectFactory;
    private int createCount = 0;

    public ObjectPool(IObjectFactory<T> factory, int initSize)
    {
        objectFactory = factory;
        createCount = initSize;
        CreateObjects();
    }

    public void CreateObjects()
    {
        for (int i = 0; i < createCount; ++i)
        {
            T obj = objectFactory.CreateObject();
            obj.GameObject().SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public T GetObjectFromPool()
    {
        if (pool.Count == 0)
        {
            CreateObjects();
        }

        if (pool.Count > 0)
        {
            T obj = pool.Dequeue();
            obj.GameObject().SetActive(true);
            return obj;
        }
        else
        {
            return null;
        }
    }

    public void ReturnObjectToPool(T obj)
    {
        obj.GameObject().SetActive(false);
        pool.Enqueue(obj);
    }
}
