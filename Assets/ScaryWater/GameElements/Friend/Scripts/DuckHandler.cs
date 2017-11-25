using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckHandler : MonoBehaviour
{
    public enum eDuckState
    {
        None = 0,
        MoveToPointA,
        MoveToPointB,
        MoveToPointC,
        MoveToPointD
    }

	eDuckState meDuckState = eDuckState.None;
    Vector3 mvTempPos;
	bool mbSkipChecking;
    bool mbGiveMiniGamePoints = true;

    public Transform _tPointA;
    public Transform _tPointB;
    public Transform _tPointC;
    public Transform _tPointD;
    public float _fSpeedA = 20f;
    public float _fSpeedB = 20f;
    public float _fSpeedC = 12.5f;
    public float _fRotationSpeed = 2.5f;
    public float _fPickUpPositionOnZAxis;

	void OnEnable()
	{
        FriendManager.Instance._listOfFriends.Add(this.transform);
	}

    void Start ()
	{
        SetVisibilityOfMovingPoints();
        _fPickUpPositionOnZAxis = _tPointA.position.z;
	}

    void FixedUpdate ()
    {
        DuckStateHandler();
    }

    private void DuckStateHandler()
    {
        switch (meDuckState)
        {
            case eDuckState.MoveToPointA:
                MovingDuckAlongGivenPath(_tPointA.localPosition, _fSpeedA, eDuckState.MoveToPointB);
                break;

            case eDuckState.MoveToPointB:
                MovingDuckAlongGivenPath(_tPointB.localPosition, _fSpeedB, eDuckState.MoveToPointC);
                break;

            case eDuckState.MoveToPointC:
                MovingDuckAlongGivenPath(_tPointC.localPosition, _fSpeedA, eDuckState.MoveToPointD, true);
                break;

            case eDuckState.MoveToPointD:
                MovingDuckAlongGivenPath(_tPointD.localPosition, _fSpeedC, eDuckState.None);
                break;
        }
    }

    void MovingDuckAlongGivenPath(Vector3 targetPos, float movingSpeed, eDuckState nextState, bool dropThePlayer = false)
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, Time.deltaTime * movingSpeed);
        SmoothLook(targetPos);
        if (targetPos.z - transform.localPosition.z < 0.1f)
        {
            meDuckState = nextState;

            if (dropThePlayer)
            {
                transform.GetComponent<BoxCollider>().enabled = false;
                FriendManager.SetPlayerIsWithFriendState(false);
                if (mbGiveMiniGamePoints)
                {
                    mbGiveMiniGamePoints = false;
                    //if (MiniGameManager.Instance._eMiniGameState == eMiniGameState.AcceptFriendHelp || MiniGameManager.Instance._eMiniGameState == eMiniGameState.AvoidFriend)
                        //MiniGameManager.Instance._iFriendsHelpAccepted += 1;
                }
                UpdatePlayerStatesOnDrop();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (FriendManager.GetPlayerIsWithFriendState())
            return;

        if (other.CompareTag("Player"))
        {
            if (!mbSkipChecking)
            {
                mbSkipChecking = true;
                if (ScoreHandler._OnScoreEventCallback != null)
                    ScoreHandler._OnScoreEventCallback(eScoreType.Duck);

                FriendManager.SetPlayerIsWithFriendState(true);
                FriendManager.SetFriendType(eFriendType.Duck);
                UpdatePlayerStatesOnTriggerEnter();
                SetPlayerPositionForDuck();

                meDuckState = eDuckState.MoveToPointA;
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
        PlayerManager.Instance._playerHandler._playerPropertiesScr.SetPlayerRequiredAndNextPlatformPosition(_tPointC.position);
        PlayerManager.Instance._CameraControllerScr.CameraFollowPlayerOnYAxis(false);
        PlayerManager.Instance._playerHandler.DoSingleJump();
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

    void SetPlayerPositionForDuck()
    {
        Vector3 duckPosition = transform.position;
        mvTempPos.x = duckPosition.x;
        mvTempPos.y = duckPosition.y + 0.2f;
        mvTempPos.z = duckPosition.z - 0.65f;
        PlayerManager.Instance._playerHandler._playerPropertiesScr.SetPlayerPosition(mvTempPos.x, mvTempPos.y, mvTempPos.z);
    }

    void SetVisibilityOfMovingPoints(bool state = false)
    {
        _tPointA.GetComponent<MeshRenderer>().enabled = state;
        _tPointB.GetComponent<MeshRenderer>().enabled = state;
        _tPointC.GetComponent<MeshRenderer>().enabled = state;
        _tPointD.GetComponent<MeshRenderer>().enabled = state;
    }

    void OnDisable()
    {
        FriendManager.Instance._listOfFriends.Remove(this.transform);
    }
}
