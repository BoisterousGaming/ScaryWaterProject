using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum eAggrEnemyState
{
    MovingTowardPlayer = 0,
    ChangeLane,
    MovingStraight
}

public class AggressiveEnemy : MonoBehaviour
{
    EnemyHandler mEnemyHandler;
    eAggrEnemyState meAggrEnemyState = eAggrEnemyState.MovingTowardPlayer;
    Vector3 mvEndPosition;
    float mfNormalMovingSpeed = 10f;
    float mfLaneChangingSpeed = 25f;
    Transform mtTransform;

    public EnemyAndObstacleManager _enemyAndObstacleManager;

    void Awake()
    {
        mtTransform = transform;  
        mEnemyHandler = GetComponent<EnemyHandler>();
    }
	
	void FixedUpdate () 
    {
        EnemyMovementHandler();
        StartCoroutine(IChangeEnemyState());
	}

	IEnumerator IChangeEnemyState()
	{
		if (mtTransform.position.z - PlayerManager.Instance._playerHandler._tPlayerTransform.position.z < 40f)
			meAggrEnemyState = eAggrEnemyState.MovingStraight;

		else
		{
			if (PlayerManager.Instance._playerHandler._bSwipedLeftOrRight)
				meAggrEnemyState = eAggrEnemyState.ChangeLane;

			else
			{
				yield return new WaitForSeconds(3f);
				meAggrEnemyState = eAggrEnemyState.MovingTowardPlayer;
			}
		}
	}

    void EnemyMovementHandler()
    {
        switch (meAggrEnemyState)
        {
            case eAggrEnemyState.MovingTowardPlayer:
                EnemyMoving(mfNormalMovingSpeed, PlayerManager.Instance._playerHandler._tPlayerTransform.position.x, 0f);
				break;

            case eAggrEnemyState.ChangeLane:
                EnemyMoving(mfLaneChangingSpeed, PlayerManager.Instance._playerHandler._tPlayerTransform.position.x, mtTransform.position.z - 2f);
                break;

            case eAggrEnemyState.MovingStraight:
                EnemyMoving(mfNormalMovingSpeed, mtTransform.position.x, 0f);
                break;
        }
    }

    void EnemyMoving(float speed, float xPos, float zPos)
    {
        mvEndPosition.x = xPos;
        mvEndPosition.y = Mathf.Clamp(PlayerManager.Instance._playerHandler._tPlayerTransform.position.y, DataHandler._fPlayerAutoJumpHeight, DataHandler._fPlayerSpiderInitiatedJumpHeight);
        mvEndPosition.z = zPos;
		mtTransform.position = Vector3.MoveTowards(mtTransform.position, mvEndPosition, speed * Time.deltaTime);
    }
}
