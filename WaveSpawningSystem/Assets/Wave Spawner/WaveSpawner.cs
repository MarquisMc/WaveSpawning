using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [Tooltip("the objects that will spawn in the scene")]
    public List<string> objects = new List<string>();

    [Tooltip("object positions that the enemies spawn at")]
    public List<Transform> spawns = new List<Transform>();

    [Tooltip("the objects in the scene")]
    public List<GameObject> objectsInScene = new List<GameObject>();

    [Tooltip("start spawning objects")]
    public bool startSpawning = false;

    [Tooltip("amount of enemies in every wave")]
    public int objectCount;

    [Tooltip(("max number of enemies per wave"))]
    public int maxObjects;

    [Tooltip("counts the waves that have been spawned")]
    public int waveCount;

    [Tooltip("spawn an object every x seconds")]
    public float spawnWait;

    [Tooltip("wait x seconds before start spawning waves")]
    public float startWait;

    [Tooltip("wait x seconds before spawning the next wave")]
    public float waveWait;

    [Tooltip("the wave spawner audio source")]
    public AudioSource audioSource;

    [Tooltip("the wave spawner for every wave audio clip")]
    public AudioClip nextWaveSound; 

    // Start is called before the first frame update
    void Start()
    {
        waveCount = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        StartCoroutine(Spawner()); // Coroutines can get IEnumerators to function
    }

    IEnumerator Spawner() // use IEnumerators when you want have something to wait base on time
    {
        yield return new WaitForSeconds(startWait); // wait for x seconds before starting the wave spawner

            if (objectsInScene.Count < maxObjects)
            {
                for (int i = 0; i < objectCount; i++)
                {
                    // get a random number between 0 and the number of objects in the objects array
                    int rand = Random.Range(0, objects.Count);

                    // get a random number between 0 and the number of spawns in the objects array
                    int randS = Random.Range(0, spawns.Count);

                    // spawn the object at the random spawn position
                    GameObject instance = ObjectPooler.Instance.RandomlySpawnFromPools(objects, spawns[randS], spawns[randS].rotation);

                    instance.transform.parent = null;

                    yield return new WaitForSeconds(spawnWait); // wait for x seconds before spawning the next object
                }
            }

            yield return new WaitForSeconds(waveWait); // wait for x seconds before spawning the next wave
            
            waveCount++;
            maxObjects++;
            objectCount++;

            if (nextWaveSound != null)
            {
                audioSource.PlayOneShot(nextWaveSound);
            }
    }
}
