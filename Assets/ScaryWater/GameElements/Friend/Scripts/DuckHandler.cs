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
	bool mbSkipChecking;
    Vector3 mvTempPos;

    public Transform _tPointA;
    public Transform _tPointB;
    public Transform _tPointC;
    public Transform _tPointD;
    public float _fSpeedA = 40f;
    public float _fSpeedB = 40f;
    public float _fSpeedC = 25f;
    public float _fRotationSpeed = 5f;
    public FrirendSurroundingScr _SurroundingColliderScr;

	void OnEnable()
	{
        FriendManager.Instance._listOfFriends.Add(this.transform);
	}

	void OnDestroy()
	{
        FriendManager.Instance._listOfFriends.Remove(this.transform);
	}

	void Start ()
	{
        _tPointA.GetComponent<MeshRenderer>().enabled = false;
        _tPointB.GetComponent<MeshRenderer>().enabled = false;
        _tPointC.GetComponent<MeshRenderer>().enabled = false;
        _tPointD.GetComponent<MeshRenderer>().enabled = false;
	}

    void FixedUpdate ()
    {
        DuckStateHandler();
    }

    private void DuckStateHandler()
    {
        switch (meDuckState)
        {
            case eDuckState.None:
                break;

            case eDuckState.MoveToPointA:
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, _tPointA.localPosition, _fSpeedA * Time.deltaTime);
                SmoothLook(_tPointA.localPosition);

                if (_tPointA.localPosition.z - transform.localPosition.z < 0.1f)
                    meDuckState = eDuckState.MoveToPointB;
                break;

            case eDuckState.MoveToPointB:
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, _tPointB.localPosition, _fSpeedB * Time.deltaTime);
                SmoothLook(_tPointB.localPosition);

                if (_tPointB.localPosition.z - transform.localPosition.z < 0.1f)
                    meDuckState = eDuckState.MoveToPointC;
                break;

            case eDuckState.MoveToPointC:
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, _tPointC.localPosition, _fSpeedA * Time.deltaTime);
                SmoothLook(_tPointC.localPosition);

                if (_tPointC.localPosition.z - transform.localPosition.z < 0.1f)
                {
                    GetComponent<Collider>().enabled = false;
                    meDuckState = eDuckState.MoveToPointD;

                    FriendManager._bPlayerIsWithAFriend = false;
                    FriendManager._bPlayerIsWithinFriendSurrounding = false;

                    PlayerManager.Instance._playerHandler._tPlayerTransform.SetParent(PlayerManager.Instance.transform);
                    PlayerManager.Instance._playerHandler._tPlayerTransform.rotation = new Quaternion(0f, 0f, 0f, 0f);
                    PlayerManager.Instance._playerHandler.GetComponent<Rigidbody>().useGravity = true;
                    PlayerManager.Instance._playerHandler.GetComponent<Rigidbody>().isKinematic = false;
                    PlayerManager.Instance._playerHandler._eControlState = eControlState.Active;

                    PlayerManager.Instance._playerHandler._vPlayerRequiredPosition = _tPointC.position;
                    PlayerManager.Instance._playerHandler._vNextPlatformPosition = _tPointC.position;
                    PlayerManager.Instance._CameraControllerScr._bFollowPlayerY = false;
                    PlayerManager.Instance._playerHandler.DoSingleJump();
                }
                break;

            case eDuckState.MoveToPointD:
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, _tPointD.localPosition, _fSpeedC * Time.deltaTime);
                SmoothLook(_tPointD.localPosition);

                if (_tPointD.localPosition.z - transform.localPosition.z < 0.1f)
                    meDuckState = eDuckState.None;
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (FriendManager._bPlayerIsWithAFriend)
            return;

        if (other.CompareTag("Player"))
        {
            if (!mbSkipChecking)
            {
                mbSkipChecking = false;
                _SurroundingColliderScr.GetComponent<Collider>().enabled = false;

                PlayerManager.Instance._CameraControllerScr._bFollowPlayerY = true;
                if (ScoreHandler._OnScoreEventCallback != null)
                    ScoreHandler._OnScoreEventCallback(eScoreType.Duck);

                other.transform.SetParent(transform);

                FriendManager._bPlayerIsWithAFriend = true;
                PlayerManager.Instance._playerHandler._jumpActionScr.StopJump("Armature|idle");
                other.GetComponent<Rigidbody>().useGravity = false;
                other.GetComponent<Rigidbody>().isKinematic = true;
                FriendManager._eFriendType = eFriendType.Duck;
                SetPlayerPositionForDuck();

                meDuckState = eDuckState.MoveToPointA;

                if (MiniGameManager.Instance._eMiniGameState == eMiniGameState.AcceptFriendHelp || MiniGameManager.Instance._eMiniGameState == eMiniGameState.AvoidFriend)
                    MiniGameManager.Instance._iFriendsHelpAccepted += 1;
            }
        }
    }

    void SmoothLook(Vector3 Direction)
    {
        var targetRotation = Quaternion.LookRotation(Direction - transform.localPosition);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _fRotationSpeed * Time.deltaTime);
    }

    void SetPlayerPositionForDuck()
    {
        Vector3 duckPosition = transform.position;
        mvTempPos.x = duckPosition.x;
        mvTempPos.y = duckPosition.y + 0.2f;
        mvTempPos.z = duckPosition.z - 0.65f;
        PlayerManager.Instance._playerHandler._tPlayerTransform.position = mvTempPos;
    }
}
