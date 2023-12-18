using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private GameObject prefab;
    private Queue<GameObject> objectPool = new Queue<GameObject>();

    public ObjectPool(GameObject prefab, int initialSize)
    {
        this.prefab = prefab;

        // Belirtilen ba�lang�� boyutunda bir obje havuzu olu�turulur
        for (int i = 0; i < initialSize; i++)
        {
            CreateObject();
        }
    }

    public GameObject GetObject()
    {
        // E�er havuzda kullan�lmayan bir obje varsa, onu al�r; yoksa yeni bir obje olu�turur
        if (objectPool.Count > 0)
        {
            GameObject pooledObject = objectPool.Dequeue();
            return pooledObject;
        }
        else
        {
            return CreateObject();
        }
    }

    public void ReturnObject(GameObject obj)
    {
        
        // Kullan�lm�� objeyi havuza geri koyar
        obj.SetActive(false);
        objectPool.Enqueue(obj);
    }

    private GameObject CreateObject()
    {
        // Yeni bir obje olu�turur ve havuza ekler
        GameObject newObj = Object.Instantiate(prefab);
        newObj.SetActive(false);
        objectPool.Enqueue(newObj);
        return newObj;
    }
}
