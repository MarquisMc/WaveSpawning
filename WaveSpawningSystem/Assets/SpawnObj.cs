using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObj : MonoBehaviour
{
    OldStateMachine waveSpawner;

    private void OnEnable() 
    {
        waveSpawner = FindObjectOfType<OldStateMachine>();
        waveSpawner.objectsInScene.Add(this.gameObject);
    }

    private void OnDisable() 
    {
        waveSpawner.objectsInScene.Remove(this.gameObject);
    }
}
