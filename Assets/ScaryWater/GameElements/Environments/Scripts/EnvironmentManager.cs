using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    eSetType meSetType = eSetType.None;
    int miCount = 0;
    int miSetHandlerPrefabIndex = 0;
    int miSetPrefabIndex = 0;
    int miChallengeSetPrefabIndex = 0;
    int miNumberOfSetSpawnAtATime = 5;
    int miNumberOfSetsToDeleteFromBack = 1;
    float mfLowTireSetInstantiationTimeLimit = 45f;
    float mfMediumTireSetInstantiationTimeLimit = 75f;
    float mfStartTime;
    bool mbInstantiateMinigameSet = false;
    bool mbInstantiateChallengeSet = false;
    Vector3 mvPositionForSet = Vector3.zero;
    static EnvironmentManager mInstance = null;
    List<EnvironmentHandler> mListOfEnvironmentHandler = new List<EnvironmentHandler>();

    public AutoIntensityScr _AutoIntensityScr;
    public DayNightHandler _DayNightHandlerScr;
    public EnvironmentHandler _currentActiveEnvironmentHandler = null;
    public GenerateRandomValueScr _GenerateRandomValueScr;
    public List<GameObject> _listOfLowTireSets;
    public List<GameObject> _listOfMediumTireSets;
    public List<GameObject> _listOfHighTireSets;
    public List<GameObject> _listOfChallengeSets;
    public List<MiniGameSetHandler> _listOfMiniGameSets = new List<MiniGameSetHandler>();
    public PlayerManager _playerManager; 
    public Vector3 _vCurrentPlatformPosition;
    public List<PlatformHandler> _listOfPlatformHandler = new List<PlatformHandler>();
    public eSetTire _eSetTire = eSetTire.Low;
    public ParticleSystem _RippleLeaf;
    public ParticleSystem _RippleDeath;
    public bool _bDisableIndicator = false;

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

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Challenge_GamePlay_01")
            ChallengeTypeSetInstantiation();

        else
            _eSetTire = eSetTire.Low;
    }

    void Start()
    {
        _GenerateRandomValueScr = new GenerateRandomValueScr();
        mfStartTime = Time.time;
        SetTireOfEnvToSpawn();
    }

    void Update()
    {
        InitializeTheNextSets();
        SetFireflyParticleState();
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
            InstantiateSet(Position, _listOfChallengeSets[miChallengeSetPrefabIndex]);
	}

	void InstantiateSet(Vector3 Position, GameObject currentSet)
	{
        GameObject tGoSet = Instantiate(currentSet);
        tGoSet.transform.SetParent(this.transform);
        tGoSet.transform.position = Position;
		_currentActiveEnvironmentHandler = tGoSet.GetComponent<EnvironmentHandler>();
		_currentActiveEnvironmentHandler._environmentManager = this;
		mListOfEnvironmentHandler.Add(_currentActiveEnvironmentHandler);
	}

	public void MiniGameTypeSetInstantiation(eSetType setType, bool state, bool miniGameState)
	{
		meSetType = setType;
		mbInstantiateMinigameSet = state;

        if (miniGameState)
            ManipulateInstantiatedSets();

        if (!state)
            ManipulateInstantiatedSets();
	}

    void ManipulateInstantiatedSets()
    {
        int tMinIndex = 0;
        for (int i = 0; i < mListOfEnvironmentHandler.Count; i++)
        {
            if (_playerManager._playerHandler._tPlayerTransform.position.z > mListOfEnvironmentHandler[i].transform.position.z)
            {
                tMinIndex = i + 2;
                break;
            }
        }

        mvPositionForSet = mListOfEnvironmentHandler[tMinIndex].transform.position;

        for (int i = tMinIndex; i < mListOfEnvironmentHandler.Count; i++)
            Destroy(mListOfEnvironmentHandler[i].gameObject);

        int tRemove = mListOfEnvironmentHandler.Count - tMinIndex;
        mListOfEnvironmentHandler.RemoveRange(tMinIndex, tRemove);

        int tRange = (mListOfEnvironmentHandler.Count - 1) - tMinIndex;
        SetTireOfEnvToSpawn(tRange + 1);
    }

    public void ChallengeTypeSetInstantiation()
    {
        _eSetTire = eSetTire.Challenge;
        mbInstantiateChallengeSet = true;
    }

    void SetTireOfEnvToSpawn(int limit = 5)
    {
        int tLimit = limit;
        if (mListOfEnvironmentHandler.Any())
            tLimit = miNumberOfSetSpawnAtATime - mListOfEnvironmentHandler.Count;
        
        for (int i = 0; i < tLimit; i++)
        {
            if (!mbInstantiateMinigameSet & !mbInstantiateChallengeSet)
            {
                if (Time.time - mfStartTime < mfLowTireSetInstantiationTimeLimit)
                    ChooseEnvironmentToSpawn(mvPositionForSet, eSetTire.Low);

                else if (Time.time - mfStartTime > mfLowTireSetInstantiationTimeLimit & Time.time - mfStartTime < mfMediumTireSetInstantiationTimeLimit)
                    ChooseEnvironmentToSpawn(mvPositionForSet, eSetTire.Medium);

                else if (Time.time - mfStartTime > mfMediumTireSetInstantiationTimeLimit)
                    ChooseEnvironmentToSpawn(mvPositionForSet, eSetTire.High);
            }

            else if (mbInstantiateMinigameSet & !mbInstantiateChallengeSet)
                ChooseEnvironmentToSpawn(mvPositionForSet, eSetTire.MiniGame);

            else if (!mbInstantiateMinigameSet & mbInstantiateChallengeSet)
            {
                ChooseEnvironmentToSpawn(mvPositionForSet, eSetTire.Challenge);

                if (miChallengeSetPrefabIndex < _listOfChallengeSets.Count - 1)
                    miChallengeSetPrefabIndex++;
            }

            mvPositionForSet.x = 0f;
            mvPositionForSet.y = 0f;
            mvPositionForSet.z += DataHandler._fEnvironmentSetLength;
        }
    }

    void InitializeTheNextSets()
	{
        if (!mListOfEnvironmentHandler.Any())
            return;
        
        float tSetPos = mListOfEnvironmentHandler[miNumberOfSetsToDeleteFromBack + 1].transform.position.z;
        if (_playerManager._playerHandler._tPlayerTransform.position.z > tSetPos)
        {
            if (miCount > 0)
                DeleteSetsFromBack();
            
            if (miCount < 2)
                miCount++;
            
            SetTireOfEnvToSpawn();
        }
	}

    void DeleteSetsFromBack()
    {
        if (!mListOfEnvironmentHandler.Any())
            return;
        
        if (mListOfEnvironmentHandler.Count < miNumberOfSetSpawnAtATime)
            return;
        
        for (int i = 0; i < miNumberOfSetsToDeleteFromBack & miNumberOfSetsToDeleteFromBack < mListOfEnvironmentHandler.Count; i++)
        {
            Destroy(mListOfEnvironmentHandler[0].gameObject);
            mListOfEnvironmentHandler.RemoveAt(0);
        }
    }

    public bool ComparePlatformAndPlayerPositionForLanding(Vector3 vPlayerPosition, float fDifference, out int iDeathLane)
    {
        bool bRequiredState = true; // Testing 
        iDeathLane = 0;
        for (int i = 0; i < _listOfPlatformHandler.Count; i++)
        {
            PlatformHandler element = _listOfPlatformHandler[i];

            if (Vector3.Distance(element.GetTransform.position, vPlayerPosition) < fDifference)
            {
                if (element._eSupportType == ePlatformHandlerType.Fixed)
                {
                    bRequiredState = true;
                    element.PlayRippleParticle(_RippleLeaf);
                    break;
                }
                else
                    element.PlayRippleParticle(_RippleDeath);

                float tRelativePos = element.GetTransform.position.x / DataHandler._fSpaceBetweenLanes;
                int tLane = Mathf.RoundToInt(tRelativePos);
                iDeathLane = tLane;
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

    public void SetCurrentPlatformPosition(float X_Pos, float Y_Pos, float Z_Pos)
    {
        _vCurrentPlatformPosition = new Vector3(X_Pos, Y_Pos, Z_Pos);
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

    void SetFireflyParticleState()
    {
        for (int i = 0; i < mListOfEnvironmentHandler.Count; i++)
        {
            EnvironmentHandler tEnvHandlerScr = mListOfEnvironmentHandler[i];
            if (_DayNightHandlerScr._bNightTime)
            {
                if (tEnvHandlerScr._Firefly != null)
                    tEnvHandlerScr._Firefly.Play(); 
            }
            else
            {
                if (tEnvHandlerScr._Firefly != null)
                    tEnvHandlerScr._Firefly.Stop();
            }
        }
    }
}
