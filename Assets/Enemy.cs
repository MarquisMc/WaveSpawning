using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetKeyDown(KeyCode.Alpha2))
        {
            ObjectPooler.Instance.ReturnToPool("Enemies", this.gameObject);
        }
    }
}
