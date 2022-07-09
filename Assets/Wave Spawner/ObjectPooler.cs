using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    /// <summary>
    /// ObjectPooler.Instance.SpawnFromPool("Tag Name", position, rotation);
    /// 
    /// </summary>

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    #region Singleton

    public static ObjectPooler Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    TimerData timerData = new TimerData();

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    public List<int> ObjInPoolSize = new List<int>();

    public Queue<GameObject> objectPool;

    // Start is called before the first frame update
    void Start()
    {
        PopulateThePool();
    }

    void PopulateThePool () 
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            objectPool = new Queue<GameObject>();

            ObjInPoolSize.Add(pool.size);
            GameObject holder = new GameObject(pool.tag + " Holder");

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.transform.parent = holder.transform;
                obj.name = pool.tag;
                obj.SetActive(false);
                objectPool.Enqueue(obj);
                
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool (string tag, Transform position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't excist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.transform.position = position.position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.transform.parent = null;

        poolDictionary[tag].Enqueue(objectToSpawn);

        Debug.Log("Spawned " + objectToSpawn.name + " from pool " + tag);

        objectToSpawn.SetActive(true);

        return objectToSpawn;
    }

    // return object to pool
    public void ReturnToPool(string tag, GameObject objectToReturn)
    {
        if (poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't excist.");
            return;
        }

        poolDictionary[tag].Enqueue(objectToReturn);
        objectToReturn.SetActive(false);
    }

    public void ReturnToPoolOnTrigger(string tag, GameObject objectToReturn, Collider other)
    {
        if (poolDictionary.ContainsKey(tag) && other.gameObject.tag == tag)
        {
            poolDictionary[tag].Enqueue(objectToReturn);
            objectToReturn.SetActive(false);
        }
        else if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't excist.");
        }
    }

    public void ReturnToPoolOnTimer(string tag, GameObject objectToReturn, float time)
    {
        if (timerData.GetCurrentTime() != time || timerData.GetCurrentTime() == 0)
        {
            timerData.SetTimer(time);
            timerData.StartTimer();
        }

        if (poolDictionary.ContainsKey(tag) && timerData.GetCurrentTime() <= 0)
        {
            poolDictionary[tag].Enqueue(objectToReturn);
            objectToReturn.SetActive(false);
        }
        else if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't excist.");
        }
    }
}
