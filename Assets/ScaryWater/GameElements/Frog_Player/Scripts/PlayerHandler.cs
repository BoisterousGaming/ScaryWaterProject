using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eControlState
{
    None = 0,
    Active,
    Deactive
}

public class PlayerHandler : MonoBehaviour
{
    int miDeathLane = 0;
    float mfTimeInGameLoadingBufferState = 0f;
    float miJumpDistance = 10f;
    float mfDrownOffset = 3f;
    float mfPlayerOldZPosition = 0f;
	bool mbInGameLoadingBufferState = true;
    bool mbLockHighAndLongJumpAction;
    bool mbPerformingSpiderJump = false;
    Vector3 mvTempPos;

    public int _iLaneNumber;
    public eControlState _eControlState = eControlState.None;
    public PlayerManager _playerManager = null;
    public PlayerJumpAction _jumpActionScr;
    public TouchInputHandler _touchInputHandlerScr;
    public PlayerProperties _playerPropertiesScr;
    public Animator _Animator;
    public Vector3 _vPlayerRequiredPosition = Vector3.zero;
    public Vector3 _vCurPlatformPosition;
    public Vector3 _vNextPlatformPosition;
    public Transform _tPlayerTransform;
    public bool _bLockUpdatingPosition;
    public bool _bSwipedLeftOrRight;
    public Rigidbody _rigidBodyOfPlayer;
    public BarProgressSprite _BarProgressSpriteScr;
    public AttachedComponentScr _AttachedComponentScr;
    public float _fSpeed = 17f; // Testing
    public float _fLaneChangeLimit = 0.6f; // Testing

    void Awake()
    {
        _iLaneNumber = 0;
        _vCurPlatformPosition = Vector3.zero;
        _vNextPlatformPosition = Vector3.zero;
    }

    void Start()
    {
        _tPlayerTransform = transform;
		_eControlState = eControlState.Active;
        _jumpActionScr = new PlayerJumpAction();
        _jumpActionScr._PlayerHandler = this;
        _touchInputHandlerScr = new TouchInputHandler();
        _touchInputHandlerScr._playerHandler = this;
        _touchInputHandlerScr._InputTypeChangedCallback += InputHandlerCallback;
        _playerPropertiesScr = new PlayerProperties();
        _playerPropertiesScr._playerHandler = this;
        JumpFinished();

        mbInGameLoadingBufferState = true;
        _rigidBodyOfPlayer.useGravity = false;
    }

    void Update()
    {
        if(mbInGameLoadingBufferState)
        {
            _rigidBodyOfPlayer.useGravity = true;
            return;
        }

        if (!_bLockUpdatingPosition)
		    transform.position = _vPlayerRequiredPosition;

		_jumpActionScr.CustomUpdate();
        _touchInputHandlerScr.CustomUpdate();

        if (_playerManager.GetPlayerDeadState())
            DrownThePlayer();

        AddCommonScore();
	}

    public void ReadySteadyGo()
    {
        mbInGameLoadingBufferState = false;
        transform.rotation = Quaternion.identity;
        _BarProgressSpriteScr.GetComponent<SpriteRenderer>().enabled = true;
        _BarProgressSpriteScr.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        //_BarProgressSpriteScr.GetComponentInChildren<SpriteRenderer>().enabled = true;
        //transform.position = _vPlayerRequiredPosition;
        _vCurPlatformPosition = new Vector3(0f, 0f, -10f);
        //Debug.Log("Player Pos: " + transform.position);
        miJumpDistance = Mathf.Abs(0 - transform.position.z);
        DoJumpToNextPlatform(DataHandler._fPlayerHighAndLongJumpHeight, 10f, miJumpDistance, "long_jump_root_motion");
    }

    void AddCommonScore()
    {
        if (_tPlayerTransform.position.z - mfPlayerOldZPosition > 10f)
        {
            mfPlayerOldZPosition = _tPlayerTransform.position.z;
            if (ScoreHandler._OnScoreEventCallback != null)
                        ScoreHandler._OnScoreEventCallback(eScoreType.NormalJump);   
        }
    }

