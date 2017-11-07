using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum eSetType
{
    None = 0,
    Coin,
    Butterfly,
    Food,
    Friend_Duck,
    Friend_Spider,
    Friend_Kingfisher,
    Friend_Dragonfly,
    Friend_Multiple,
    Enemy,
    Obstacles,
    LeftRight,
    HighAndLongJump,
    HardCore,
    PowerUp,
    OtherType,
    AllType
}

public enum eSetTire
{
	Low = 0,
	Medium,
	High,
    MiniGame,
    Challenge
}

[System.Serializable]
public class MiniGameSetHandler
{
    public eSetType _eSetType = eSetType.None;
	public List<GameObject> _listOfSets = new List<GameObject>();
}

public class EnvironmentManager : MonoBehaviour
{
    public enum eEnvironmentState
    {
        None = 0,
        Instatiate
    }

    List<GameObject> mListOfInstatiatedSets = new List<GameObject>();
    List<EnvironmentHandler> mListOfEnvironmentHandler = new List<EnvironmentHandler>();
    eEnvironmentState meEnvironmentState = eEnvironmentState.None;
    eSetType meSetType = eSetType.None;
    int miCount;
    int miSetCount;
    int miSetHandlerPrefabIndex;
    int miSetPrefabIndex;
    int miChallengeSetPrefabIndex = 0;
    float mfSetPosZ;
    float mfPlayerTravelHowFarBeforeTheNextSetGenerate;
    float mfLowTireSetInstantiationTimeLimit = 60f;
    float mfMediumTireSetInstantiationTimeLimit = 60f;
    float mfStartTime;
    bool mbInstantiateMinigameSet = false;
    bool mbInstantiateChallengeSet = false;
    Vector3 mvTempPos;
    static EnvironmentManager mInstance = null;

    public EnvironmentHandler _currentActiveEnvironmentHandler = null;
    public GenerateRandomValueScr _GenerateRandomValueScr;
    public List<GameObject> _listOfLowTireSets;
    public List<GameObject> _listOfMediumTireSets;
    public List<GameObject> _listOfHighTireSets;
    public List<GameObject> _listOfChallengeSets;
    public List<MiniGameSetHandler> _listOfMiniGameSets = new List<MiniGameSetHandler>();
	public int _iNumberOfActiveTilesOnScreen = 3;
    public PlayerManager _playerManager; 
    [HideInInspector] public Vector3 _vCurrentPlatformPosition;
    [HideInInspector] public List<PlatformHandler> _listOfPlatformHandler = new List<PlatformHandler>();
    [HideInInspector] public eSetTire _eSetTire = eSetTire.Low;

