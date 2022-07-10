using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OldStateMachine : MonoBehaviour
{
    [HideInInspector]
    public enum State
    {
        State1,
        State2,
        State3, 

        NUM_STATES
    }

    [Header("List of objects")]
    public List<string> objects = new List<string>();
    public List<Transform> spawns = new List<Transform>();
    public List<GameObject> objectsInScene = new List<GameObject>();

    [Header("States")]
    public State currentState;
    public State previousState;

    [Header("Spawning")]
    public bool startSpawning = false;
    public int maxObjects;
    public int waveCount;

    [Header("Waits")]
    public float spawnWait;
    public float startWait;
    public float waveWait;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip nextWaveSound;

    public Dictionary<State, Action> stateFsm = new Dictionary<State, Action>();

    TimerData timerData = new TimerData();

    // Start is called before the first frame update
    void Start()
    {
        AddToStateActions();
        waveCount = 0;
    }

    void AddToStateActions(){
        stateFsm.Add(State.State1, new Action(State1State));
        stateFsm.Add(State.State2, new Action(State2State));
        stateFsm.Add(State.State3, new Action(State3State));
    }

    public void SetState(State newState)
    {
        previousState = currentState;
        currentState = newState;
        stateFsm[currentState]();
    }

    void State1State()
    {
        Debug.Log("State 1");

        if (timerData.GetCurrentTime() >= spawnWait )
        {
            timerData.SetTimer(spawnWait);
            timerData.StartTimer();
        }
        else
        {
            SetState(State.State2);
        }
    }

    void State2State()
    {
        Debug.Log("State 2");

        if (objectsInScene.Count < maxObjects)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, spawns.Count);

                GameObject instance = ObjectPooler.Instance.RandomlySpawnFromPools(objects, spawns[randomIndex], spawns[randomIndex].rotation);

                instance.transform.parent = null;
                
                timerData.SetTimer(spawnWait);
                timerData.StartTimer();
                
            }
        }
        else
        {
            SetState(State.State3);
        }


    }

    void State3State()
    {
        Debug.Log("State 3");

        if (timerData.GetCurrentTime() > 0)
        {
            timerData.SetTimer(waveWait);
            timerData.StartTimer();
        }
        else
        {
            waveCount++;
            SetState(State.State1);
        }

    }

    // Update is called once per frame
    void Update()
    {
        stateFsm[currentState].Invoke();
    }
}
