using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableAndFoodManager : MonoBehaviour
{
    Vector3 mvTempVector;
    float mfPlayerCoinMagnetLifeDurationInSecond;
	float mfTempPlayerCoinMagnetLifeDurationInSecond;
    bool mbMagnetIsAttachedToPlayer;
    GameObject[] mArrOfChilds;
    static CollectableAndFoodManager mInstance;

    public List<CollectableHandler> _listOfCollectableHandlers = new List<CollectableHandler>();
    public List<FoodHandler> _listOfFoodHandlers = new List<FoodHandler>();
    public PlayerManager _playerManager;
    public float[] _arrOfMagnetTime;
	public ParticleSystem _ParticleCoins;

    public static CollectableAndFoodManager Instance
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

    void Update()
    {
		if (mbMagnetIsAttachedToPlayer)
			MagnetHandler();
    }

    public void ApplyHealthBasedOnFood(eFoodType foodType)
    {
		switch (foodType)
		{
			case eFoodType.Bug:
				if (ScoreHandler._OnScoreEventCallback != null)
					ScoreHandler._OnScoreEventCallback(eScoreType.Bug);
                
                _playerManager._BarProgressSpriteScr.AddHealth(0.1f, _playerManager.PlayerDeathHandler);
				break;

			case eFoodType.Firefly:
				if (ScoreHandler._OnScoreEventCallback != null)
					ScoreHandler._OnScoreEventCallback(eScoreType.Firefly);
                
                _playerManager._BarProgressSpriteScr.AddHealth(0.1f, _playerManager.PlayerDeathHandler);
				break;

			case eFoodType.Insect:
				if (ScoreHandler._OnScoreEventCallback != null)
					ScoreHandler._OnScoreEventCallback(eScoreType.Insect);
                
                _playerManager._BarProgressSpriteScr.AddHealth(0.1f, _playerManager.PlayerDeathHandler);
				break;

			case eFoodType.Worm:
				if (ScoreHandler._OnScoreEventCallback != null)
					ScoreHandler._OnScoreEventCallback(eScoreType.Worm);
                
                _playerManager._BarProgressSpriteScr.AddHealth(0.1f, _playerManager.PlayerDeathHandler);
				break;
		}

		if (MiniGameManager.Instance.AutoImplementedProperties_eMiniGameState == eMiniGameState.EatFood || MiniGameManager.Instance.AutoImplementedProperties_eMiniGameState == eMiniGameState.AvoidFood)
			MiniGameManager.Instance._iFoodsEaten += 1;
    }

    public void CompareCollectableElements(CollectableHandler scr)
    {
        //Debug.Log("Collectable type: " + scr._eCollectableType + ", Collectable pos: " + scr.transform.position + ", Collectable name: " + scr.gameObject.name);
		if (scr._eCollectableType == eCollectableType.StartCoin)
        {
			if (ScoreHandler._OnScoreEventCallback != null)
				ScoreHandler._OnScoreEventCallback(eScoreType.StartCoin);
        }

		else if (scr._eCollectableType == eCollectableType.Butterfly)
        {
			if (ScoreHandler._OnScoreEventCallback != null)
				ScoreHandler._OnScoreEventCallback(eScoreType.Butterfly);
        }
		
		else if (scr._eCollectableType == eCollectableType.Magnet)
		{
			EnableMagnet();
			Destroy(scr.gameObject);
		}

		else if (scr._eCollectableType == eCollectableType.Poison)
		{
            DataManager.AddToCSessionPoisonAmount(1);
			Destroy(scr.gameObject);
		}

        else if (scr._eCollectableType == eCollectableType.AirWing)
        {
            FriendManager.Instance.InstantiateAirWings(scr._xAxisPosition);
            Destroy(scr.gameObject);
        }
	}

    public void EnableMagnet()
    {
		if (!mbMagnetIsAttachedToPlayer)
		{
            if (DataManager.GetPoisonRange() != -1)
            {
                int tIndex = DataManager.GetPoisonRange();
                mfPlayerCoinMagnetLifeDurationInSecond = _arrOfMagnetTime[tIndex];
            }

            else
                mfPlayerCoinMagnetLifeDurationInSecond = 15f;

			mfTempPlayerCoinMagnetLifeDurationInSecond = mfPlayerCoinMagnetLifeDurationInSecond;
			mbMagnetIsAttachedToPlayer = true;
		}

		else if (mbMagnetIsAttachedToPlayer)
            mfTempPlayerCoinMagnetLifeDurationInSecond += mfPlayerCoinMagnetLifeDurationInSecond;

		if (MiniGameManager.Instance.AutoImplementedProperties_eMiniGameState == eMiniGameState.CollectPowerUp || MiniGameManager.Instance.AutoImplementedProperties_eMiniGameState == eMiniGameState.AvoidPowerUp)
			MiniGameManager.Instance._iPowerUpsCollected += 1;
    }

	void MagnetHandler()
	{
		if (mfTempPlayerCoinMagnetLifeDurationInSecond > 0f)
		    mfTempPlayerCoinMagnetLifeDurationInSecond -= Time.deltaTime;

		if (mfTempPlayerCoinMagnetLifeDurationInSecond > 0.5f)
		    SearchForSpecificElement(eCollectableType.StartCoin, DataHandler._fPlayerCoinCollectionDistanceForMagnet);

		else if (mfTempPlayerCoinMagnetLifeDurationInSecond > 0f && mfTempPlayerCoinMagnetLifeDurationInSecond < 0.5f)
		    mbMagnetIsAttachedToPlayer = false;
	}

	void SearchForSpecificElement(eCollectableType SpecificElement, float MaxDistanceAllowed)
	{
        for (int i = 0; i < _listOfCollectableHandlers.Count; i++)
        {
            CollectableHandler element = _listOfCollectableHandlers[i];
            if (element._eCollectableType == SpecificElement)
            {
                if (Vector3.Distance(element.GetTransform.position, _playerManager._playerHandler._tPlayerTransform.position) < MaxDistanceAllowed)
                    element._eMoveTowards = eMoveTowards.Player;
            }
        }
	}

    public void ManipulateElement(eFoodType checkType, eFoodType setType, bool state)
    {
        for (int i = 0; i < _listOfFoodHandlers.Count; i++)
        {
            FoodHandler element = _listOfFoodHandlers[i];
			if (element._eFoodType == checkType)
			{
				element._eFoodType = setType;
				mArrOfChilds = new GameObject[element.transform.childCount];

				for (int j = 0; j < mArrOfChilds.Length; j++)
				{
					mArrOfChilds[j] = element.transform.GetChild(j).gameObject;
					mArrOfChilds[j].gameObject.SetActive(state);
				}
			}
        }
    }

	public void PlayerCollectedCoin(Transform coinTransform)
	{
		_ParticleCoins.transform.position = coinTransform.position;
		_ParticleCoins.Play ();
	}
}
