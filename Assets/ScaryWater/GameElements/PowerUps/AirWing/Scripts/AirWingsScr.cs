using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eAirWingState
{
    None = 0,
    CatchThePlayer,
    MoveThroughPath,
    InCaseFailure
}

public class AirWingsScr : MonoBehaviour 
{
    eAirWingState meAirWingState = eAirWingState.None;
    Transform mtPlayerTransform;
    Transform mtAirWingTransform;
    Vector3 mvFinalDestination;
    Vector3 mvOffset;
    Vector3 mvTempPos;
    AirWingsPathScr mAirWingsPathScr;
    float mfSqrLen;
    bool mbPlayerTriggerEnterChecking = true;
    bool mbForceStateChanged = false;

    public FriendManager _friendManager;
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
    public Vector3 _vLandingPadPosition;
    public float _fSpeedA = 50f;
    public float _fSpeedB = 12.5f;
    public float _fRotationSpeed = 5f;
    public float _fLandingPadXPos;

    void Start()
    {
        mtAirWingTransform = transform;
        _friendManager._listOfFriends.Add(this.transform);
        mtPlayerTransform = _friendManager._playerManager._playerHandler.transform;
        meAirWingState = eAirWingState.CatchThePlayer;
    }

    void Update()
    {
        RemoveOtherFriendFromScene();
        ForceChangeState();
    }

    void FixedUpdate()
    {
        AirWingStateHandler();    
    }

    void RemoveOtherFriendFromScene()
    {
        if (meAirWingState == eAirWingState.MoveThroughPath)
        {
            if (mAirWingsPathScr._eAirWingPathState == eAirWingPathState.PointA | mAirWingsPathScr._eAirWingPathState == eAirWingPathState.PointB | mAirWingsPathScr._eAirWingPathState == eAirWingPathState.PointC)
                _friendManager.RemoveOtherFriendsIfWithinAirwingsDropPointRange();
        }
    }

    void ForceChangeState()
    {
        if (mbForceStateChanged)
            return;

        if (PlayerManager.Instance.GetPlayerDeadState() | FriendManager.GetIfPlayerIsCloseToAFriend())
        {
            mbForceStateChanged = true;
            transform.GetComponent<SphereCollider>().enabled = false;
            DataManager.AddToAirwingAmount(1);
            FriendManager.SetPlayerIsWithFriendState(false);
            FriendManager.SetAirwingActiveState(false);
            FriendManager.SetIfPlayerIsCloseToAFriend(false);

            GameObject tCanvas = UICanvasHandler.Instance.GetActiveCanvasByName("HUDCanvas");
            if (tCanvas != null)
            {
                GameplayAreaUIHandler tScr = tCanvas.GetComponent<GameplayAreaUIHandler>();
                tScr.DisplayAirwingCount();
                tScr.SetAirwingBtnState(true);
            }

            mvTempPos.x = transform.position.x;
            mvTempPos.y = transform.position.y + 50f;
            mvTempPos.z = transform.position.z + 100f;
            mvFinalDestination = mvTempPos;
            meAirWingState = eAirWingState.InCaseFailure;
        }
    }

