using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    static DataManager mInstance;
    static List<ePlayerSkinID> mListOfPlayerSkinID = new List<ePlayerSkinID>();
    static List<eEnvID> mListOfEnvID = new List<eEnvID>();

	#region PlayerPrefs String Related Variables
	static string msTotalCoin = "TotalCoinAmount";
    static string msTotalButterfly = "TotalButterflyAmount";
    static string msCSessionCoin = "CSessionCoinAmount";
    static string msCSessionButterfly = "CSessionButterflyAmount";

    static string msHighScore = "HighScore";
    static string msCSessionScore = "CSessionScore";

    static string msCSessionCollectedPoison = "CSessionCollectedPoison";

    static string msPurchasedLive = "PurchasedLive";
    static string msPurchasedPoison = "PurchasedPoison";
    static string msPurchasedAirwing = "PurchasedAirwing";
    static string msPurchasedMagnet = "PurchasedMagnet";

    static string msCoinMultiplierValue = "CoinMultiplierValue";
    static string msMagnetTime = "MagnetTime";
    static string msPoisonRange = "PoisonRange";

	static string msEquipedSkinID = "EquipedSkinID";
    static string msSkinLockState = "LockState";
    static string msSkinPurchaseState = "PurchaseState";
    static string msSkinEquipState = "EquipState";

    static string msActiveEnvID = "ActiveEnvID";
    static string msEnvLockState = "LockState";
    static string msEnvPurchaseState = "PurchaseState";

    static string msLockEnvInitialization = "LockEnvInitialization";
    static string msNonPurchasedEnvIDCheckState = "NonPurchasedEnvIDCheckState";

    static string msChallengeCommenceTimerState = "CCTimerActive";
    static string msChallengeCommenceTimeStampTarget = "CCTimeStampTarget";
    static string msChallengeActiveTimerState = "CATimerActive";
    static string msChallengeActiveTimeStampTarget = "CATimeStampTarget";

    static string msSettingScreenExitTarget = "SettingScreenExitTarget";
    static string msMainMenuScreenLoadingState = "MainMenuScreenLoadingState";
    #endregion

    public static DataManager Instance
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

    void Start()
    {
        CollectableDataInitialize();
        LiveDataInitialize();
        ScoreDataInitialize();
        SkinDataInitialize();
        EnvDataInitialize();
    }

    void CollectableDataInitialize()
    {
        if (PlayerPrefs.HasKey(msTotalCoin))
        {
            // Do nothing
        }

        else
            PlayerPrefs.SetInt(msTotalCoin, 10000);

        if (PlayerPrefs.HasKey(msTotalButterfly))
        {
            // Do nothing
        }

        else
            PlayerPrefs.SetInt(msTotalButterfly, 50);
        
        PlayerPrefs.SetInt(msCSessionCoin, 0);
        PlayerPrefs.SetInt(msCSessionButterfly, 0);
        PlayerPrefs.SetInt(msCSessionCollectedPoison, 0);
	}

    void LiveDataInitialize()
    {
        if (PlayerPrefs.HasKey(msPurchasedLive))
        {
            if (PlayerPrefs.GetInt(msPurchasedLive) <= 1)
                PlayerPrefs.SetInt(msPurchasedLive, 3);
        }

        else
            PlayerPrefs.SetInt(msPurchasedLive, 3);
    }

    void ScoreDataInitialize()
    {
        PlayerPrefs.SetInt(msCSessionScore, 0);
    }

    void SkinDataInitialize()
    {
        mListOfPlayerSkinID.Add(ePlayerSkinID.Skin_1);
        mListOfPlayerSkinID.Add(ePlayerSkinID.Skin_2);
        mListOfPlayerSkinID.Add(ePlayerSkinID.Skin_3);
        mListOfPlayerSkinID.Add(ePlayerSkinID.Skin_4);
        mListOfPlayerSkinID.Add(ePlayerSkinID.Skin_5);
        mListOfPlayerSkinID.Add(ePlayerSkinID.Skin_6);
        mListOfPlayerSkinID.Add(ePlayerSkinID.Skin_7);
        mListOfPlayerSkinID.Add(ePlayerSkinID.Skin_8);
        mListOfPlayerSkinID.Add(ePlayerSkinID.Skin_9);
        mListOfPlayerSkinID.Add(ePlayerSkinID.Skin_10);
        mListOfPlayerSkinID.Add(ePlayerSkinID.Skin_11);
        mListOfPlayerSkinID.Add(ePlayerSkinID.Skin_12);
        mListOfPlayerSkinID.Add(ePlayerSkinID.Skin_13);
        mListOfPlayerSkinID.Add(ePlayerSkinID.Skin_14);
        mListOfPlayerSkinID.Add(ePlayerSkinID.Skin_15);
        mListOfPlayerSkinID.Add(ePlayerSkinID.Skin_16);
        mListOfPlayerSkinID.Add(ePlayerSkinID.Skin_17);
        mListOfPlayerSkinID.Add(ePlayerSkinID.Skin_18);
        mListOfPlayerSkinID.Add(ePlayerSkinID.Skin_19);
        mListOfPlayerSkinID.Add(ePlayerSkinID.Skin_20);
        mListOfPlayerSkinID.Add(ePlayerSkinID.Skin_21);
        mListOfPlayerSkinID.Add(ePlayerSkinID.Skin_22);
        mListOfPlayerSkinID.Add(ePlayerSkinID.Skin_23);
        mListOfPlayerSkinID.Add(ePlayerSkinID.Skin_24);
        mListOfPlayerSkinID.Add(ePlayerSkinID.Skin_25);
        mListOfPlayerSkinID.Add(ePlayerSkinID.Skin_26);
        mListOfPlayerSkinID.Add(ePlayerSkinID.Skin_27);
        mListOfPlayerSkinID.Add(ePlayerSkinID.Skin_28);
        mListOfPlayerSkinID.Add(ePlayerSkinID.Skin_29);
        mListOfPlayerSkinID.Add(ePlayerSkinID.Skin_30);

        if (GetEquipedSkinID() != 0)
            return;
        
        SetSkinStates(ePlayerSkinID.Skin_1.ToString(), 1, 1, 1);
        SetSkinStates(ePlayerSkinID.Skin_2.ToString(), 1, 0, 0);
        SetSkinStates(ePlayerSkinID.Skin_3.ToString(), 1, 0, 0);
        SetSkinStates(ePlayerSkinID.Skin_4.ToString(), 1, 0, 0);
        SetSkinStates(ePlayerSkinID.Skin_5.ToString(), 1, 0, 0);
        SetSkinStates(ePlayerSkinID.Skin_6.ToString(), 1, 0, 0);
        SetSkinStates(ePlayerSkinID.Skin_7.ToString(), 1, 0, 0);
        SetSkinStates(ePlayerSkinID.Skin_8.ToString(), 1, 0, 0);
    }

    void EnvDataInitialize()
    {
        mListOfEnvID.Add(eEnvID.Env_1);
        mListOfEnvID.Add(eEnvID.Env_2);
        mListOfEnvID.Add(eEnvID.Env_3);

        if (PlayerPrefs.HasKey(msLockEnvInitialization))
            return;
        
        SetEnvStates(eEnvID.Env_1.ToString(), 1, 1);
        SetEnvStates(eEnvID.Env_2.ToString(), 1, 0);
        SetEnvStates(eEnvID.Env_3.ToString(), 1, 0);
        PlayerPrefs.SetInt(msLockEnvInitialization, 1);
    }

    public static void AddToTotalCoin(int Value)
    {
		if (PlayerPrefs.HasKey(msTotalCoin))
		{
			int tValue = PlayerPrefs.GetInt(msTotalCoin);
			tValue += Value;
			PlayerPrefs.SetInt(msTotalCoin, tValue);
		}

		else
			PlayerPrefs.SetInt(msTotalCoin, Value);

        GameObject tCanvas = UICanvasHandler.Instance.GetActiveCanvasByName("StoreTabCanvas");
        if (tCanvas != null)
            tCanvas.GetComponent<StoreTabUIHandler>().DisplayCoinAmount();
    }

	public static void AddToCSessionCoin(int Value)
	{
		if (PlayerPrefs.HasKey(msCSessionCoin))
		{
			int tValue = PlayerPrefs.GetInt(msCSessionCoin);
			tValue += Value;
			PlayerPrefs.SetInt(msCSessionCoin, tValue);
		}

		else
			PlayerPrefs.SetInt(msCSessionCoin, Value);
	}

    public static void AddToTotalButterfly(int Value)
    {
		if (PlayerPrefs.HasKey(msTotalButterfly))
		{
			int tValue = PlayerPrefs.GetInt(msTotalButterfly);
			tValue += Value;
			PlayerPrefs.SetInt(msTotalButterfly, tValue);
		}

		else
			PlayerPrefs.SetInt(msTotalButterfly, Value);

        GameObject tCanvas = UICanvasHandler.Instance.GetActiveCanvasByName("StoreTabCanvas");
        if (tCanvas != null)
            tCanvas.GetComponent<StoreTabUIHandler>().DisplayButterflyAmount();
    }

	public static void AddToCSessionButterfly(int Value)
	{
		if (PlayerPrefs.HasKey(msCSessionButterfly))
		{
			int tValue = PlayerPrefs.GetInt(msCSessionButterfly);
			tValue += Value;
			PlayerPrefs.SetInt(msCSessionButterfly, tValue);
		}

		else
			PlayerPrefs.SetInt(msCSessionButterfly, Value);
	}

    public static void SubstractFromTotalCoin(int Value)
    {
        int tValue = PlayerPrefs.GetInt(msTotalCoin);
        tValue -= Value;
        PlayerPrefs.SetInt(msTotalCoin, tValue);

        GameObject tCanvas = UICanvasHandler.Instance.GetActiveCanvasByName("StoreTabCanvas");
        if (tCanvas != null)
            tCanvas.GetComponent<StoreTabUIHandler>().DisplayCoinAmount();
    }

	public static void SubstractFromTotalButterfly(int Value)
	{
		int tValue = PlayerPrefs.GetInt(msTotalButterfly);
        tValue -= Value;
		PlayerPrefs.SetInt(msTotalButterfly, tValue);

        GameObject tCanvas = UICanvasHandler.Instance.GetActiveCanvasByName("StoreTabCanvas");
        if (tCanvas != null)
            tCanvas.GetComponent<StoreTabUIHandler>().DisplayButterflyAmount();
	}

    public static void AddToHighScore(int score)                //cRamappa
    {
		if (PlayerPrefs.HasKey(msHighScore))
		{
			int tScore = PlayerPrefs.GetInt(msHighScore);
            tScore += score;
			PlayerPrefs.SetInt(msHighScore, tScore);
		}

		else
			PlayerPrefs.SetInt(msHighScore, score);
    }

    public static void AddToCSessionScore(int score)
    {
		if (PlayerPrefs.HasKey(msCSessionScore))
		{
			int tScore = PlayerPrefs.GetInt(msCSessionScore);
			tScore += score;
			PlayerPrefs.SetInt(msCSessionScore, tScore);
		}

		else
			PlayerPrefs.SetInt(msCSessionScore, score);
    }

	public static void AddToLiveAmount(int Value)
	{
		if (PlayerPrefs.HasKey(msPurchasedLive))
		{
			int tValue = PlayerPrefs.GetInt(msPurchasedLive);
			tValue += Value;
			PlayerPrefs.SetInt(msPurchasedLive, tValue);
		}

		else
			PlayerPrefs.SetInt(msPurchasedLive, Value);
	}

	public static void SetLiveAmount(int Value)
	{
		PlayerPrefs.SetInt(msPurchasedLive, Value);
	}

    public static void AddToPoisonAmount(int Value)
    {
        if (PlayerPrefs.HasKey(msPurchasedPoison))
        {
            int tValue = PlayerPrefs.GetInt(msPurchasedPoison);
            tValue += Value;
            PlayerPrefs.SetInt(msPurchasedPoison, tValue);
        }

        else
            PlayerPrefs.SetInt(msPurchasedPoison, Value);
    }

    public static void SubstarctFromPoisonAmount(int Value)
    {
        int tValue = PlayerPrefs.GetInt(msPurchasedPoison);
        tValue -= Value;
        PlayerPrefs.SetInt(msPurchasedPoison, tValue);
    }

	public static void AddToCSessionPoisonAmount(int Value)
	{
		if (PlayerPrefs.HasKey(msCSessionCollectedPoison))
		{
			int tValue = PlayerPrefs.GetInt(msCSessionCollectedPoison);
			tValue += Value;
			PlayerPrefs.SetInt(msCSessionCollectedPoison, tValue);
		}

		else
			PlayerPrefs.SetInt(msCSessionCollectedPoison, Value);
	}

	public static void SubstarctFromCSessionPoisonAmount(int Value)
	{
		int tValue = PlayerPrefs.GetInt(msCSessionCollectedPoison);
		tValue -= Value;
		PlayerPrefs.SetInt(msCSessionCollectedPoison, tValue);
	}

	public static void AddToAirwingAmount(int Value)
	{
		if (PlayerPrefs.HasKey(msPurchasedAirwing))
		{
			int tValue = PlayerPrefs.GetInt(msPurchasedAirwing);
			tValue += Value;
			PlayerPrefs.SetInt(msPurchasedAirwing, tValue);
		}

		else
			PlayerPrefs.SetInt(msPurchasedAirwing, Value);
	}

	public static void SubstarctFromAirwingAmount(int Value)
	{
		int tValue = PlayerPrefs.GetInt(msPurchasedAirwing);
		tValue -= Value;
		PlayerPrefs.SetInt(msPurchasedAirwing, tValue);
	}

	public static void AddToMagnetAmount(int Value)
	{
		if (PlayerPrefs.HasKey(msPurchasedMagnet))
		{
			int tValue = PlayerPrefs.GetInt(msPurchasedMagnet);
			tValue += Value;
			PlayerPrefs.SetInt(msPurchasedMagnet, tValue);
		}

		else
			PlayerPrefs.SetInt(msPurchasedMagnet, Value);
	}

	public static void SubstarctFromMagnetAmount(int Value)
	{
		int tValue = PlayerPrefs.GetInt(msPurchasedMagnet);
		tValue -= Value;
		PlayerPrefs.SetInt(msPurchasedMagnet, tValue);
	}

    public static void SetCoinValue(int Value)
    {
        PlayerPrefs.SetInt(msCoinMultiplierValue, Value);
    }

	public static void SetMagnetTime(int Value)
	{
		PlayerPrefs.SetInt(msMagnetTime, Value);
	}

    public static void SetPoisonRange(int Value)
    {
        PlayerPrefs.SetInt(msPoisonRange, Value);
    }

	public static void SetEquipedSkinID(int Value)
	{
		PlayerPrefs.SetInt(msEquipedSkinID, Value);
	}

    public static void SetSkinStates(string skinName, int lockState, int purchaseState, int equipState)
    {
        PlayerPrefs.SetInt(skinName + msSkinLockState, lockState);
        PlayerPrefs.SetInt(skinName + msSkinPurchaseState, purchaseState);
        PlayerPrefs.SetInt(skinName + msSkinEquipState, equipState);
    }

    public static void SetActiveEnvID(int Value)
    {
        PlayerPrefs.SetInt(msActiveEnvID, Value);
    }

    public static void SetEnvStates(string envName, int lockState, int purchaseState)
    {
        PlayerPrefs.SetInt(envName + msEnvLockState, lockState);
        PlayerPrefs.SetInt(envName + msEnvPurchaseState, purchaseState);
    }

    public static void SetNonPurchasedEnvIDCheckState(int Value)
    {
        PlayerPrefs.SetInt(msNonPurchasedEnvIDCheckState, Value);
    }

    public static void SetChallengeCommenceTimerState(int Value)
    {
        PlayerPrefs.SetInt(msChallengeCommenceTimerState, Value);
    }

    public static void SetChallengeCommenceTimeStampTarget(string Value)
    {
        PlayerPrefs.SetString(msChallengeCommenceTimeStampTarget, Value);
    }

    public static void SetChallengeActiveTimerState(int Value)
    {
        PlayerPrefs.SetInt(msChallengeActiveTimerState, Value);
    }

    public static void SetChallengeActiveTimeStampTarget(string Value)
    {
        PlayerPrefs.SetString(msChallengeActiveTimeStampTarget, Value);
    }

    public static void SetSettingsCanvasExitTarget(int Value)
    {
        PlayerPrefs.SetInt(msSettingScreenExitTarget, Value);
    }

    public static void SetMainMenuScreenLoadingState(int Value)
    {
        PlayerPrefs.SetInt(msMainMenuScreenLoadingState, Value);
    }

    //-----------------------------------------------------------------------------------------------//

    public static int GetTotalCoinAmount()
    {
        return PlayerPrefs.GetInt(msTotalCoin);
    }

	public static int GetCSessionCoinAmount()
	{
		return PlayerPrefs.GetInt(msCSessionCoin);
	}

    public static int GetTotalButterflyAmount()
	{
		return PlayerPrefs.GetInt(msTotalButterfly);
	}

	public static int GetCSessionButterflyAmount()
	{
		return PlayerPrefs.GetInt(msCSessionButterfly);
	}

	public static int GetHighScore()
	{
		return PlayerPrefs.GetInt(msHighScore);
	}

	public static int GetCSessionScore()
	{
		return PlayerPrefs.GetInt(msCSessionScore);
	}

    public static int GetLiveAmount()
    {
        return PlayerPrefs.GetInt(msPurchasedLive);
    }

    public static int GetPoisonAmount()
    {
        return PlayerPrefs.GetInt(msPurchasedPoison);
    }

    public static int GetCSessionPoisonAmount()
    {
        return PlayerPrefs.GetInt(msCSessionCollectedPoison);
    }

    public static int GetAirwingAmount()
    {
        return PlayerPrefs.GetInt(msPurchasedAirwing);
    }

    public static int GetMagnetAmount()
    {
        return PlayerPrefs.GetInt(msPurchasedMagnet);
    }

    public static int GetCoinValue()
    {
        int tValue = 1;
        if (PlayerPrefs.HasKey(msCoinMultiplierValue))
            tValue = PlayerPrefs.GetInt(msCoinMultiplierValue);
        return tValue;
    }

	public static int GetMagnetTime()
	{
		int tValue = -1;
		if (PlayerPrefs.HasKey(msMagnetTime))
			tValue = PlayerPrefs.GetInt(msMagnetTime);
		return tValue;
	}

    public static int GetPoisonRange()
    {
        int tValue = -1;
        if (PlayerPrefs.HasKey(msPoisonRange))
            tValue = PlayerPrefs.GetInt(msPoisonRange);
        return tValue;
    }

    public static int GetEquipedSkinID()
    {
        int tValue = 0;
        if (PlayerPrefs.HasKey(msEquipedSkinID))
            tValue = PlayerPrefs.GetInt(msEquipedSkinID);
        return tValue;
    }

	public static SkinStates GetSkinStates(string skinName)
	{
        SkinStates tSkinStates = new SkinStates();
		for (int i = 0; i < mListOfPlayerSkinID.Count; i++)
		{
			string tePlayerSkinID = mListOfPlayerSkinID[i].ToString();
			if (tePlayerSkinID.Equals(skinName))
			{
				tSkinStates._iSkinLockState = PlayerPrefs.GetInt(skinName + msSkinLockState);
				tSkinStates._iSkinPurchaseState = PlayerPrefs.GetInt(skinName + msSkinPurchaseState);
				tSkinStates._iSkinEquipState = PlayerPrefs.GetInt(skinName + msSkinEquipState);
				break;
			}
		}
		return tSkinStates;
	}

    public static int GetActiveEnvID()
    {
        int tValue = 0;
        if (PlayerPrefs.HasKey(msActiveEnvID))
            tValue = PlayerPrefs.GetInt(msActiveEnvID);
        return tValue;
    }

    public static EnvStates GetEnvStates(string envName)
    {
        EnvStates tEnvStates = new EnvStates();
        for (int i = 0; i < mListOfEnvID.Count; i++)
        {
            string teEnvID = mListOfEnvID[i].ToString();
            if (teEnvID.Equals(envName))
            {
                tEnvStates._iEnvLockState = PlayerPrefs.GetInt(envName + msEnvLockState);
                tEnvStates._iEnvPurchaseState = PlayerPrefs.GetInt(envName + msEnvPurchaseState);
                break;
            }
        }
        return tEnvStates;
    }

    public static bool GetAllEnvPurchasedState()
    {
        bool tEnvPurchasedState = false;
        int tCount = 0;
        for (int i = 0; i < mListOfEnvID.Count; i++)
        {
            if (PlayerPrefs.GetInt(mListOfEnvID[i].ToString() + msEnvPurchaseState) == 1)
                tCount += 1;
        }

        if (tCount == mListOfEnvID.Count)
            tEnvPurchasedState = true;  
        
        return tEnvPurchasedState;
    }

    public static int GetNonPurchasedEnvID()
    {
        int tValue = 0;
        for (int i = 0; i < mListOfEnvID.Count; i++)
        {
            if (PlayerPrefs.GetInt(mListOfEnvID[i].ToString() + msEnvPurchaseState) == 0)
            {
                tValue = i;
                break;
            }
        }
        return tValue;
    }

    public static int GetNonPurchasedEnvIDCheckState()
    {
        int tValue = 0;
        if (PlayerPrefs.HasKey(msNonPurchasedEnvIDCheckState))
            tValue = PlayerPrefs.GetInt(msNonPurchasedEnvIDCheckState);
        return tValue;
    }

    public static int GetChallengeCommenceTimerState()
    {
        int tValue = 0;
        if (PlayerPrefs.HasKey(msChallengeCommenceTimerState))
            tValue = PlayerPrefs.GetInt(msChallengeCommenceTimerState, 0);
        return tValue;
    }

    public static string GetChallengeCommenceTimeStampTarget()
    {
        string tValue = "00";
        if (PlayerPrefs.HasKey(msChallengeCommenceTimeStampTarget))
            tValue = PlayerPrefs.GetString(msChallengeCommenceTimeStampTarget);
        return tValue;
    }

    public static int GetChallengeActiveTimerState()
    {
        int tValue = 0;
        if (PlayerPrefs.HasKey(msChallengeActiveTimerState))
            tValue = PlayerPrefs.GetInt(msChallengeActiveTimerState, 0);
        return tValue;
    }

    public static string GetChallengeActiveTimeStampTarget()
    {
        string tValue = "00";
        if (PlayerPrefs.HasKey(msChallengeActiveTimeStampTarget))
            tValue = PlayerPrefs.GetString(msChallengeActiveTimeStampTarget);
        return tValue;
    }

    public static int GetSettingsCanvasExitTarget()
    {
        int tValue = 0;
        if (PlayerPrefs.HasKey(msSettingScreenExitTarget))
            tValue = PlayerPrefs.GetInt(msSettingScreenExitTarget);
        return tValue;
    }

    public static int GetMainMenuScreenLoadingState()
    {
        int tValue = 0;
        if (PlayerPrefs.HasKey(msMainMenuScreenLoadingState))
            tValue = PlayerPrefs.GetInt(msMainMenuScreenLoadingState);
        return tValue;
    }
}

public class SkinStates
{
    public int _iSkinLockState;
    public int _iSkinPurchaseState;
    public int _iSkinEquipState;
}

public class EnvStates
{
    public int _iEnvLockState;
    public int _iEnvPurchaseState;
}
