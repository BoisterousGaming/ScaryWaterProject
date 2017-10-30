﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eObstacleType
{
    None = 0,
	Cattail,
	Pickerelweed,
	Iris,
	Horsetail
}

public class ObstacleHandler : MonoBehaviour 
{
    bool mbSkipChecking = false;

    public eObstacleType _eObstacleType = eObstacleType.None;

    void OnEnable()
    {
        EnemyAndObstacleManager.Instance._listOfObstacleHandlers.Add(this);
    }

    void OnDestroy()
    {
        EnemyAndObstacleManager.Instance._listOfObstacleHandlers.Remove(this);    
    }

    void Update()
    {
		if (PlayerManager.Instance._playerHandler._tPlayerTransform.position.z - this.transform.position.z >= 20f)
			Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!mbSkipChecking)
            {
                mbSkipChecking = true;
				EnemyAndObstacleManager.Instance.ApplyDamageBasedOnObstacle(_eObstacleType);
            }
            Destroy(this.gameObject);
        }
    }
}
