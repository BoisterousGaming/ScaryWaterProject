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
    GameObject mTempGameObject;
    bool mbSkipChecking = false;
    bool mbShouldDestroy = false;
    bool mbShouldDestroyTempGO = false;

    public eEnemyType _eEnemyType = eEnemyType.None;
    public eEnemyState _eEnemyState = eEnemyState.Active;

    void OnEnable()
    {
        EnemyAndObstacleManager.Instance._listOfEnemyHandlers.Add(this);
    }

    void Start()
    {
        DetectEnemyType();
    }

    void Update()
	{
		if (PlayerManager.Instance._playerHandler._tPlayerTransform.position.z - this.transform.position.z >= 20f)
            Destroy(this.gameObject);

        if (mbShouldDestroy)
            Destroy(this.gameObject);

        if (mbShouldDestroyTempGO & mTempGameObject != null)
        {
            mbShouldDestroyTempGO = false;
            Destroy(mTempGameObject);
        }
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
                CEffectsPlayer.Instance.Play("EnemyCollision");

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

            mbShouldDestroy = true;
		}

        else if (other.CompareTag("Poison"))
        {
            if (_eEnemyState.Equals(eEnemyState.Active))
            {
				_eEnemyState = eEnemyState.Stunned;
				if (MiniGameManager.Instance._eMiniGameState == eMiniGameState.StunEnemy)
					MiniGameManager.Instance._iEnemiesStunned += 1;
            }

            mTempGameObject = other.gameObject;
            mbShouldDestroyTempGO = true;
        }
    }

    void DetectEnemyType(string audioBlueHeron = "BlueHeronSound", string audioSnake = "SnakeSound")
    {
        if (this._eEnemyType == eEnemyType.BlueHeron)
            CEffectsPlayer.Instance.Play(audioBlueHeron);

        else if (this._eEnemyType == eEnemyType.Snake)
            CEffectsPlayer.Instance.Play(audioSnake);
    }
}
