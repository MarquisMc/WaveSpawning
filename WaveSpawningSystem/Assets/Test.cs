using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public List<string> objects = new List<string>(); // the enemies that will spawn in the scene

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetKeyDown(KeyCode.Alpha1))
        {
            ObjectPooler.Instance.RandomlySpawnFromPools(objects, this.transform , new Quaternion(0, 0, 0, 0));
        }
    }


}