    void AirWingStateHandler()
    {
        switch (meAirWingState)
        {
            case eAirWingState.CatchThePlayer:
                transform.position = Vector3.MoveTowards(mtAirWingTransform.position, mtPlayerTransform.position, Time.deltaTime * _fSpeedA);
                SmoothLook(mtPlayerTransform.position);
                break;

            case eAirWingState.MoveThroughPath:
                switch (mAirWingsPathScr._eAirWingPathState)
                {
                    case eAirWingPathState.PointA:
                        MovingAirWingAlongGivenPath(mAirWingsPathScr._tPointA.position, _fSpeedB, eAirWingPathState.PointB);
                        break;

                    case eAirWingPathState.PointB:
                        MovingAirWingAlongGivenPath(mAirWingsPathScr._tPointB.position, _fSpeedB, eAirWingPathState.PointC);
                        break;

                    case eAirWingPathState.PointC:
                        mvTempPos.x = _fLandingPadXPos;
                        mvTempPos.y = mAirWingsPathScr._tPointC.position.y;
                        mvTempPos.z = mAirWingsPathScr._tPointC.position.z;
                        MovingAirWingAlongGivenPath(mvTempPos, _fSpeedB, eAirWingPathState.PointD, true);
                        break;

                    case eAirWingPathState.PointD:
                        MovingAirWingAlongGivenPath(mAirWingsPathScr._tPointD.position, _fSpeedB, eAirWingPathState.PointE1, false, true);
                        break;

                    case eAirWingPathState.PointE1:
                        MovingAirWingAlongGivenPath(mAirWingsPathScr._tPointE1.position, _fSpeedB, eAirWingPathState.PointF1);
                        break;

                    case eAirWingPathState.PointE2:
                        MovingAirWingAlongGivenPath(mAirWingsPathScr._tPointE2.position, _fSpeedB, eAirWingPathState.PointF2);
                        break;

                    case eAirWingPathState.PointF1:
                        MovingAirWingAlongGivenPath(mAirWingsPathScr._tPointF1.position, _fSpeedB, eAirWingPathState.PointF1, false, false, false, true);
                        break;

                    case eAirWingPathState.PointF2:
                        MovingAirWingAlongGivenPath(mAirWingsPathScr._tPointF2.position, _fSpeedB, eAirWingPathState.PointF2, false, false, false, true);
                        break;
                }
                break;

            case eAirWingState.InCaseFailure:
                GoToFinalDestination();
                break;
        }
    }

    void MovingAirWingAlongGivenPath(Vector3 targetPos, float movingSpeed, eAirWingPathState nextState, bool dropThePlayer = false, bool chooseLane = false, bool keepMoving = true, bool destroyAirwing = false)
    {
        transform.position = Vector3.MoveTowards(mtAirWingTransform.position, targetPos, Time.deltaTime * movingSpeed);
        SmoothLook(targetPos);

        mvOffset = mtAirWingTransform.position - targetPos;
        mfSqrLen = mvOffset.sqrMagnitude;
        if (mfSqrLen < 2f)
        {
            if (dropThePlayer)
            {
                transform.GetComponent<SphereCollider>().enabled = false;
                FriendManager.SetAirwingActiveState(false);
                FriendManager.SetPlayerIsWithFriendState(false);
                MovePlayerTowardsLandingPad();
            }

            if (chooseLane)
            {
                int tValue = Random.Range(0, 1);
                if (tValue == 0)
                    nextState = eAirWingPathState.PointE1;
                else
                    nextState = eAirWingPathState.PointE2;
            }

            if (keepMoving)
                mAirWingsPathScr._eAirWingPathState = nextState;

            if (destroyAirwing)
            {
                Destroy(mAirWingsPathScr.gameObject);
                Destroy(this.gameObject);
            }
        }
    }

    void GoToFinalDestination()
    {
        transform.position = Vector3.MoveTowards(mtAirWingTransform.position, mvFinalDestination, Time.deltaTime * _fSpeedB);
        SmoothLook(mvFinalDestination);

        mvOffset = mvFinalDestination - mtAirWingTransform.position;
        mfSqrLen = mvOffset.sqrMagnitude;
        if (mfSqrLen < 2f * 2f)
        {
            meAirWingState = eAirWingState.None;
            Destroy(this.gameObject);
        }
    }

