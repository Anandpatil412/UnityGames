using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler instance;

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;

    public Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for(int i = 0;i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab,new Vector3(1000,1000,0),Quaternion.identity);
                //obj.SetActive(true);

                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);

        }

    }

    public GameObject SpawnFromPool(string tag)
    {
        if(!poolDictionary.ContainsKey(tag))
            return null;
       
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        //objectToSpawn.SetActive(true);

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

}