	public static EnvironmentManager Instance
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
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 4)
            ChallengeTypeSetInstantiation();

        else
            _eSetTire = eSetTire.Low;
        
        _GenerateRandomValueScr = new GenerateRandomValueScr();
        _GenerateRandomValueScr._EnvironmentManager = this;
        mfStartTime = Time.time;
        miCount = 1;
        mfPlayerTravelHowFarBeforeTheNextSetGenerate = 20.0f;
        miSetCount += 1;
        mfSetPosZ = DataHandler._fEnvironmentSetLength;
        SetPosition();
        //Invoke("SetPosition", 0.75f);
    }

	void ChooseEnvironmentToSpawn(Vector3 Position, eSetTire setTire)
	{
		if (setTire == eSetTire.Low)
		{
            int j = _GenerateRandomValueScr.Random(0, _listOfLowTireSets.Count - 1);
			InstantiateSet(Position, _listOfLowTireSets[j]);
		}

		else if (setTire == eSetTire.Medium)
		{
            int j = _GenerateRandomValueScr.Random(0, _listOfMediumTireSets.Count - 1);
			InstantiateSet(Position, _listOfMediumTireSets[j]);
		}

		else if (setTire == eSetTire.High)
		{
            int j = _GenerateRandomValueScr.Random(0, _listOfHighTireSets.Count - 1);
			InstantiateSet(Position, _listOfHighTireSets[j]);
		}

		else if (setTire == eSetTire.MiniGame)
		{
			if (_listOfMiniGameSets.Count > 0)
			{
				for (int i = 0; i < _listOfMiniGameSets.Count; i++)
				{
					if (_listOfMiniGameSets[i]._eSetType == meSetType)
    					miSetHandlerPrefabIndex = i;
				}
                miSetPrefabIndex = _GenerateRandomValueScr.Random(0, _listOfMiniGameSets[miSetHandlerPrefabIndex]._listOfSets.Count - 1);
				InstantiateSet(Position, _listOfMiniGameSets[miSetHandlerPrefabIndex]._listOfSets[miSetPrefabIndex]);
			}
		}

        else if (setTire == eSetTire.Challenge)
        {
            InstantiateSet(Position, _listOfChallengeSets[miChallengeSetPrefabIndex]);
        }
	}

	void InstantiateSet(Vector3 Position, GameObject currentSet)
	{
        GameObject tGoSet = Instantiate(currentSet);
        tGoSet.transform.SetParent(this.transform);
        tGoSet.transform.position = Position;
		_currentActiveEnvironmentHandler = tGoSet.GetComponent<EnvironmentHandler>();
		_currentActiveEnvironmentHandler._environmentManager = this;
		mListOfEnvironmentHandler.Add(_currentActiveEnvironmentHandler);
		mListOfInstatiatedSets.Add(tGoSet);
	}

	public void MiniGameTypeSetInstantiation(eSetType setType, bool state, bool miniGameState)
	{
		meSetType = setType;
		mbInstantiateMinigameSet = state;

        if (miniGameState)
        {
			Vector3 tLastSetPos = mListOfInstatiatedSets[mListOfInstatiatedSets.Count - 1].transform.position;
			Destroy(mListOfInstatiatedSets[mListOfInstatiatedSets.Count - 1]);
			mListOfInstatiatedSets.RemoveAt(mListOfInstatiatedSets.Count - 1);
			ChooseEnvironmentToSpawn(tLastSetPos, eSetTire.MiniGame);
        }
	}

    public void ChallengeTypeSetInstantiation()
    {
        _eSetTire = eSetTire.Challenge;
        mbInstantiateChallengeSet = true;
        SetPosition();
    }

    public Vector3 GetNextPlatformPosition(float MaxDistanceOn_X_Axis, float DistanceOn_Z_Axis, float X_Axis)
    {
        Vector3 tRequiredPosition = Vector3.zero;
        for (int i = 0; i < _listOfPlatformHandler.Count; i++)
        {
            PlatformHandler element = _listOfPlatformHandler[i];
            Vector3 tPos = element.transform.position;
		    if (Mathf.Abs(tPos.x - X_Axis) <= MaxDistanceOn_X_Axis & tPos.z - _vCurrentPlatformPosition.z <= DistanceOn_Z_Axis & tPos.z - _vCurrentPlatformPosition.z >= DistanceOn_Z_Axis)
			{
				tRequiredPosition = tPos;
                break;
		    }
        }
        return tRequiredPosition;
    }

    void Update()
    {
        EnvironmentStateHandler();
        InitializeTheNextSet();
    }

    void EnvironmentStateHandler()
    {
		switch (meEnvironmentState)
		{
			case eEnvironmentState.None:
				break;

			case eEnvironmentState.Instatiate:
				miSetCount += 1;
				SetPosition();

				if (miSetCount > _iNumberOfActiveTilesOnScreen)
				{
					Destroy(mListOfInstatiatedSets[0]);
					mListOfInstatiatedSets.RemoveAt(0);
				}

				meEnvironmentState = eEnvironmentState.None;
				break;
		}
    }

    void SetPosition()
    {
        if (miSetCount == 1 & !mbInstantiateMinigameSet & !mbInstantiateChallengeSet)
        {
            ChooseEnvironmentToSpawn(new Vector3(0, 0, 0), eSetTire.Low);
        }

        else if (miSetCount == 2 & !mbInstantiateMinigameSet & !mbInstantiateChallengeSet)
        {
            ChooseEnvironmentToSpawn(new Vector3(0, 0, mfSetPosZ), eSetTire.Low);
        }

        else if (miSetCount > 2 & !mbInstantiateMinigameSet & !mbInstantiateChallengeSet)
        {
			if (Time.time - mfStartTime < mfLowTireSetInstantiationTimeLimit)
			{
				mfSetPosZ += DataHandler._fEnvironmentSetLength;
				ChooseEnvironmentToSpawn(new Vector3(0, 0, mfSetPosZ), eSetTire.Low);
			}

			else if (Time.time - mfStartTime > mfLowTireSetInstantiationTimeLimit & Time.time - mfStartTime < mfMediumTireSetInstantiationTimeLimit)
			{
				mfSetPosZ += DataHandler._fEnvironmentSetLength;
				ChooseEnvironmentToSpawn(new Vector3(0, 0, mfSetPosZ), eSetTire.Medium);
			}

			else if (Time.time - mfStartTime > mfMediumTireSetInstantiationTimeLimit)
			{
				mfSetPosZ += DataHandler._fEnvironmentSetLength;
				ChooseEnvironmentToSpawn(new Vector3(0, 0, mfSetPosZ), eSetTire.High);
			}
        }

        else if (mbInstantiateMinigameSet & !mbInstantiateChallengeSet)
        {
            mfSetPosZ += DataHandler._fEnvironmentSetLength;
            ChooseEnvironmentToSpawn(new Vector3(0, 0, mfSetPosZ), eSetTire.MiniGame);
        }

        else if (miSetCount == 1 & mbInstantiateChallengeSet)
        {
            miChallengeSetPrefabIndex = 0;
            ChooseEnvironmentToSpawn(new Vector3(0, 0, 0), eSetTire.Challenge);
        }

        else if (miSetCount == 2 & mbInstantiateChallengeSet)
        {
            if (miChallengeSetPrefabIndex < _listOfChallengeSets.Count)
                miChallengeSetPrefabIndex += 1;
            
            ChooseEnvironmentToSpawn(new Vector3(0, 0, mfSetPosZ), eSetTire.Challenge);    
        }

        else if (miSetCount > 2 & mbInstantiateChallengeSet)
        {
            if (miChallengeSetPrefabIndex < _listOfChallengeSets.Count)
                miChallengeSetPrefabIndex += 1;
            
            mfSetPosZ += DataHandler._fEnvironmentSetLength;
            ChooseEnvironmentToSpawn(new Vector3(0, 0, mfSetPosZ), eSetTire.Challenge);
        }
    }

	void InitializeTheNextSet()
	{
		if (miCount == 1)
		{
			if (_playerManager._playerHandler._tPlayerTransform.position.z > mfPlayerTravelHowFarBeforeTheNextSetGenerate)
			{
				meEnvironmentState = eEnvironmentState.Instatiate;
				mfPlayerTravelHowFarBeforeTheNextSetGenerate += DataHandler._fEnvironmentSetLength;
				miCount += 1;
			}
		}

		else if (miCount > 1)
		{
			if (_playerManager._playerHandler._tPlayerTransform.position.z > mfPlayerTravelHowFarBeforeTheNextSetGenerate)
			{
				meEnvironmentState = eEnvironmentState.Instatiate;
				mfPlayerTravelHowFarBeforeTheNextSetGenerate += DataHandler._fEnvironmentSetLength;
			}
		}
	}

    public bool ComparePlatformAndPlayerPositionForLanding(Vector3 PlayerPosition, float Difference)
    {
        bool bRequiredState = false;
        for (int i = 0; i < _listOfPlatformHandler.Count; i++)
        {
            PlatformHandler element = _listOfPlatformHandler[i];
            if (element._eSupportType == ePlatformHandlerType.Fixed)
            {
                if (Vector3.Distance(element.GetTransform.position, PlayerPosition) < Difference)
                {
					bRequiredState = true;
					break;
                }
            }
        }
        return bRequiredState;
    }

    public Vector3 ComparePlatformAndPlayerPositionForReSpawning(Vector3 PlayerPosition, float MaxDifferenceOnXAxis, float MaxSpawningDistance)
    {
        Vector3 tRequiredPosition = Vector3.zero;
        for (int i = 0; i < _listOfPlatformHandler.Count; i++)
        {
            PlatformHandler element = _listOfPlatformHandler[i];
            if (element.GetTransform.position.z < PlayerPosition.z & Mathf.Abs(element.GetTransform.position.z - PlayerPosition.z) < MaxSpawningDistance)
            {
                if (Mathf.Abs(PlayerPosition.x - element.GetTransform.position.x) < MaxDifferenceOnXAxis)
                {
                    tRequiredPosition = element.GetTransform.position;
                    break;
                }
            }
        }
        return tRequiredPosition;
    }

    bool ConditionForDistanceChecking(Vector3 FirstPosition, Vector3 SecondPosition, float MinDistance)
	{
        bool bRequiredState = false;
		if (Mathf.Abs(FirstPosition.x - SecondPosition.x) > MinDistance & Mathf.Abs(FirstPosition.z - SecondPosition.z) > MinDistance)
            bRequiredState = true;
		return bRequiredState;
	}

    public void SetCurrentPlatformPosition(float X_Pos, float Y_Pos, float Z_Pos)
    {
        _vCurrentPlatformPosition = new Vector3(X_Pos, Y_Pos, Z_Pos);
    }

    public void UpdatePlatformState(float centerPoint, ePlatformHandlerType platformState, bool state)
    {
        for (int i = 0; i < _listOfPlatformHandler.Count; i++)
        {
            PlatformHandler element = _listOfPlatformHandler[i];
			if (Mathf.Abs(element.GetTransform.position.z - centerPoint) <= 50f)
			{
				element.GetComponent<MeshRenderer>().enabled = state;
				element._eSupportType = platformState;
			}
        }
    }
}