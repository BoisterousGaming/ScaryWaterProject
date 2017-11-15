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
    Vector3 mvTempPos;
    int miCount = 0;
    int miDeathLane = 0;
    float mfTimeInGameLoadingBufferState = 0f;
    float miJumpDistance = 10f;
    float mfDrownOffset = 3f;
	bool mbInGameLoadingBufferState = true;
    bool mbLockHighAndLongJumpAction;
    bool mbPerformingSpiderJump = false;

    public int _iLaneNumber;
    public eControlState _eControlState = eControlState.None;
    public PlayerManager _playerManager = null;
    public PlayerJumpAction _jumpActionScr;
    public TouchInputHandler _touchInputHandlerScr;
    public Animator _Animator;
    public Vector3 _vPlayerRequiredPosition = new Vector3(0, 0, 0);
    public Vector3 _vCurPlatformPosition;
    public Vector3 _vNextPlatformPosition;
    public Transform _tPlayerTransform;
    public bool _bLockUpdatingPosition;
    public bool _bSwipedLeftOrRight;
    public Rigidbody _rigidBodyOfPlayer;
    public BarProgressSprite _BarProgressSpriteScr;

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

        JumpFinished();

        mbInGameLoadingBufferState = true;
        _rigidBodyOfPlayer.useGravity = false;
    }

    void Update()
    {
        if(mbInGameLoadingBufferState)
        {
            mfTimeInGameLoadingBufferState += Time.deltaTime;
            if (mfTimeInGameLoadingBufferState > 15.0f)
            {
                _rigidBodyOfPlayer.useGravity = true;
                mbInGameLoadingBufferState = false;
            }
            return;
        }

        if (!_bLockUpdatingPosition)
		    transform.position = _vPlayerRequiredPosition;

		_jumpActionScr.CustomUpdate();
        _touchInputHandlerScr.CustomUpdate();

        if (_playerManager.GetPlayerDeadState())
            DrownThePlayer();
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
        miCount++;
        //Jump finished now what to do check if platform is available below player then jump again
        //Or depending on swipe you can change the jump parameters for next jump
        if (_playerManager._CameraControllerScr != null)
            _playerManager._CameraControllerScr._bFollowPlayerY = false;
        
        mbPerformingSpiderJump = false;

        if (_playerManager._EnvironmentManagerScr != null)
        {
            if (_playerManager._EnvironmentManagerScr.ComparePlatformAndPlayerPositionForLanding(transform.position, 2f, out miDeathLane))
            {
                if (miCount > 1)
                {
                    if (ScoreHandler._OnScoreEventCallback != null)
                        ScoreHandler._OnScoreEventCallback(eScoreType.NormalJump);    
                }

                transform.position = _vPlayerRequiredPosition;
                _vCurPlatformPosition = _vNextPlatformPosition;
                miJumpDistance = 10;
                //_Animator.SetTrigger("Jump");
                _Animator.speed = 2;
                DoJumpToNextPlatform(DataHandler._fPlayerAutoJumpHeight, miJumpDistance);
            }   

            else
            {
                _iLaneNumber = miDeathLane;
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

                if (_playerManager._MiniGameManagerScr != null)
                {
                    if (_playerManager._MiniGameManagerScr._eMiniGameState == eMiniGameState.AvoidDying)
                        _playerManager._MiniGameManagerScr._iPlayerDeathCount += 1;

                    //else
                        //_playerManager._MiniGameManagerScr.DeactivateMiniGame(false);
                }

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
			_iLaneNumber -= 1;
            if (_jumpActionScr._Progress <= 0.6f)
                DoJumpToNextPlatform(DataHandler._fPlayerAutoJumpHeight, miJumpDistance);
		}
	}

	public void DoPlayerShiftRight()
	{
		if (_iLaneNumber < 1)
		{
			_iLaneNumber += 1;
            if (_jumpActionScr._Progress <= 0.6f)
                DoJumpToNextPlatform(DataHandler._fPlayerAutoJumpHeight, miJumpDistance);
		}
	}

    void DoJumpToNextPlatform(float height = DataHandler._fPlayerAutoJumpHeight, float offset = 10.0f, float speed = 17, string animationName = "short_jump_root_motion")
    {
        if (_playerManager._EnvironmentManagerScr == null)
            return;

        _bLockUpdatingPosition = false;
        _playerManager._EnvironmentManagerScr.SetCurrentPlatformPosition(_vCurPlatformPosition.x, _vCurPlatformPosition.y, _vCurPlatformPosition.z);
        _vNextPlatformPosition = _playerManager._EnvironmentManagerScr.GetNextPlatformPosition(1f, offset, 5f * _iLaneNumber);
        _jumpActionScr.JumpToPosition(transform.position, _vNextPlatformPosition, speed, height, JumpFinished, animationName);
    }

	public void DoSingleJump()
	{
		transform.position = _vPlayerRequiredPosition;
		_vCurPlatformPosition = _vNextPlatformPosition;
		miJumpDistance = 10;
        //_Animator.SetTrigger("Jump");
        DoJumpToNextPlatform(DataHandler._fPlayerAutoJumpHeight, miJumpDistance, 17, "short_jump_root_motion");
	}

    public void DoDoubleJump()
    {
        if (mbPerformingSpiderJump)
            return;

        if (_playerManager._CameraControllerScr != null)
            _playerManager._CameraControllerScr._bFollowPlayerY = true;

		if (ScoreHandler._OnScoreEventCallback != null)
			ScoreHandler._OnScoreEventCallback(eScoreType.HighAndLongJump);
        
        transform.position = _vPlayerRequiredPosition;
        _vCurPlatformPosition = _vNextPlatformPosition;
        miJumpDistance = 10;
        //_Animator.SetTrigger("LongJump");
        DoJumpToNextPlatform(DataHandler._fPlayerHighAndLongJumpHeight, miJumpDistance, 17, "long_jump_root_motion");
        _playerManager._BarProgressSpriteScr.AddDamage(0.03f, _playerManager.PlayerDeathHandler);
        //CEffectsPlayer.Instance.Play("LongJump");
    }

	public void DoSpiderJump()
	{
        mbPerformingSpiderJump = true;

        if (_playerManager._CameraControllerScr != null)
            _playerManager._CameraControllerScr._bFollowPlayerY = true;

		transform.position = _vPlayerRequiredPosition;
		_vCurPlatformPosition = _vNextPlatformPosition;
		miJumpDistance = 30;
        //Debug.Log("PlayerReqPosition: " + _vPlayerRequiredPosition + " CurPlatformPosition: " + _vNextPlatformPosition);
        //_Animator.SetTrigger("LongJump");
        DoJumpToNextPlatform(DataHandler._fPlayerSpiderInitiatedJumpHeight, miJumpDistance, 17, "long_jump_root_motion");
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
