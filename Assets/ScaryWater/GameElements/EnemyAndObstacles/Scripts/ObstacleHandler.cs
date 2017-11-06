using System.Collections;
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
    bool mbShouldDestroy = false;

    public eObstacleType _eObstacleType = eObstacleType.None;

    void OnEnable()
    {
        EnemyAndObstacleManager.Instance._listOfObstacleHandlers.Add(this);
    }

    void Update()
    {
        if (PlayerManager.Instance._playerHandler._tPlayerTransform.position.z - this.transform.position.z >= 20f)
            Destroy(this.gameObject);

        if (mbShouldDestroy)
            Destroy(this.gameObject);
    }

    void OnDestroy()
    {
        EnemyAndObstacleManager.Instance._listOfObstacleHandlers.Remove(this);    
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!mbSkipChecking)
            {
                mbSkipChecking = true;
				EnemyAndObstacleManager.Instance.ApplyDamageBasedOnObstacle(_eObstacleType);
                Debug.Log("Name of the obstacle: " + this.gameObject.name + ", Position: " + this.transform.position);
            }
            mbShouldDestroy = true;
        }
    }
}
