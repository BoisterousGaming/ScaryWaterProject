using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eAirWingState
{
    None = 0,
    CatchPlayer,
    MoveForward,
    MoveDown,
    DropPlayer,
    LeaveTheScene,
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
        mtPlayerTransform = PlayerManager.Instance._playerHandler.transform;
        meAirWingState = eAirWingState.CatchPlayer;
    }

    void LateUpdate()
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
        
        if (PlayerManager.Instance.GetPlayerDeadState())
        {
            mbForceStateChanged = true;

            DataManager.AddToAirwingAmount(1);
            FriendManager.SetPlayerIsWithFriendState(false);
            FriendManager.SetAirwingActiveState(false);

            GameObject tCanvas = UICanvasHandler.Instance.GetActiveCanvasByName("HUDCanvas");
            if (tCanvas != null)
            {
                GameplayAreaUIHandler tScr = tCanvas.GetComponent<GameplayAreaUIHandler>();
                tScr.DisplayAirwingCount();
                tScr.SetAirwingBtnState(true);
            }

            transform.GetComponent<SphereCollider>().enabled = false;
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
            case eAirWingState.CatchPlayer:
                transform.position = Vector3.MoveTowards(mtAirWingTransform.position, mtPlayerTransform.position, Time.deltaTime * _fBirdMoveTowardPlayerSpeed);
                SmoothLook(mtPlayerTransform.position);
                break;

            case eAirWingState.MoveForward:
                transform.position = Vector3.MoveTowards(mtAirWingTransform.position, mvAirWingMovePoint, Time.deltaTime * _fBirdMoveForwardSpeed);
                SmoothLook(mvAirWingMovePoint);

                mvOffset = mvAirWingMovePoint - mtAirWingTransform.position;
                mfSqrLen = mvOffset.sqrMagnitude;
                if (mfSqrLen < 2f * 2f)
                {
                    meAirWingState = eAirWingState.MoveDown;
                }
                break;

            case eAirWingState.MoveDown:
				transform.position = Vector3.MoveTowards(mtAirWingTransform.position, mvPlayerDropPoint, Time.deltaTime * _fBirdMoveForwardSpeed);
				SmoothLook(mvPlayerDropPoint);

				mvOffset = mvPlayerDropPoint - mtAirWingTransform.position;
				mfSqrLen = mvOffset.sqrMagnitude;
				if (mfSqrLen < 2f * 2f)
				{
					EnvironmentManager.Instance.UpdatePlatformState(mvPlayerDropPoint.z, ePlatformHandlerType.Fixed, true);
					meAirWingState = eAirWingState.DropPlayer;
				}
                break;

            case eAirWingState.DropPlayer:
                PlayerMoveTowardsLandingPad();
                break;

            case eAirWingState.LeaveTheScene:
                GoToFinalDestination();
                break;

            case eAirWingState.InCaseFailure:
                GoToFinalDestination();
                break;
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
        meAirWingState = eAirWingState.LeaveTheScene;
        FriendManager.SetAirwingActiveState(false);

        mtPlayerTransform.position = Vector3.MoveTowards(mtPlayerTransform.position, mvLandingPadPosition, 10 * Time.deltaTime);

        if (mvLandingPadPosition.y - mtPlayerTransform.position.y < 0.1f)
        {
            FriendManager.SetPlayerIsWithFriendState(false);
			mtPlayerTransform.SetParent(PlayerManager.Instance.transform);
			mtPlayerTransform.rotation = new Quaternion(0f, 0f, 0f, 0f);
			mtPlayerTransform.GetComponent<Rigidbody>().useGravity = true;
			mtPlayerTransform.GetComponent<Rigidbody>().isKinematic = false;
			PlayerManager.Instance._playerHandler._eControlState = eControlState.Active;

			PlayerManager.Instance._playerHandler._vPlayerRequiredPosition = mvLandingPadPosition;
			PlayerManager.Instance._playerHandler._vNextPlatformPosition = mvLandingPadPosition;
            PlayerManager.Instance._CameraControllerScr._bFollowPlayerY = false;
			PlayerManager.Instance._playerHandler.DoSingleJump();

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

                PlayerManager.Instance._CameraControllerScr._bFollowPlayerY = true;
				if (ScoreHandler._OnScoreEventCallback != null)
					ScoreHandler._OnScoreEventCallback(eScoreType.Dragonfly);
                
				other.transform.SetParent(transform);
                FriendManager.SetPlayerIsWithFriendState(true);

				mvTempPos.x = _landingXPos;
				mvTempPos.y = 5f;
				mvTempPos.z = EnvironmentManager.Instance._vCurrentPlatformPosition.z + _fPlayerDropDistance;
				mvPlayerDropPoint = mvTempPos;

				mvTempPos.x = mvPlayerDropPoint.x;
				mvTempPos.y = 10f;
				mvTempPos.z = mvPlayerDropPoint.z - 20f;
				mvAirWingMovePoint = mvTempPos;

				mvTempPos.x = mvPlayerDropPoint.x;
				mvTempPos.y = 0f;
				mvTempPos.z = mvPlayerDropPoint.z;
				mvLandingPadPosition = mvTempPos;

				mvTempPos.x = mvPlayerDropPoint.x;
				mvTempPos.y = 50f;
				mvTempPos.z = mvPlayerDropPoint.z + 100f;
				mvFinalDestination = mvTempPos;

				PlayerManager.Instance._playerHandler._jumpActionScr.StopJump("Armature|idle");
				other.GetComponent<Rigidbody>().useGravity = false;
				other.GetComponent<Rigidbody>().isKinematic = true;

                SetPlayerPositionForAirWings();

				meAirWingState = eAirWingState.MoveForward;

				if (MiniGameManager.Instance._eMiniGameState == eMiniGameState.AcceptFriendHelp || MiniGameManager.Instance._eMiniGameState == eMiniGameState.AvoidFriend)
					MiniGameManager.Instance._iFriendsHelpAccepted += 1;
			}
		}
    }

    void SmoothLook(Vector3 Direction)
	{
		var targetRotation = Quaternion.LookRotation(Direction - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _fRotationSpeed * Time.deltaTime);
	}

    void SetPlayerPositionForAirWings()
    {
        FriendManager._eFriendType = eFriendType.Dragonfly;
        Vector3 airWingPosition = transform.position;
        mvTempPos.x = airWingPosition.x;
        mvTempPos.y = airWingPosition.y - 0.85f;
        mvTempPos.z = airWingPosition.z;
        PlayerManager.Instance._playerHandler._tPlayerTransform.position = mvTempPos;
    }
}
