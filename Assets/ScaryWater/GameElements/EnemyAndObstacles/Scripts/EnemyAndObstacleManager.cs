using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAndObstacleManager : MonoBehaviour
{
    static EnemyAndObstacleManager mInstance = null;

    public List<EnemyHandler> _listOfEnemyHandlers = new List<EnemyHandler>();
    public List<ObstacleHandler> _listOfObstacleHandlers = new List<ObstacleHandler>();
    public GameObject _batPrefab;
    public GameObject _hawkPrefab;
    public AggressiveEnemy _aggressiveEnemyScr;
    public List<AggressiveEnemy> _listOfAggressiveEnemyScr;
    public PlayerManager _playerManager;

    public static EnemyAndObstacleManager Instance
    {
        get { return mInstance; }
    }

    void Awake()
    {
		if (mInstance == null)
			mInstance = this;

		else if (mInstance != this)
			Destroy(this.gameObject);
    }

    public void ApplyDamageBasedOnObstacle(eObstacleType obsType)
    {
        if (FriendManager._bPlayerIsWithAFriend)
            return;
        
		GameObject HudObj = UICanvasHandler.Instance.GetActiveCanvasByName("HUDCanvas");
		GameplayAreaUIHandler tHudScr = HudObj.GetComponent<GameplayAreaUIHandler>();

		switch (obsType)
		{
			case eObstacleType.Cattail:
				if (ScoreHandler._OnScoreEventCallback != null)
					ScoreHandler._OnScoreEventCallback(eScoreType.Cattail);
                
                tHudScr._HealthBar.AddDamage(0.5f, _playerManager.PlayerDeathHandler);
				break;

			case eObstacleType.Horsetail:
				if (ScoreHandler._OnScoreEventCallback != null)
					ScoreHandler._OnScoreEventCallback(eScoreType.Horsetail);
                
				tHudScr._HealthBar.AddDamage(0.5f, _playerManager.PlayerDeathHandler);
				break;

			case eObstacleType.Iris:
				if (ScoreHandler._OnScoreEventCallback != null)
					ScoreHandler._OnScoreEventCallback(eScoreType.Iris);
                
				tHudScr._HealthBar.AddDamage(0.5f, _playerManager.PlayerDeathHandler);
				break;

			case eObstacleType.Pickerelweed:
				if (ScoreHandler._OnScoreEventCallback != null)
					ScoreHandler._OnScoreEventCallback(eScoreType.Pickerelweed);
                
				tHudScr._HealthBar.AddDamage(0.5f, _playerManager.PlayerDeathHandler);
				break;
		}

		//Debug.Log("obsType: " + obsType);

		if (MiniGameManager.Instance.AutoImplementedProperties_eMiniGameState == eMiniGameState.AvoidObstacles)
			MiniGameManager.Instance._iObstaclesTouched += 1;
	}

    public void ApplyDamageBasedOnEnemy(eEnemyType enemyType)
    {
		if (FriendManager._bPlayerIsWithAFriend)
			return;
        
		GameObject HudObj = UICanvasHandler.Instance.GetActiveCanvasByName("HUDCanvas");
		GameplayAreaUIHandler tHudScr = HudObj.GetComponent<GameplayAreaUIHandler>();

		switch (enemyType)
		{
			case eEnemyType.Bat:
				tHudScr._HealthBar.AddDamage(0.5f, _playerManager.PlayerDeathHandler);
				break;

			case eEnemyType.BlueHeron:
				tHudScr._HealthBar.AddDamage(0.5f, _playerManager.PlayerDeathHandler);
				break;

			case eEnemyType.Crab:
				tHudScr._HealthBar.AddDamage(0.5f, _playerManager.PlayerDeathHandler);
				break;

			case eEnemyType.GiantWaterBug:
				tHudScr._HealthBar.AddDamage(0.5f, _playerManager.PlayerDeathHandler);
				break;

			case eEnemyType.GreatEgret:
				tHudScr._HealthBar.AddDamage(0.5f, _playerManager.PlayerDeathHandler);
				break;

			case eEnemyType.Hawk:
				tHudScr._HealthBar.AddDamage(0.5f, _playerManager.PlayerDeathHandler);
				break;

			case eEnemyType.Raccoon:
				tHudScr._HealthBar.AddDamage(0.5f, _playerManager.PlayerDeathHandler);
				break;

			case eEnemyType.Snake:
				tHudScr._HealthBar.AddDamage(0.5f, _playerManager.PlayerDeathHandler);
				break;

			case eEnemyType.VenusFlytrap:
				tHudScr._HealthBar.AddDamage(0.5f, _playerManager.PlayerDeathHandler);
				break;
		}

		//Debug.Log("enemyType: " + enemyType);

		if (MiniGameManager.Instance.AutoImplementedProperties_eMiniGameState == eMiniGameState.AvoidEnemy)
			MiniGameManager.Instance._iEnemiesStunned += 1;
    }

    public void InstantiateBat(Vector3 playerPos)
    {
		if (_batPrefab != null & DayNightHandler.Instance._bNightTime)
		{
			GameObject goBat = (GameObject)Instantiate(_batPrefab);
			goBat.transform.SetParent(this.transform);
			goBat.transform.position = new Vector3(playerPos.x, 2.5f, playerPos.z + 250f);
			_aggressiveEnemyScr = (AggressiveEnemy)goBat.GetComponent<AggressiveEnemy>();
			_aggressiveEnemyScr._enemyAndObstacleManager = this;
			_listOfAggressiveEnemyScr.Add(_aggressiveEnemyScr);
		}
    }

	public void InstantiateHawk(Vector3 playerPos)
	{
        if (_hawkPrefab != null)
        {
			GameObject goHawk = (GameObject)Instantiate(_hawkPrefab);
			goHawk.transform.SetParent(this.transform);
			goHawk.transform.position = new Vector3(playerPos.x, 2.5f, playerPos.z + 250f);
			_aggressiveEnemyScr = (AggressiveEnemy)goHawk.GetComponent<AggressiveEnemy>();
			_aggressiveEnemyScr._enemyAndObstacleManager = this;
			_listOfAggressiveEnemyScr.Add(_aggressiveEnemyScr);
        }
	}
}