    void InputHandlerCallback(eInputType Input)
    {
        if (_playerManager.GetPlayerDeadState())
            return;
        
        switch(Input)
        {
            case eInputType.SingleTap:
                if (DataManager.GetCSessionPoisonAmount() > 0)
                {
                    DataManager.SubstarctFromCSessionPoisonAmount(1);
                    DoPoisonThrow();
                }

                else if (DataManager.GetPoisonAmount() > 0)
                {
                    DataManager.SubstarctFromPoisonAmount(1);
                    DoPoisonThrow();
                }
                break;

            case eInputType.SwipeUp:
                if (!mbLockHighAndLongJumpAction)
                {
                    mbLockHighAndLongJumpAction = true;
                    DoDoubleJump();
                }
                break;

            case eInputType.SwipeDown:
                break;

            case eInputType.SwipeLeft:
                _bSwipedLeftOrRight = true;
                DoPlayerShiftLeft();
                break;

            case eInputType.SwipeRight:
                _bSwipedLeftOrRight = true;
                DoPlayerShiftRight();
                break;
        }
    }

    void JumpFinished()
    {
        //Jump finished now what to do check if platform is available below player then jump again
        //Or depending on swipe you can change the jump parameters for next jump
        if (_playerManager._CameraControllerScr != null)
            _playerManager._CameraControllerScr.CameraFollowPlayerOnYAxis(false);
        
        mbPerformingSpiderJump = false;

        if (_playerManager._EnvironmentManagerScr != null)
        {
            if (_playerManager._EnvironmentManagerScr.ComparePlatformAndPlayerPositionForLanding(transform.position, 2f, out miDeathLane))
            {
                transform.position = _vPlayerRequiredPosition;
                _vCurPlatformPosition = _vNextPlatformPosition;
                miJumpDistance = 10;
                //_Animator.SetTrigger("Jump");
                _Animator.speed = 2;
                DoJumpToNextPlatform(DataHandler._fPlayerAutoJumpHeight, _fSpeed, miJumpDistance);
            }   

            else
            {
                _playerManager.SetPlayerDeadState(true);
                SetPlayerColliderState(false);
                //CEffectsPlayer.Instance.Play("PlayerWaterDeath");
                _touchInputHandlerScr._eInputType = eInputType.None;
                _bSwipedLeftOrRight = false;
                _jumpActionScr.StopJump("death");

                BarProgressSprite tHealthBarScr = _playerManager._BarProgressSpriteScr;

                float targetHealth = tHealthBarScr._TargetValue;
                float diff = targetHealth - (int)targetHealth;
                if (diff <= 0.001f)
                    diff = 1.0f;
                targetHealth -= diff;
                targetHealth = Mathf.RoundToInt(targetHealth);
                tHealthBarScr.AddDamage(diff + 0.001f, _playerManager.PlayerDeathHandler);

                //if (_playerManager._MiniGameManagerScr != null)
                //{
                //    if (_playerManager._MiniGameManagerScr._eMiniGameState == eMiniGameState.AvoidDying)
                //        _playerManager._MiniGameManagerScr._iPlayerDeathCount += 1;

                //    //else
                //        //_playerManager._MiniGameManagerScr.DeactivateMiniGame(false);
                //}
                if (_playerManager._EnvironmentManagerScr != null)
                {
                    if (tHealthBarScr.GetNumberOfFills() >= 1)
                        StartCoroutine(IRespawnThePlayer(_playerManager._EnvironmentManagerScr.ComparePlatformAndPlayerPositionForReSpawning(transform.position, 2f, 30f)));
                }
            }

            _bSwipedLeftOrRight = false;
            mbLockHighAndLongJumpAction = false;
        }
    }

    void DrownThePlayer()
    {
        Vector3 tDestinationPos = Vector3.zero;
        tDestinationPos.x = transform.position.x;
        tDestinationPos.y = transform.position.y - mfDrownOffset;
        tDestinationPos.z = transform.position.z;
        transform.position = Vector3.MoveTowards(transform.position, tDestinationPos, Time.deltaTime * 2f);
    }

	IEnumerator IRespawnThePlayer(Vector3 SpawnPosition)
	{
		yield return new WaitForSeconds(2f);
        SetPlayerColliderState();
        _playerManager.SetPlayerDeadState(false);
        mvTempPos.x = SpawnPosition.x;
        mvTempPos.y = SpawnPosition.y + 0.5f;
        mvTempPos.z = SpawnPosition.z;
        _tPlayerTransform.position = mvTempPos;
        _vNextPlatformPosition = mvTempPos;
        _vPlayerRequiredPosition = mvTempPos;
		_eControlState = eControlState.Active;
        _iLaneNumber = miDeathLane;
        DoSingleJump();
	}

    void SetPlayerColliderState(bool state = true)
    {
        transform.GetComponent<SphereCollider>().enabled = state;
    }

