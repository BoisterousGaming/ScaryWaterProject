using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eEnemyType
{
    None = 0,
    Snake,
    GiantWaterBug,
    BlueHeron,
    Crab,
    VenusFlytrap,
    Raccoon,
    GreatEgret,
    Bat,
    Hawk,
    DayTime
}

public enum eEnemyState
{
	Active = 0,
	Stunned
}

public class EnemyHandler : MonoBehaviour
{
    bool mbSkipChecking = false;

    public eEnemyType _eEnemyType = eEnemyType.None;
    public eEnemyState _eEnemyState = eEnemyState.Active;

    void OnEnable()
    {
        EnemyAndObstacleManager.Instance._listOfEnemyHandlers.Add(this);
    }

	void Update()
	{
		if (PlayerManager.Instance._playerHandler._tPlayerTransform.position.z - this.transform.position.z >= 20f)
            Destroy(this.gameObject);
	}

    void OnDestroy()
    {
        EnemyAndObstacleManager.Instance._listOfEnemyHandlers.Remove(this);
    }

    void OnTriggerEnter(Collider other)
    {
		if (other.CompareTag("Player"))
		{
            if (!mbSkipChecking)
            {
                mbSkipChecking = true;
                if (_eEnemyState.Equals(eEnemyState.Active))
                {
                    EnemyAndObstacleManager.Instance.ApplyDamageBasedOnEnemy(_eEnemyType);
                }

                else if (_eEnemyState.Equals(eEnemyState.Stunned))
                {
                    if (ScoreHandler._OnScoreEventCallback != null)
                    {
                        if (_eEnemyType.Equals(eEnemyType.Snake))
                            ScoreHandler._OnScoreEventCallback(eScoreType.SnakeKilling);

                        else if (_eEnemyType.Equals(eEnemyType.GiantWaterBug))
                            ScoreHandler._OnScoreEventCallback(eScoreType.GiantWaterBugKilling);

						else if (_eEnemyType.Equals(eEnemyType.BlueHeron))
							ScoreHandler._OnScoreEventCallback(eScoreType.BlueHeronKilling);

						else if (_eEnemyType.Equals(eEnemyType.Crab))
							ScoreHandler._OnScoreEventCallback(eScoreType.CrabKilling);

						else if (_eEnemyType.Equals(eEnemyType.VenusFlytrap))
							ScoreHandler._OnScoreEventCallback(eScoreType.VenusFlytrapKilling);

						else if (_eEnemyType.Equals(eEnemyType.Raccoon))
							ScoreHandler._OnScoreEventCallback(eScoreType.RaccoonKilling);

						else if (_eEnemyType.Equals(eEnemyType.GreatEgret))
							ScoreHandler._OnScoreEventCallback(eScoreType.GreatEgretKilling);

						else if (_eEnemyType.Equals(eEnemyType.Bat))
							ScoreHandler._OnScoreEventCallback(eScoreType.BatKilling);

						else if (_eEnemyType.Equals(eEnemyType.Hawk))
							ScoreHandler._OnScoreEventCallback(eScoreType.HawkKilling);
                    }
                }
            }

            Destroy(this.gameObject);
		}

        else if (other.CompareTag("Poison"))
        {
            if (_eEnemyState.Equals(eEnemyState.Active))
            {
				_eEnemyState = eEnemyState.Stunned;
				if (MiniGameManager.Instance.AutoImplementedProperties_eMiniGameState == eMiniGameState.StunEnemy)
					MiniGameManager.Instance._iEnemiesStunned += 1;
            }

            Destroy(other.gameObject);
        }
    }
}
