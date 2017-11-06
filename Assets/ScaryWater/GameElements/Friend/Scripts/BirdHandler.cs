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
	bool mbDetectPlayer;
	Vector3 mvLandingPadPosition = Vector3.zero;
    Vector3 mvTempPos;

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
    public float _fSpeedA = 50f;
    public float _fSpeedB = 20f;
    public float _fSpeedC = 20f;
    public float _fPlayerLandingSpeed = 5f;
    public float _fRotationSpeed = 5f;
    public BirdInitiate _BirdInitiateScr;

	void OnEnable()
	{
        FriendManager.Instance._listOfFriends.Add(this.transform);
	}

	void OnDestroy()
	{
        FriendManager.Instance._listOfFriends.Remove(this.transform);

        if (_BirdInitiateScr._InitiateBird != null)
            _BirdInitiateScr._InitiateBird -= StartMoving;
	}

    void Start()
    {
        mvLandingPadPosition = new Vector3(_tPointE.position.x, 0f, _tPointE.position.z);
		_tPointA.GetComponent<MeshRenderer> ().enabled = false;
		_tPointB.GetComponent<MeshRenderer> ().enabled = false;
		_tPointC.GetComponent<MeshRenderer> ().enabled = false;
		_tPointD.GetComponent<MeshRenderer> ().enabled = false;
		_tPointE.GetComponent<MeshRenderer> ().enabled = false;
		_tPointF.GetComponent<MeshRenderer> ().enabled = false;
		_tPointG1.GetComponent<MeshRenderer> ().enabled = false;
		_tPointG2.GetComponent<MeshRenderer> ().enabled = false;
		_tPointH1.GetComponent<MeshRenderer> ().enabled = false;
		_tPointH2.GetComponent<MeshRenderer> ().enabled = false;
        _BirdInitiateScr._InitiateBird += StartMoving;
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
            case eBirdState.Idle:
                break;

            case eBirdState.PointA:
                transform.localPosition = _tPointA.localPosition;

                if(_tPointA.localPosition.z - transform.localPosition.z < 0.1f)
                    meBirdState = eBirdState.PointB;
                break;

            case eBirdState.PointB:
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, _tPointB.localPosition, _fSpeedB * Time.deltaTime);
                SmoothLook(_tPointB.localPosition);

                if (_tPointB.localPosition.z - transform.localPosition.z < 0.1f)
                    mbDetectPlayer = true;
                break;

            case eBirdState.PointC:
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, _tPointC.localPosition, _fSpeedC * Time.deltaTime);
                SmoothLook(_tPointC.localPosition);

                if (_tPointC.localPosition.z - transform.localPosition.z < 0.1f)
                    meBirdState = eBirdState.PointD;
                break;

            case eBirdState.PointD:
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, _tPointD.localPosition, _fSpeedC * Time.deltaTime);
                SmoothLook(_tPointD.localPosition);

                if (_tPointD.localPosition.z - transform.localPosition.z < 0.1f)
                    meBirdState = eBirdState.PointE;
                break;

            case eBirdState.PointE:
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, _tPointE.localPosition, _fSpeedC * Time.deltaTime);
                SmoothLook(_tPointE.localPosition);

                if (_tPointE.localPosition.z - transform.localPosition.z < 0.1f)
                    PlayerMoveTowardsLandingPad();
                break;

            case eBirdState.PointF:
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, _tPointF.localPosition, _fSpeedB * Time.deltaTime);
                SmoothLook(_tPointF.localPosition);

                if (_tPointF.localPosition.z - transform.localPosition.z < 0.1f)
                {
                    int i = Random.Range(0, 1);

                    if (i == 0)
                        meBirdState = eBirdState.PointG1;

                    else
                        meBirdState = eBirdState.PointG2;
                }
                break;

            case eBirdState.PointG1:
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, _tPointG1.localPosition, _fSpeedB * Time.deltaTime);
                SmoothLook(_tPointG1.localPosition);

                if (_tPointG1.localPosition.z - transform.localPosition.z < 0.1f)
                    meBirdState = eBirdState.PointH1;
                break;

            case eBirdState.PointG2:
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, _tPointG2.localPosition, _fSpeedB * Time.deltaTime);
                SmoothLook(_tPointG2.localPosition);

                if (_tPointG2.localPosition.z - transform.localPosition.z < 0.1f)
                    meBirdState = eBirdState.PointH2;
                break;

            case eBirdState.PointH1:
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, _tPointH1.localPosition, _fSpeedB * Time.deltaTime);
                SmoothLook(_tPointH1.localPosition);

                if (_tPointH1.localPosition.z - transform.localPosition.z < 0.1f)
                    Destroy(this.gameObject);
                break;

            case eBirdState.PointH2:
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, _tPointH2.localPosition, _fSpeedB * Time.deltaTime);
                SmoothLook(_tPointH2.localPosition);

                if (_tPointH2.localPosition.z - transform.localPosition.z < 0.1f)
                    Destroy(this.gameObject);
                break;
        }
    }

    void PlayerMoveTowardsLandingPad()
    {
        transform.tag = "Untagged";
        transform.GetComponent<SphereCollider>().enabled = false;
        meBirdState = eBirdState.PointF;

        PlayerManager.Instance._playerHandler._tPlayerTransform.position = Vector3.MoveTowards(PlayerManager.Instance._playerHandler._tPlayerTransform.position, mvLandingPadPosition, _fPlayerLandingSpeed * Time.deltaTime);

        if (mvLandingPadPosition.y - PlayerManager.Instance._playerHandler._tPlayerTransform.position.y < 0.1f)
        {
            FriendManager._bPlayerIsWithAFriend = false;
            PlayerManager.Instance._playerHandler._tPlayerTransform.SetParent(PlayerManager.Instance.transform);
			PlayerManager.Instance._playerHandler._tPlayerTransform.rotation = new Quaternion(0f, 0f, 0f, 0f);
			PlayerManager.Instance._playerHandler.GetComponent<Rigidbody>().useGravity = true;
			PlayerManager.Instance._playerHandler.GetComponent<Rigidbody>().isKinematic = false;
			PlayerManager.Instance._playerHandler._eControlState = eControlState.Active;

			PlayerManager.Instance._playerHandler._vPlayerRequiredPosition = mvLandingPadPosition;
			PlayerManager.Instance._playerHandler._vNextPlatformPosition = mvLandingPadPosition;
            PlayerManager.Instance._CameraControllerScr._bFollowPlayerY = false;
			PlayerManager.Instance._playerHandler.DoSingleJump();
        }
    }

    void OnTriggerEnter(Collider other)
    {
		if (FriendManager._bPlayerIsWithAFriend)
			return;

		if (other.CompareTag("Player"))
		{
			if (mbDetectPlayer)
			{
				mbDetectPlayer = false;

                PlayerManager.Instance._CameraControllerScr._bFollowPlayerY = true;
				if (ScoreHandler._OnScoreEventCallback != null)
                {
                    if (_eBirdType.Equals(eBirdType.Kingfisher))
                        ScoreHandler._OnScoreEventCallback(eScoreType.Kingfisher);

                    else if (_eBirdType.Equals(eBirdType.Dragonfly))
                        ScoreHandler._OnScoreEventCallback(eScoreType.Dragonfly);
                }

				other.transform.SetParent(transform);

				FriendManager._bPlayerIsWithAFriend = true;
				PlayerManager.Instance._playerHandler._jumpActionScr.StopJump("Armature|idle");
				other.GetComponent<Rigidbody>().useGravity = false;
				other.GetComponent<Rigidbody>().isKinematic = true;
                DetectBirdType();

				meBirdState = eBirdState.PointC;

				if (MiniGameManager.Instance.AutoImplementedProperties_eMiniGameState == eMiniGameState.AcceptFriendHelp || MiniGameManager.Instance.AutoImplementedProperties_eMiniGameState == eMiniGameState.AvoidFriend)
					MiniGameManager.Instance._iFriendsHelpAccepted += 1;
			}
		}
    }

    void SmoothLook(Vector3 Direction)
	{
		Vector3 diff = Direction - transform.localPosition;
		if (Vector3.SqrMagnitude (diff) > 0) 
        {
			var targetRotation = Quaternion.LookRotation (diff);
			transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, _fRotationSpeed * Time.deltaTime);
		}
	}

    void DetectBirdType()
    {
        if (this._eBirdType == eBirdType.Kingfisher)
            SetPlayerPositionForKingfisher();

        else if (this._eBirdType == eBirdType.Dragonfly)
            SetPlayerPositionForDragonfly();
    }

    void SetPlayerPositionForKingfisher()
    {
        Vector3 birdPosition = transform.position;
        mvTempPos.x = birdPosition.x;
        mvTempPos.y = birdPosition.y - 1.25f;
        mvTempPos.z = birdPosition.z - 0.35f;
        PlayerManager.Instance._playerHandler._tPlayerTransform.position = mvTempPos;
    }

    void SetPlayerPositionForDragonfly()
    {
        Vector3 birdPosition = transform.position;
        mvTempPos.x = birdPosition.x;
        mvTempPos.y = birdPosition.y - 0.85f;
        mvTempPos.z = birdPosition.z;
        PlayerManager.Instance._playerHandler._tPlayerTransform.position = mvTempPos;
    }
}
