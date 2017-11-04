using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum eMiniGameState
{
	None = -1,
	CollectCoins,
	CollectButterflies,
	StunEnemy,
	AcceptFriendHelp,
	EatFood,
	CollectPowerUp,
	LeftRightMovement,
	HighAndLongJump,
	AvoidEnemy,
	AvoidFriend,
	AvoidFood,
	AvoidPowerUp,
	AvoidObstacles,
	AvoidDying
}

public enum eRewardState
{
    None = 0,
    Reward
}

public class MiniGameManager : MonoBehaviour
{
    static MiniGameManager mInstance;
    eMiniGameState meMiniGameState = eMiniGameState.None;
    eMiniGameState meTempMiniGameState = eMiniGameState.None;
    MiniGameInfoUIHandler mMiniGameInfoUIHandlerScr;
    MiniGameRewardUIHandler mMiniGameRewardUIHandlerScr;
    MiniGameHandler mCurrentMiniGameHandlerScr;
	float mfMiniGameStartTime;
    float mfTempMiniGameLength;
    bool mbSkipInfo;
    bool mbSkipReward;
    bool mbMinGameIsActive;
    bool mbActive;
    bool mbDoOnce = true;

    public eRewardState _eRewardState = eRewardState.None;
	public int _iCoinsCollected;
	public int _iButterfliesCollected;
	public int _iEnemiesStunned;
	public int _iFriendsHelpAccepted;
	public int _iLeftRightMovementPerformed;
	public int _iHighAndLongJumpPerformed;
	public int _iFoodsEaten;
	public int _iPowerUpsCollected;
    public int _iObstaclesTouched;
    public int _iPlayerDeathCount;
    public string[] _arrOfMiniGameInfo;
    public List<MiniGameHandler> _listOfMinGameHandlers = new List<MiniGameHandler>();

	public eMiniGameState AutoImplementedProperties_eMiniGameState
	{
		get { return meMiniGameState; }

		set { meMiniGameState = value; }
	}

