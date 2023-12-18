using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private GameObject prefab;
    private Queue<GameObject> objectPool = new Queue<GameObject>();

    public ObjectPool(GameObject prefab, int initialSize)
    {
        this.prefab = prefab;

        // Belirtilen baþlangýç boyutunda bir obje havuzu oluþturulur
        for (int i = 0; i < initialSize; i++)
        {
            CreateObject();
        }
    }

    public GameObject GetObject()
    {
        // Eðer havuzda kullanýlmayan bir obje varsa, onu alýr; yoksa yeni bir obje oluþturur
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
        
        // Kullanýlmýþ objeyi havuza geri koyar
        obj.SetActive(false);
        objectPool.Enqueue(obj);
    }

    private GameObject CreateObject()
    {
        // Yeni bir obje oluþturur ve havuza ekler
        GameObject newObj = Object.Instantiate(prefab);
        newObj.SetActive(false);
        objectPool.Enqueue(newObj);
        return newObj;
    }
}
