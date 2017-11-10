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
    List<GameObject> mListOfCollectableGameObject = new List<GameObject>();
    GameObject mTempGameObject;
    bool mbShouldDestroyCGO = false;
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

        if (mbShouldDestroyCGO)
        {
            mbShouldDestroyCGO = false;
            Destroy(mTempGameObject);
        }
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

		if (MiniGameManager.Instance._eMiniGameState == eMiniGameState.EatFood || MiniGameManager.Instance._eMiniGameState == eMiniGameState.AvoidFood)
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
            CEffectsPlayer.Instance.Play("MagnetActive");
			EnableMagnet();
            mbShouldDestroyCGO = true;
            mTempGameObject = scr.gameObject;
		}

		else if (scr._eCollectableType == eCollectableType.Poison)
		{
            CEffectsPlayer.Instance.Play("OtherCollection");
            DataManager.AddToCSessionPoisonAmount(1);
            mbShouldDestroyCGO = true;
            mTempGameObject = scr.gameObject;

            GameObject tCanvas = UICanvasHandler.Instance.GetActiveCanvasByName("HUDCanvas");
            if (tCanvas != null)
                tCanvas.GetComponent<GameplayAreaUIHandler>().DisplayPoisonCount();
		}

        else if (scr._eCollectableType == eCollectableType.AirWing)
        {
            CEffectsPlayer.Instance.Play("AirwingsActive");
            FriendManager.Instance.InstantiateAirWings(scr._xAxisPosition);
            mbShouldDestroyCGO = true;
            mTempGameObject = scr.gameObject;
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

		if (MiniGameManager.Instance._eMiniGameState == eMiniGameState.CollectPowerUp || MiniGameManager.Instance._eMiniGameState == eMiniGameState.AvoidPowerUp)
			MiniGameManager.Instance._iPowerUpsCollected += 1;

        GameObject tCanvas = UICanvasHandler.Instance.GetActiveCanvasByName("HUDCanvas");
        if (tCanvas != null)
            tCanvas.GetComponent<GameplayAreaUIHandler>().SetMagnetBtnState(false);
    }

	void MagnetHandler()
	{
		if (mfTempPlayerCoinMagnetLifeDurationInSecond > 0f)
		    mfTempPlayerCoinMagnetLifeDurationInSecond -= Time.deltaTime;

		if (mfTempPlayerCoinMagnetLifeDurationInSecond > 0.5f)
		    SearchForSpecificElement(eCollectableType.StartCoin, DataHandler._fPlayerCoinCollectionDistanceForMagnet);

		else if (mfTempPlayerCoinMagnetLifeDurationInSecond > 0f && mfTempPlayerCoinMagnetLifeDurationInSecond < 0.5f)
        {
            mbMagnetIsAttachedToPlayer = false;

            GameObject tCanvas = UICanvasHandler.Instance.GetActiveCanvasByName("HUDCanvas");
            if (tCanvas != null)
                tCanvas.GetComponent<GameplayAreaUIHandler>().SetMagnetBtnState(true);
        }	    
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