	public void DoPlayerShiftLeft()
	{
		if (_iLaneNumber > -1)
		{
            if (_jumpActionScr._Progress <= _fLaneChangeLimit)
            {
                _iLaneNumber -= 1;
                DoJumpToNextPlatform(DataHandler._fPlayerAutoJumpHeight, _fSpeed, miJumpDistance);
            }
		}
	}

	public void DoPlayerShiftRight()
	{
		if (_iLaneNumber < 1)
		{
            if (_jumpActionScr._Progress <= _fLaneChangeLimit)
            {
                _iLaneNumber += 1;
                DoJumpToNextPlatform(DataHandler._fPlayerAutoJumpHeight, _fSpeed, miJumpDistance);
            }
		}
	}

    void DoJumpToNextPlatform(float height = DataHandler._fPlayerAutoJumpHeight, float speed = 17.0f, float offset = 10.0f, string animationName = "short_jump_root_motion")
    {
        if (_playerManager._EnvironmentManagerScr == null)
            return;

        _bLockUpdatingPosition = false;
        _playerManager._EnvironmentManagerScr.SetCurrentPlatformPosition(_vCurPlatformPosition);
        _vNextPlatformPosition = _playerManager._EnvironmentManagerScr.GetNextPlatformPosition(offset, DataHandler._fSpaceBetweenLanes * _iLaneNumber);
        _jumpActionScr.JumpToPosition(transform.position, _vNextPlatformPosition, speed, height, JumpFinished, animationName);
    }

	public void DoSingleJump()
	{
		transform.position = _vPlayerRequiredPosition;
		_vCurPlatformPosition = _vNextPlatformPosition;
		miJumpDistance = 10;
        //_Animator.SetTrigger("Jump");
        DoJumpToNextPlatform(DataHandler._fPlayerAutoJumpHeight, _fSpeed, miJumpDistance, "short_jump_root_motion");
	}

    public void DoDoubleJump()
    {
        if (mbPerformingSpiderJump)
            return;

        if (_playerManager._CameraControllerScr != null)
            _playerManager._CameraControllerScr.CameraFollowPlayerOnYAxis();

		if (ScoreHandler._OnScoreEventCallback != null)
			ScoreHandler._OnScoreEventCallback(eScoreType.HighAndLongJump);
        
        transform.position = _vPlayerRequiredPosition;
        _vCurPlatformPosition = _vNextPlatformPosition;
        miJumpDistance = 10;
        //_Animator.SetTrigger("LongJump");
        DoJumpToNextPlatform(DataHandler._fPlayerHighAndLongJumpHeight, _fSpeed, miJumpDistance, "long_jump_root_motion");
        _playerManager._BarProgressSpriteScr.AddDamage(0.03f, _playerManager.PlayerDeathHandler);
        //CEffectsPlayer.Instance.Play("LongJump");
    }

	public void DoSpiderJump()
	{
        mbPerformingSpiderJump = true;

        if (_playerManager._CameraControllerScr != null)
            _playerManager._CameraControllerScr.CameraFollowPlayerOnYAxis();

		transform.position = _vPlayerRequiredPosition;
		_vCurPlatformPosition = _vNextPlatformPosition;
		miJumpDistance = 30;
        //Debug.Log("PlayerReqPosition: " + _vPlayerRequiredPosition + " CurPlatformPosition: " + _vNextPlatformPosition);
        //_Animator.SetTrigger("LongJump");
        DoJumpToNextPlatform(DataHandler._fPlayerSpiderInitiatedJumpHeight, _fSpeed, miJumpDistance, "long_jump_root_motion");
	}

    public void DoPoisonThrow()
    {
        if (FriendManager.GetPlayerIsWithFriendState())
            return;
        
		GameObject mGoPoison = Instantiate(_playerManager._poisonPrefab);
		PoisonHandler poisonHandler = mGoPoison.GetComponent<PoisonHandler>();
		poisonHandler._playerHandler = this;
        poisonHandler.Initialize();

        GameObject tCanvas = UICanvasHandler.Instance.GetActiveCanvasByName("HUDCanvas");
        if (tCanvas != null)
            tCanvas.GetComponent<GameplayAreaUIHandler>().DisplayPoisonCount();
    }

    void OnDisable()
    {
        if (_touchInputHandlerScr._InputTypeChangedCallback != null)
            _touchInputHandlerScr._InputTypeChangedCallback -= InputHandlerCallback;
    }
}
