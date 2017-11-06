using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressiveEnemyTriggerScr : MonoBehaviour 
{
    bool mbShouldDestroy = false;

    void Update()
    {
        if (mbShouldDestroy)
            Destroy(this.gameObject);  
    }

    void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
            if (transform.CompareTag("BatInstantiation"))
                EnemyAndObstacleManager.Instance.InstantiateBat(PlayerManager.Instance._playerHandler._tPlayerTransform.position);

            else if (transform.CompareTag("HawkInstantiation"))
                EnemyAndObstacleManager.Instance.InstantiateHawk(PlayerManager.Instance._playerHandler._tPlayerTransform.position);

            mbShouldDestroy = true;
		}
	}
}
