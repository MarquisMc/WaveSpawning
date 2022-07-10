using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    List<int> ObjInPoolSize = new List<int>();

    public Queue<GameObject> objectPool;

    List<GameObject> holders = new List<GameObject>();

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
            holders.Add(new GameObject(pool.tag));

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.transform.parent = holders.Find(x => x.name == pool.tag).transform;
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
        
        objectToSpawn.SetActive(false);
        objectToSpawn.transform.position = position.position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.transform.parent = null;
        objectToSpawn.SetActive(true);
        
        poolDictionary[tag].Enqueue(objectToSpawn);

        Debug.Log("Spawned " + objectToSpawn.name + " from pool " + tag);

        return objectToSpawn;
    }

    public GameObject RandomlySpawnFromPools (List<string> tags, Transform position, Quaternion rotation)
    {
        if (tags.Count == 0)
        {
            Debug.LogWarning("No tags were given to randomly spawn from.");
            return null;
        }

        int rand = Random.Range(0, tags.Count);
        string tag = tags[rand];
        return SpawnFromPool(tag, position, rotation);
    }

    // return object to pool
    public void ReturnToPool(string tag, GameObject objectToReturn)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't excist.");
            return;
        }

        poolDictionary[tag].Enqueue(objectToReturn);
        objectToReturn.SetActive(false);
        objectToReturn.transform.parent = holders.Find(x => x.name == tag).transform;
    }

    public void ReturnToPoolOnTrigger(string tag, GameObject objectToReturn, Collider other)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't excist.");
            return;
        }

        if (other.gameObject.tag == tag) 
        {
            poolDictionary[tag].Enqueue(objectToReturn);
            objectToReturn.SetActive(false);
        }
    }

    public void ReturnToPoolOnTimer(string tag, GameObject objectToReturn, float time)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't excist.");
            return;
        }

        if (timerData.GetCurrentTime() != time || timerData.GetCurrentTime() == 0)
        {
            timerData.SetTimer(time);
            timerData.StartTimer();
        }

        if (timerData.GetCurrentTime() <= 0)
        {
            poolDictionary[tag].Enqueue(objectToReturn);
            objectToReturn.SetActive(false);
        }
    }
}