    public static MiniGameManager Instance
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
        MiniGameStateHandler();
    }

    public void CheckIfMiniGameCanBeActivated(MiniGameHandler scr)
    {
        if (!mbMinGameIsActive)
        {
			UICanvasHandler.Instance.LoadScreen("MiniGameChioceCanvas", null, true);
			GetPropertiesOfMiniGame(scr);
		}
    }

	void GetPropertiesOfMiniGame(MiniGameHandler scr)
	{
        mCurrentMiniGameHandlerScr = scr;
        Destroy(scr.gameObject);
		PlayerManager.Instance._playerHandler._eControlState = eControlState.Deactive;
        meTempMiniGameState = (eMiniGameState)mCurrentMiniGameHandlerScr._eMiniGameTypes;
	}

    public void ActivateMiniGame(float currentTime)
	{
		GameObject tChoiceCanvas = UICanvasHandler.Instance.GetActiveCanvasByName("MiniGameChioceCanvas");
		if (tChoiceCanvas != null)
			UICanvasHandler.Instance.DestroyScreen(tChoiceCanvas);

		UICanvasHandler.Instance.LoadScreen("MiniGameInfoCanvas", null, true);

		GameObject tInfoCanvas = UICanvasHandler.Instance.GetActiveCanvasByName("MiniGameInfoCanvas");
		if (tInfoCanvas != null)
			mMiniGameInfoUIHandlerScr = tInfoCanvas.GetComponent<MiniGameInfoUIHandler>();
        
        PlayerManager.Instance._playerHandler._eControlState = eControlState.Active;
        mbMinGameIsActive = true;
        mbActive = true;
        mbSkipInfo = false;
        mbSkipReward = false;
		mfMiniGameStartTime = currentTime;
        mfTempMiniGameLength = mCurrentMiniGameHandlerScr._fMinGameTimeLengthInSecond;
        meMiniGameState = meTempMiniGameState;
        meTempMiniGameState = eMiniGameState.None;
	}

	void MiniGameStateHandler()
	{
		switch (meMiniGameState)
		{
			case eMiniGameState.None:
				_iCoinsCollected = 0;
				_iEnemiesStunned = 0;
				_iFoodsEaten = 0;
				_iButterfliesCollected = 0;
				_iPowerUpsCollected = 0;
				_iFriendsHelpAccepted = 0;
				_iHighAndLongJumpPerformed = 0;
				_iLeftRightMovementPerformed = 0;
                _iObstaclesTouched = 0;
                _iPlayerDeathCount = 0;
                mbDoOnce = true;
				break;

			case eMiniGameState.CollectCoins:
				MiniGameActionHandler(mCurrentMiniGameHandlerScr._fMinGameTimeLengthInSecond, eSetType.Coin, eMiniGameState.CollectCoins, _iCoinsCollected, mCurrentMiniGameHandlerScr._iCoinsNeedsToCollect, false);
				break;

			case eMiniGameState.CollectButterflies:
				MiniGameActionHandler(mCurrentMiniGameHandlerScr._fMinGameTimeLengthInSecond, eSetType.Butterfly, eMiniGameState.CollectButterflies, _iButterfliesCollected, mCurrentMiniGameHandlerScr._iButterfliesNeedsToCollect, false);
				break;

			case eMiniGameState.StunEnemy:
				MiniGameActionHandler(mCurrentMiniGameHandlerScr._fMinGameTimeLengthInSecond, eSetType.Enemy, eMiniGameState.StunEnemy, _iEnemiesStunned, mCurrentMiniGameHandlerScr._iEnemiesNeedsToStun, false);
				break;

			case eMiniGameState.AcceptFriendHelp:
				MiniGameActionHandler(mCurrentMiniGameHandlerScr._fMinGameTimeLengthInSecond, eSetType.Friend_Multiple, eMiniGameState.AcceptFriendHelp, _iFriendsHelpAccepted, mCurrentMiniGameHandlerScr._iFriendsHelpIsRequired, false);
				break;

			case eMiniGameState.EatFood:
				MiniGameActionHandler(mCurrentMiniGameHandlerScr._fMinGameTimeLengthInSecond, eSetType.Food, eMiniGameState.EatFood, _iFoodsEaten, mCurrentMiniGameHandlerScr._iFoodsNeedsToEat, false);
				break;

			case eMiniGameState.CollectPowerUp:
				MiniGameActionHandler(mCurrentMiniGameHandlerScr._fMinGameTimeLengthInSecond, eSetType.PowerUp, eMiniGameState.CollectPowerUp, _iPowerUpsCollected, mCurrentMiniGameHandlerScr._iPowerUpsNeedsToCollect, false);
				break;

			case eMiniGameState.LeftRightMovement:
                MiniGameActionHandler(mCurrentMiniGameHandlerScr._fMinGameTimeLengthInSecond, eSetType.LeftRight, eMiniGameState.LeftRightMovement, _iLeftRightMovementPerformed, mCurrentMiniGameHandlerScr._iLeftRightMovementNeedsToPerform, false);
				break;

			case eMiniGameState.HighAndLongJump:
                MiniGameActionHandler(mCurrentMiniGameHandlerScr._fMinGameTimeLengthInSecond, eSetType.HighAndLongJump, eMiniGameState.HighAndLongJump, _iHighAndLongJumpPerformed, mCurrentMiniGameHandlerScr._iHighAndLongJumpNeedsToPerform, false);
				break;

			case eMiniGameState.AvoidEnemy:
				MiniGameActionHandler(mCurrentMiniGameHandlerScr._fMinGameTimeLengthInSecond, eSetType.Enemy, eMiniGameState.AvoidEnemy, _iEnemiesStunned, 0, true);
				break;

			case eMiniGameState.AvoidFriend:
				MiniGameActionHandler(mCurrentMiniGameHandlerScr._fMinGameTimeLengthInSecond, eSetType.Friend_Multiple, eMiniGameState.AvoidFriend, _iFriendsHelpAccepted, 0, true);
				break;

			case eMiniGameState.AvoidFood:
				MiniGameActionHandler(mCurrentMiniGameHandlerScr._fMinGameTimeLengthInSecond, eSetType.Food, eMiniGameState.AvoidFood, _iFoodsEaten, 0, true);
				break;

			case eMiniGameState.AvoidPowerUp:
                MiniGameActionHandler(mCurrentMiniGameHandlerScr._fMinGameTimeLengthInSecond, eSetType.PowerUp, eMiniGameState.AvoidPowerUp, _iPowerUpsCollected, 0, true);
				break;

			case eMiniGameState.AvoidObstacles:
                MiniGameActionHandler(mCurrentMiniGameHandlerScr._fMinGameTimeLengthInSecond, eSetType.Obstacles, eMiniGameState.AvoidObstacles, _iObstaclesTouched, 0, true);
				break;

			case eMiniGameState.AvoidDying:
                MiniGameActionHandler(mCurrentMiniGameHandlerScr._fMinGameTimeLengthInSecond, eSetType.HardCore, eMiniGameState.AvoidDying, _iPlayerDeathCount, 0, true);
				break;
		}
	}

    void MiniGameActionHandler(float MiniGameLength, eSetType CurrentSetType, eMiniGameState CurrentMiniGameType, int CurrentAmount, int RequiredAmount, bool AvoidElement)
    {
        if (!mbActive)
            return;

		// Collecting elements
		if (!AvoidElement)
		{
			if (Time.time - mfMiniGameStartTime < MiniGameLength)
			{
				if (mfTempMiniGameLength > 0f)
					mfTempMiniGameLength -= Time.deltaTime;

                if (mbDoOnce)
                {
                    mbDoOnce = false;
                    EnvironmentManager.Instance.MiniGameTypeSetInstantiation(CurrentSetType, true, true);    
                }

                else
                    EnvironmentManager.Instance.MiniGameTypeSetInstantiation(CurrentSetType, true, false);    

				if (mMiniGameInfoUIHandlerScr._miniGameInfoText != null)
					mMiniGameInfoUIHandlerScr._miniGameInfoText.text = _arrOfMiniGameInfo[(int)CurrentMiniGameType] + " " + CurrentAmount + "/" + RequiredAmount + "  <-->  " + "Time left: " + Mathf.Round(mfTempMiniGameLength);
			}

			else
			{
				if (CurrentAmount >= RequiredAmount)
                {
                    StartCoroutine(IResetMiniGameState(false));
                    RewardInfo();
                }

				else if (CurrentAmount < RequiredAmount)
				{
					if (mMiniGameInfoUIHandlerScr._miniGameInfoText != null)
						mMiniGameInfoUIHandlerScr._miniGameInfoText.text = _arrOfMiniGameInfo[_arrOfMiniGameInfo.Length - 1];
                    StartCoroutine(IResetMiniGameState(true));
				}
			}
		}


		//-----------------------------------------------------------//


		// Avoiding elements
		if (AvoidElement)
		{
			if (Time.time - mfMiniGameStartTime < MiniGameLength)
			{
				if (CurrentAmount == RequiredAmount)
				{
					if (mfTempMiniGameLength > 0f)
						mfTempMiniGameLength -= Time.deltaTime;

                    if (mbDoOnce)
                {
                    mbDoOnce = false;
                    EnvironmentManager.Instance.MiniGameTypeSetInstantiation(CurrentSetType, true, true);    
                }

                else
                    EnvironmentManager.Instance.MiniGameTypeSetInstantiation(CurrentSetType, true, false);  

					if (mMiniGameInfoUIHandlerScr._miniGameInfoText != null)
						mMiniGameInfoUIHandlerScr._miniGameInfoText.text = _arrOfMiniGameInfo[(int)CurrentMiniGameType] + " " + Mathf.Round(mfTempMiniGameLength) + " seconds.";
				}
			}

			else
			{
				if (CurrentAmount == RequiredAmount)
                {
                    StartCoroutine(IResetMiniGameState(false));
                    RewardInfo();
                }
					
				else if (CurrentAmount > RequiredAmount)
				{
					if (mMiniGameInfoUIHandlerScr._miniGameInfoText != null)
						mMiniGameInfoUIHandlerScr._miniGameInfoText.text = _arrOfMiniGameInfo[_arrOfMiniGameInfo.Length - 1];
                    StartCoroutine(IResetMiniGameState(true));
				}
			}
		}
    }

	IEnumerator IResetMiniGameState(bool state = true)
	{
        if (state)
           mCurrentMiniGameHandlerScr = null;
        
        mbActive = false;
        mbMinGameIsActive = false;
		meMiniGameState = eMiniGameState.None;
		EnvironmentManager.Instance.MiniGameTypeSetInstantiation(eSetType.AllType, false, false);
		yield return new WaitForSeconds(2f);
		GameObject tInfoCanvas = UICanvasHandler.Instance.GetActiveCanvasByName("MiniGameInfoCanvas");
		if (tInfoCanvas != null)
			UICanvasHandler.Instance.DestroyScreen(tInfoCanvas);
	}

    void RewardInfo()
    {
        if (!mbSkipInfo)
        {
            mbSkipInfo = true;

			UICanvasHandler.Instance.LoadScreen("MiniGameRewardCanvas", null, true);

			GameObject tRewardCanvas = UICanvasHandler.Instance.GetActiveCanvasByName("MiniGameRewardCanvas");
			if (tRewardCanvas != null)
				mMiniGameRewardUIHandlerScr = tRewardCanvas.GetComponent<MiniGameRewardUIHandler>();
            
			PlayerManager.Instance._playerHandler._eControlState = eControlState.Deactive;

			if (mMiniGameRewardUIHandlerScr._InfoText != null)
			{
				if (mCurrentMiniGameHandlerScr._iReward_CoinsAmount > 0 & mCurrentMiniGameHandlerScr._iReward_ButterfliesAmount == 0)
					mMiniGameRewardUIHandlerScr._InfoText.text = "Reward Coins: " + mCurrentMiniGameHandlerScr._iReward_CoinsAmount.ToString();

				else if (mCurrentMiniGameHandlerScr._iReward_CoinsAmount == 0 & mCurrentMiniGameHandlerScr._iReward_ButterfliesAmount > 0)
					mMiniGameRewardUIHandlerScr._InfoText.text = "Reward Butteflies: " + mCurrentMiniGameHandlerScr._iReward_ButterfliesAmount.ToString();

				else if (mCurrentMiniGameHandlerScr._iReward_CoinsAmount > 0 & mCurrentMiniGameHandlerScr._iReward_ButterfliesAmount > 0)
					mMiniGameRewardUIHandlerScr._InfoText.text = "Reward Coins: " + mCurrentMiniGameHandlerScr._iReward_CoinsAmount.ToString() + "\n\nReward Butterflies: " + mCurrentMiniGameHandlerScr._iReward_ButterfliesAmount.ToString();
			}
        }
    }

    public void GiveRewardToPlayer()
    {
        if (!mbSkipReward)
        {
            mbSkipReward = true;

			DataManager.AddToTotalCoin(mCurrentMiniGameHandlerScr._iReward_CoinsAmount);
			DataManager.AddToCSessionCoin(mCurrentMiniGameHandlerScr._iReward_CoinsAmount);

			DataManager.AddToTotalButterfly(mCurrentMiniGameHandlerScr._iReward_ButterfliesAmount);
			DataManager.AddToCSessionButterfly(mCurrentMiniGameHandlerScr._iReward_ButterfliesAmount);

			GameObject tCanvas = UICanvasHandler.Instance.GetActiveCanvasByName("HUDCanvas");
			if (tCanvas != null)
            {
                tCanvas.GetComponent<GameplayAreaUIHandler>().DisplayCoin();
                tCanvas.GetComponent<GameplayAreaUIHandler>().DisplayButterfly();
            }
        }

		if (mCurrentMiniGameHandlerScr._iReward_CoinsAmount > 0)
            StartCoroutine(IReward(1, 0));

		if (mCurrentMiniGameHandlerScr._iReward_ButterfliesAmount > 0)
            StartCoroutine(IReward(0, 1));
    		
        mCurrentMiniGameHandlerScr = null;
    }

	IEnumerator IReward(int coinCount, int butterflyCount)
	{
		float tStep = 0f;
		float tRate = 1f / 5f;
		while (tStep < 0.05f)
		{
			tStep += Time.deltaTime * tRate;

            if (coinCount > 0)
            {
				GameObject tCanvas = UICanvasHandler.Instance.GetActiveCanvasByName("HUDCanvas");
				if (tCanvas != null)
					tCanvas.GetComponent<GameplayAreaUIHandler>().InstantiateCoin();
            }

            if (butterflyCount > 0)
            {
				GameObject tCanvas = UICanvasHandler.Instance.GetActiveCanvasByName("HUDCanvas");
				if (tCanvas != null)
					tCanvas.GetComponent<GameplayAreaUIHandler>().InstantiateButterfly();
            }
			yield return null;
		}
	}

    public void DeactivateMiniGame(bool state = false)
    {
        mbActive = state;
        StartCoroutine(IResetMiniGameState(true));
    }
}
