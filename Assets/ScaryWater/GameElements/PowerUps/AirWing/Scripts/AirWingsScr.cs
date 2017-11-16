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
    Vector3 mvAirWingMovePoint;
    Vector3 mvPlayerDropPoint;
    Vector3 mvLandingPadPosition;
    Vector3 mvFinalDestination;
    Vector3 mvOffset;
    Vector3 mvTempPos;
    AirWingsMovingPointsScr mAirWingsPathScr;
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
    public float _fBirdMoveTowardPlayerSpeed = 50f;
    public float _fBirdMoveForwardSpeed = 25f;
    public float _fBirdGettingOutOfTheSceneSpeed = 25f;
    public float _fPlayerDropDistance = 150f;
    public float _fRotationSpeed = 5f;
    public float _landingXPos;

	void OnDestroy()
	{
        _friendManager._listOfFriends.Remove(this.transform);
        _friendManager._listOfAirWingsScr.Remove(this);
	}

    void Start()
    {
        mtAirWingTransform = transform;
        _friendManager._listOfFriends.Add(this.transform);
        mtPlayerTransform = _friendManager._playerManager._playerHandler.transform;
        meAirWingState = eAirWingState.CatchThePlayer;
    }

    void Update()
    {
        ForceChangeState();
    }

    void FixedUpdate()
    {
        AirWingStateHandler();    
    }

    void ForceChangeState()
    {
        if (mbForceStateChanged)
            return;

        if (PlayerManager.Instance.GetPlayerDeadState() | FriendManager._PlayerIsColseToAnotherFriend)
        {
            mbForceStateChanged = true;
            transform.GetComponent<SphereCollider>().enabled = false;
            DataManager.AddToAirwingAmount(1);
            FriendManager.SetPlayerIsWithFriendState(false);
            FriendManager.SetAirwingActiveState(false);
            FriendManager._PlayerIsColseToAnotherFriend = false;

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
                transform.position = Vector3.MoveTowards(mtAirWingTransform.position, mtPlayerTransform.position, Time.deltaTime * _fBirdMoveTowardPlayerSpeed);
                SmoothLook(mtPlayerTransform.position);
                break;

            case eAirWingState.MoveThroughPath:
                switch (mAirWingsPathScr._eAirWingPathState)
                {
                    case eAirWingPathState.PointA:
                        MovingAirWingAlongGivenPath(mAirWingsPathScr._tPointA.position, _fBirdMoveForwardSpeed, eAirWingPathState.PointB);
                        break;

                    case eAirWingPathState.PointB:
                        MovingAirWingAlongGivenPath(mAirWingsPathScr._tPointB.position, _fBirdMoveForwardSpeed, eAirWingPathState.PointC);
                        break;

                    case eAirWingPathState.PointC:
                        MovingAirWingAlongGivenPath(mAirWingsPathScr._tPointC.position, _fBirdMoveForwardSpeed, eAirWingPathState.PointD, true);
                        break;

                    case eAirWingPathState.PointD:
                        MovingAirWingAlongGivenPath(mAirWingsPathScr._tPointD.position, _fBirdMoveForwardSpeed, eAirWingPathState.PointE1, false, true);
                        break;

                    case eAirWingPathState.PointE1:
                        MovingAirWingAlongGivenPath(mAirWingsPathScr._tPointE1.position, _fBirdMoveForwardSpeed, eAirWingPathState.PointF1);
                        break;

                    case eAirWingPathState.PointE2:
                        MovingAirWingAlongGivenPath(mAirWingsPathScr._tPointE2.position, _fBirdMoveForwardSpeed, eAirWingPathState.PointF2);
                        break;

                    case eAirWingPathState.PointF1:
                        MovingAirWingAlongGivenPath(mAirWingsPathScr._tPointF1.position, _fBirdMoveForwardSpeed, eAirWingPathState.PointF1, false, false, false, true);
                        break;

                    case eAirWingPathState.PointF2:
                        MovingAirWingAlongGivenPath(mAirWingsPathScr._tPointF2.position, _fBirdMoveForwardSpeed, eAirWingPathState.PointF2, false, false, false, true);
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
        SmoothLook(mvAirWingMovePoint);

        mvOffset = mtAirWingTransform.position - targetPos;
        mfSqrLen = mvOffset.sqrMagnitude;
        if (mfSqrLen < 2f)
        {
            if (dropThePlayer)
                PlayerMoveTowardsLandingPad();

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
        transform.position = Vector3.MoveTowards(mtAirWingTransform.position, mvFinalDestination, Time.deltaTime * _fBirdMoveForwardSpeed);
        SmoothLook(mvFinalDestination);

        mvOffset = mvFinalDestination - mtAirWingTransform.position;
        mfSqrLen = mvOffset.sqrMagnitude;
        if (mfSqrLen < 2f * 2f)
        {
            meAirWingState = eAirWingState.None;
            Destroy(this.gameObject);
        }
    }

	void PlayerMoveTowardsLandingPad()
	{
		transform.GetComponent<SphereCollider>().enabled = false;
        //meAirWingState = eAirWingState.LeaveTheScene;
        FriendManager.SetAirwingActiveState(false);

        mtPlayerTransform.position = Vector3.MoveTowards(mtPlayerTransform.position, mvLandingPadPosition, Time.deltaTime * 5f);

        if (mvLandingPadPosition.y - mtPlayerTransform.position.y < 0.1f)
        {
            FriendManager.SetPlayerIsWithFriendState(false);
			mtPlayerTransform.SetParent(_friendManager._playerManager.transform);
			mtPlayerTransform.rotation = new Quaternion(0f, 0f, 0f, 0f);
			mtPlayerTransform.GetComponent<Rigidbody>().useGravity = true;
			mtPlayerTransform.GetComponent<Rigidbody>().isKinematic = false;
            _friendManager._playerManager._playerHandler._eControlState = eControlState.Active;

            _friendManager._playerManager._playerHandler._vPlayerRequiredPosition = mvLandingPadPosition;
            _friendManager._playerManager._playerHandler._vNextPlatformPosition = mvLandingPadPosition;
            _friendManager._playerManager._CameraControllerScr._bFollowPlayerY = false;
            _friendManager._playerManager._playerHandler.DoSingleJump();

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

                other.transform.SetParent(transform);
                FriendManager.SetPlayerIsWithFriendState(true);

                PlayerManager.Instance._CameraControllerScr._bFollowPlayerY = true;
                if (ScoreHandler._OnScoreEventCallback != null)
                    ScoreHandler._OnScoreEventCallback(eScoreType.Dragonfly);

                if (MiniGameManager.Instance._eMiniGameState == eMiniGameState.AcceptFriendHelp || MiniGameManager.Instance._eMiniGameState == eMiniGameState.AvoidFriend)
                    MiniGameManager.Instance._iFriendsHelpAccepted += 1;

                GameObject goAirwingPath = Instantiate(FriendManager.Instance._airWingsMovingPointsPrefab);
                goAirwingPath.transform.SetParent(_friendManager.transform);
                mvTempPos.x = 0f;
                mvTempPos.y = 0f;
                mvTempPos.z = _friendManager._playerManager._playerHandler._tPlayerTransform.position.z + 50f;
                goAirwingPath.transform.position = mvTempPos;
                mAirWingsPathScr = goAirwingPath.GetComponent<AirWingsMovingPointsScr>();
                mAirWingsPathScr._AirWingsScr = this;

                EnvironmentManager.Instance.UpdatePlatformState(mAirWingsPathScr._tPointC.position.z, ePlatformHandlerType.Fixed, true);
                mvTempPos.x = mAirWingsPathScr._tPointC.position.x;
                mvTempPos.y = 0f;
                mvTempPos.z = mAirWingsPathScr._tPointC.position.z;
                mvLandingPadPosition = mvTempPos;

                _friendManager._playerManager._playerHandler._jumpActionScr.StopJump("Armature|idle");
				other.GetComponent<Rigidbody>().useGravity = false;
				other.GetComponent<Rigidbody>().isKinematic = true;

                SetPlayerPositionForAirWings();

                meAirWingState = eAirWingState.MoveThroughPath;
                mAirWingsPathScr._eAirWingPathState = eAirWingPathState.PointA;
			}
		}
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
        _friendManager._playerManager._playerHandler._tPlayerTransform.position = mvTempPos;
    }
}
