using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdHandler : MonoBehaviour
{
    public enum eBirdType
    {
        None = 0,
        Kingfisher,
        Dragonfly
    }

    public enum eBirdState
    {
        Idle = 0,
        PointA,
        PointB,
        PointC,
        PointD,
        PointE,
        PointF,
        PointG1,
        PointG2,
        PointH1,
        PointH2
    }

	eBirdState meBirdState = eBirdState.Idle;
	Vector3 mvLandingPadPosition = Vector3.zero;
    Vector3 mvTempPos;
    bool mbDetectPlayer;
    bool mbGiveMiniGamePoints = true;

    public eBirdType _eBirdType = eBirdType.None;
    public Transform _tBird;
    public Transform _tPointA;
    public Transform _tPointB;
    public Transform _tPointC;
    public Transform _tPointD;
    public Transform _tPointE;
    public Transform _tPointF;
    public Transform _tPointG1;
    public Transform _tPointG2;
    public Transform _tPointH1;
    public Transform _tPointH2;
    public float _fSpeedA = 20f;
    public float _fSpeedB = 20f;
    public float _fPlayerLandingSpeed = 5f;
    public float _fRotationSpeed = 2.5f;
    public float _fPickUpPositionOnZAxis;
    public BirdInitiate _BirdInitiateScr;

	void OnEnable()
	{
        FriendManager.Instance._listOfFriends.Add(this.transform);
	}

    void Start()
    {
        DetectBirdType("KingfisherIncomingSound", "DragonflyIncomingSound", false);
        mvLandingPadPosition = new Vector3(_tPointE.position.x, 0f, _tPointE.position.z);
        SetVisibilityOfMovingPoints();
        _BirdInitiateScr._InitiateBird += StartMoving;
        _fPickUpPositionOnZAxis = _tPointB.position.z;
    }

    void StartMoving()
    {
        meBirdState = eBirdState.PointA;
    }

    void FixedUpdate()
    {
        BirdStateHandler();
    }

    void BirdStateHandler()
    {
        switch(meBirdState)
        {
            case eBirdState.PointA:
                transform.localPosition = _tPointA.localPosition;

                if(_tPointA.localPosition.z - transform.localPosition.z < 0.1f)
                    meBirdState = eBirdState.PointB;
                break;

            case eBirdState.PointB:
                MovingBirdAlongGivenPath(_tPointB.localPosition, _fSpeedA, eBirdState.Idle, true, false);
                break;

            case eBirdState.PointC:
                MovingBirdAlongGivenPath(_tPointC.localPosition, _fSpeedB, eBirdState.PointD, false);
                break;

            case eBirdState.PointD:
                MovingBirdAlongGivenPath(_tPointD.localPosition, _fSpeedB, eBirdState.PointE, false);
                break;

            case eBirdState.PointE:
                MovingBirdAlongGivenPath(_tPointE.localPosition, _fSpeedB, eBirdState.PointF, false, true, true);
                break;

            case eBirdState.PointF:
                MovingBirdAlongGivenPath(_tPointF.localPosition, _fSpeedA, eBirdState.Idle, false, false, false, true);
                break;

            case eBirdState.PointG1:
                MovingBirdAlongGivenPath(_tPointG1.localPosition, _fSpeedA, eBirdState.PointH1);
                break;

            case eBirdState.PointG2:
                MovingBirdAlongGivenPath(_tPointG2.localPosition, _fSpeedA, eBirdState.PointH2);
                break;

            case eBirdState.PointH1:
                MovingBirdAlongGivenPath(_tPointH1.localPosition, _fSpeedA, eBirdState.Idle, false, false, false, false, true);
                break;

            case eBirdState.PointH2:
                MovingBirdAlongGivenPath(_tPointH2.localPosition, _fSpeedA, eBirdState.Idle, false, false, false, false, true);
                break;
        }
    }

    void MovingBirdAlongGivenPath(Vector3 targetPos, float movingSpeed, eBirdState nextState, bool detectPlayer = false, bool setNextState = true, bool dropThePlayer = false, bool chooseLane = false, bool destroyBird = false)
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, Time.deltaTime * movingSpeed);
        SmoothLook(targetPos);

        if (targetPos.z - transform.localPosition.z < 0.1f)
        {
            if (detectPlayer)
                mbDetectPlayer = true;
            
            if (setNextState)
                meBirdState = nextState;
            
            if (dropThePlayer)
            {
                transform.GetComponent<SphereCollider>().enabled = false;
                if (mbGiveMiniGamePoints)
                {
                    mbGiveMiniGamePoints = false;
                    if (MiniGameManager.Instance._eMiniGameState == eMiniGameState.AcceptFriendHelp || MiniGameManager.Instance._eMiniGameState == eMiniGameState.AvoidFriend)
                        MiniGameManager.Instance._iFriendsHelpAccepted += 1;
                }
                MovePlayerTowardsLandingPad();
            }
            
            if (chooseLane)
            {
                int tValue = Random.Range(0, 1);
                if (tValue == 0)
                    meBirdState = eBirdState.PointG1;
                else
                    meBirdState = eBirdState.PointG2;
            }

            if (destroyBird)
                Destroy(this.gameObject);
        }
    }

    void MovePlayerTowardsLandingPad()
    {
        PlayerManager.Instance._playerHandler._tPlayerTransform.position = Vector3.MoveTowards(PlayerManager.Instance._playerHandler._tPlayerTransform.position, mvLandingPadPosition, Time.deltaTime * _fPlayerLandingSpeed);

        if (mvLandingPadPosition.y - PlayerManager.Instance._playerHandler._tPlayerTransform.position.y < 0.1f)
        {
            FriendManager.SetPlayerIsWithFriendState(false);
            UpdatePlayerStatesOnDrop();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (FriendManager.GetPlayerIsWithFriendState())
            return;

        if (other.CompareTag("Player"))
        {
            if (mbDetectPlayer)
            {
                mbDetectPlayer = false;
                if (ScoreHandler._OnScoreEventCallback != null)
                {
                    if (_eBirdType.Equals(eBirdType.Kingfisher))
                        ScoreHandler._OnScoreEventCallback(eScoreType.Kingfisher);

                    else if (_eBirdType.Equals(eBirdType.Dragonfly))
                        ScoreHandler._OnScoreEventCallback(eScoreType.Dragonfly);
                }
                FriendManager.SetPlayerIsWithFriendState(true);
                UpdatePlayerStatesOnTriggerEnter();
                DetectBirdType();
                meBirdState = eBirdState.PointC;
            }
        }
    }

    void UpdatePlayerStatesOnTriggerEnter()
    {
        PlayerManager.Instance._playerHandler._playerPropertiesScr.SetPlayerParent(this.transform);
        PlayerManager.Instance._CameraControllerScr.CameraFollowPlayerOnYAxis();
        PlayerManager.Instance._playerHandler._playerPropertiesScr.SetPlayerIdle();
        PlayerManager.Instance._playerHandler._playerPropertiesScr.SetPlayerPhysic();
    }

    void UpdatePlayerStatesOnDrop()
    {
        PlayerManager.Instance._playerHandler._playerPropertiesScr.SetPlayerParent();
        PlayerManager.Instance._playerHandler._playerPropertiesScr.SetPlayerRotation();
        PlayerManager.Instance._playerHandler._playerPropertiesScr.SetPlayerPhysic(true);
        PlayerManager.Instance._playerHandler._playerPropertiesScr.SetPlayerControlState();
        PlayerManager.Instance._playerHandler._playerPropertiesScr.SetPlayerRequiredAndNextPlatformPosition(mvLandingPadPosition);
        PlayerManager.Instance._CameraControllerScr.CameraFollowPlayerOnYAxis(false);
        PlayerManager.Instance._playerHandler.DoSingleJump();
    }

    void SmoothLook(Vector3 Direction)
	{
		Vector3 diff = Direction - transform.localPosition;
		if (Vector3.SqrMagnitude (diff) > 0) 
        {
            Quaternion targetRotation = Quaternion.LookRotation (diff);
			transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, _fRotationSpeed * Time.deltaTime);
		}
	}

    void DetectBirdType(string audioKingfisher = "KingfisherFlight", string audioDragonfly = "DragonflyFlight", bool state = true)
    {
        if (this._eBirdType == eBirdType.Kingfisher)
        {
            //CEffectsPlayer.Instance.Play(audioKingfisher);
            FriendManager._eFriendType = eFriendType.Kingfisher;
            if (state)
                SetPlayerPositionForKingfisher();
        }

        else if (this._eBirdType == eBirdType.Dragonfly)
        {
            //CEffectsPlayer.Instance.Play(audioDragonfly);
            FriendManager._eFriendType = eFriendType.Dragonfly;
            if (state)
                SetPlayerPositionForDragonfly();
        }
    }

    void SetPlayerPositionForKingfisher()
    {
        Vector3 birdPosition = transform.position;
        mvTempPos.x = birdPosition.x;
        mvTempPos.y = birdPosition.y - 1.25f;
        mvTempPos.z = birdPosition.z - 0.35f;
        PlayerManager.Instance._playerHandler._playerPropertiesScr.SetPlayerPosition(mvTempPos.x, mvTempPos.y, mvTempPos.z);
    }

    void SetPlayerPositionForDragonfly()
    {
        Vector3 birdPosition = transform.position;
        mvTempPos.x = birdPosition.x;
        mvTempPos.y = birdPosition.y - 0.85f;
        mvTempPos.z = birdPosition.z;
        PlayerManager.Instance._playerHandler._playerPropertiesScr.SetPlayerPosition(mvTempPos.x, mvTempPos.y, mvTempPos.z);
    }

    void SetVisibilityOfMovingPoints(bool state = false)
    {
        _tPointA.GetComponent<MeshRenderer>().enabled = state;
        _tPointB.GetComponent<MeshRenderer>().enabled = state;
        _tPointC.GetComponent<MeshRenderer>().enabled = state;
        _tPointD.GetComponent<MeshRenderer>().enabled = state;
        _tPointE.GetComponent<MeshRenderer>().enabled = state;
        _tPointF.GetComponent<MeshRenderer>().enabled = state;
        _tPointG1.GetComponent<MeshRenderer>().enabled = state;
        _tPointG2.GetComponent<MeshRenderer>().enabled = state;
        _tPointH1.GetComponent<MeshRenderer>().enabled = state;
        _tPointH2.GetComponent<MeshRenderer>().enabled = state;
    }

    void OnDisable()
    {
        FriendManager.Instance._listOfFriends.Remove(this.transform);
        if (_BirdInitiateScr._InitiateBird != null)
            _BirdInitiateScr._InitiateBird -= StartMoving;
    }
}