    void MovePlayerTowardsLandingPad()
	{
        mtPlayerTransform.position = Vector3.MoveTowards(mtPlayerTransform.position, _vLandingPadPosition, Time.deltaTime * 5f);

        if (_vLandingPadPosition.y - mtPlayerTransform.position.y < 0.1f)
        {
            UpdatePlayerStatesOnDrop();
            GameObject tCanvas = UICanvasHandler.Instance.GetActiveCanvasByName("HUDCanvas");
            if (tCanvas != null)
                tCanvas.GetComponent<GameplayAreaUIHandler>().SetAirwingBtnState(true);
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (FriendManager.GetPlayerIsWithFriendState() | _friendManager._playerManager.GetPlayerDeadState())
            return;

		if (other.CompareTag("Player"))
		{
			if (mbPlayerTriggerEnterChecking)
			{
				mbPlayerTriggerEnterChecking = false;
                FriendManager.SetPlayerIsWithFriendState();

                if (ScoreHandler._OnScoreEventCallback != null)
                    ScoreHandler._OnScoreEventCallback(eScoreType.Dragonfly);
                
                InstantiatePathForAirwing();
                SetupLandingPad();
                UpdatePlayerStatesOnTriggerEnter();
                SetPlayerPositionForAirWings();

                meAirWingState = eAirWingState.MoveThroughPath;
                mAirWingsPathScr._eAirWingPathState = eAirWingPathState.PointA;
			}
		}
    }

    void InstantiatePathForAirwing()
    {
        GameObject goAirwingPath = Instantiate(FriendManager.Instance._airWingsMovingPointsPrefab);
        goAirwingPath.transform.SetParent(_friendManager.transform);
        mvTempPos.x = 0f;
        mvTempPos.y = 0f;
        mvTempPos.z = _friendManager._playerManager._playerHandler._tPlayerTransform.position.z + 50f;
        goAirwingPath.transform.position = mvTempPos;
        mAirWingsPathScr = goAirwingPath.GetComponent<AirWingsPathScr>();
        mAirWingsPathScr._AirWingsScr = this;
    }

    void SetupLandingPad()
    {
        mvTempPos.x = _fLandingPadXPos;
        mvTempPos.y = -1f;
        mvTempPos.z = SetLandingPadProperPosition((int)mAirWingsPathScr._tPointC.position.z);
        _vLandingPadPosition = mvTempPos;
        EnvironmentManager.Instance.UpdatePlatformState(_vLandingPadPosition.z, ePlatformHandlerType.Fixed, true);
    }

    void UpdatePlayerStatesOnTriggerEnter()
    {
        _friendManager._playerManager._playerHandler._playerPropertiesScr.SetPlayerParent(this.transform);
        _friendManager._playerManager._CameraControllerScr.CameraFollowPlayerOnYAxis();
        _friendManager._playerManager._playerHandler._playerPropertiesScr.SetPlayerIdle();
        _friendManager._playerManager._playerHandler._playerPropertiesScr.SetPlayerPhysic();
    }

    void UpdatePlayerStatesOnDrop()
    {
        _friendManager._playerManager._playerHandler._playerPropertiesScr.SetPlayerParent();
        _friendManager._playerManager._playerHandler._playerPropertiesScr.SetPlayerRotation();
        _friendManager._playerManager._playerHandler._playerPropertiesScr.SetPlayerPhysic(true);
        _friendManager._playerManager._playerHandler._playerPropertiesScr.SetPlayerControlState();
        _friendManager._playerManager._playerHandler._playerPropertiesScr.SetPlayerRequiredAndNextPlatformPosition(_vLandingPadPosition);
        _friendManager._playerManager._CameraControllerScr.CameraFollowPlayerOnYAxis(false);
        _friendManager._playerManager._playerHandler.DoSingleJump();
    }

    void SmoothLook(Vector3 Direction)
	{
        Vector3 diff = Direction - transform.localPosition;
        if (Vector3.SqrMagnitude(diff) > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(diff);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _fRotationSpeed * Time.deltaTime);
        }
	}

    void SetPlayerPositionForAirWings()
    {
        FriendManager._eFriendType = eFriendType.Dragonfly;
        Vector3 airWingPosition = transform.position;
        mvTempPos.x = airWingPosition.x;
        mvTempPos.y = airWingPosition.y - 0.85f;
        mvTempPos.z = airWingPosition.z;
        _friendManager._playerManager._playerHandler._playerPropertiesScr.SetPlayerPosition(mvTempPos.x, mvTempPos.y, mvTempPos.z);
    }

    int SetLandingPadProperPosition(int landingPos)
    {
        int tValue = 0;
        string tStringValue = landingPos.ToString();
        if (tStringValue.EndsWith(tValue.ToString(), System.StringComparison.CurrentCulture))
            return landingPos;
        else
        {
            tStringValue = tStringValue.Remove(tStringValue.Length - 1);
            tValue = int.Parse(tStringValue + 5);
            return tValue += 5;    
        }
    }

    void OnDisable()
    {
        _friendManager._listOfFriends.Remove(this.transform);
    }
}
