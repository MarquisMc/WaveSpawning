using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyTag;

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetKeyDown(KeyCode.Alpha2))
        {
            ObjectPooler.Instance.ReturnToPool(enemyTag, this.gameObject);
        }
    }
}
