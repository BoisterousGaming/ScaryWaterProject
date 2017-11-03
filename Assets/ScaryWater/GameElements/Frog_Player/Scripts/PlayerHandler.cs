﻿using System;
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
    float mfTimeInGameLoadingBufferState = 0.0f;
	float miJumpDistance = 10;
	bool mbInGameLoadingBufferState = true;
    bool mbLockHighAndLongJumpAction;

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
	}

    void InputHandlerCallback(eInputType Input)
    {
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
		if (EnvironmentManager.Instance.ComparePlatformAndPlayerPositionForLanding(transform.position, 2f))
        {
			if (ScoreHandler._OnScoreEventCallback != null)
				ScoreHandler._OnScoreEventCallback(eScoreType.NormalJump);
            
			transform.position = _vPlayerRequiredPosition;
			_vCurPlatformPosition = _vNextPlatformPosition;
			miJumpDistance = 10;
			//_Animator.SetTrigger("Jump");
			_Animator.speed = 2;
			DoJumpToNextPlatform(DataHandler._fPlayerAutoJumpHeight, miJumpDistance);
        }

        else
        {
			GameObject tCanvas = UICanvasHandler.Instance.GetActiveCanvasByName("HUDCanvas");
			if (tCanvas != null)
			{
				_jumpActionScr.StopJump("death");
				GameplayAreaUIHandler tHUDCanvasScr = tCanvas.GetComponent<GameplayAreaUIHandler>();
                tHUDCanvasScr._HealthBar.AddDamage(tHUDCanvasScr._HealthBar._CurrentFillAmount + 0.001f, _playerManager.PlayerDeathHandler);
                //Debug.Log("Executing");

				if (MiniGameManager.Instance.AutoImplementedProperties_eMiniGameState == eMiniGameState.AvoidDying)
					MiniGameManager.Instance._iPlayerDeathCount += 1;

				else
					MiniGameManager.Instance.DeactivateMiniGame(false);

				if (tHUDCanvasScr._HealthBar.GetNumberOfFills() >= 1)
					StartCoroutine(IRespawnThePlayer(EnvironmentManager.Instance.ComparePlatformAndPlayerPositionForReSpawning(transform.position, 2f, 30f)));
			}
		}

        _bSwipedLeftOrRight = false;
        mbLockHighAndLongJumpAction = false;
    }

	IEnumerator IRespawnThePlayer(Vector3 SpawnPosition)
	{
		yield return new WaitForSeconds(2f);

        mvTempPos.x = SpawnPosition.x;
        mvTempPos.y = SpawnPosition.y + 0.5f;
        mvTempPos.z = SpawnPosition.z;
        _tPlayerTransform.position = mvTempPos;
        _vNextPlatformPosition = mvTempPos;
        _vPlayerRequiredPosition = mvTempPos;
		_eControlState = eControlState.Active;
        DoSingleJump();
	}

	public void DoPlayerShiftLeft()
	{
		if (_iLaneNumber > -1)
		{
			_iLaneNumber -= 1;
            if (_jumpActionScr._Progress <= 0.6f)
                DoJumpToNextPlatform(1.5f, miJumpDistance);
		}
	}

	public void DoPlayerShiftRight()
	{
		if (_iLaneNumber < 1)
		{
			_iLaneNumber += 1;
            if (_jumpActionScr._Progress <= 0.6f)
                DoJumpToNextPlatform(1.5f, miJumpDistance);
		}
	}

    void DoJumpToNextPlatform(float height = 1.5f, float offset = 10.0f, float speed = 17, string animationName = "short_jump_root_motion")
    {
        _bLockUpdatingPosition = false;
        EnvironmentManager.Instance.SetCurrentPlatformPosition(_vCurPlatformPosition.x, _vCurPlatformPosition.y, _vCurPlatformPosition.z);
        _vNextPlatformPosition = EnvironmentManager.Instance.GetNextPlatformPosition(1f, offset, 5f * _iLaneNumber);
        _jumpActionScr.JumpToPosition(transform.position, _vNextPlatformPosition, speed, height, JumpFinished, animationName);
    }

	public void DoSingleJump()
	{
		transform.position = _vPlayerRequiredPosition;
		_vCurPlatformPosition = _vNextPlatformPosition;
		miJumpDistance = 10;
        //_Animator.SetTrigger("Jump");
        DoJumpToNextPlatform(DataHandler._fPlayerHighAndLongJumpHeight, miJumpDistance, 17, "short_jump_root_motion");
	}

    public void DoDoubleJump()
    {
		if (ScoreHandler._OnScoreEventCallback != null)
			ScoreHandler._OnScoreEventCallback(eScoreType.HighAndLongJump);
        
        transform.position = _vPlayerRequiredPosition;
        _vCurPlatformPosition = _vNextPlatformPosition;
        miJumpDistance = 10;
        //_Animator.SetTrigger("LongJump");
        DoJumpToNextPlatform(DataHandler._fPlayerHighAndLongJumpHeight, miJumpDistance, 17, "long_jump_root_motion");
    }

	public void DoSpiderJump()
	{
		transform.position = _vPlayerRequiredPosition;
		_vCurPlatformPosition = _vNextPlatformPosition;
		miJumpDistance = 30;
        Debug.Log("PlayerReqPosition: " + _vPlayerRequiredPosition + " CurPlatformPosition: " + _vNextPlatformPosition);
        //_Animator.SetTrigger("LongJump");
        DoJumpToNextPlatform(DataHandler._fPlayerSpiderInitiatedJumpHeight, miJumpDistance, 17, "long_jump_root_motion");
	}

    public void DoPoisonThrow()
    {
		GameObject mGoPoison = Instantiate(PlayerManager.Instance._poisonPrefab);
		PoisonHandler poisonHandler = mGoPoison.GetComponent<PoisonHandler>();
		poisonHandler._playerHandler = this;
        poisonHandler.Initialize();
    }

    void OnDestroy()
    {
        if (_touchInputHandlerScr._InputTypeChangedCallback != null)
            _touchInputHandlerScr._InputTypeChangedCallback -= InputHandlerCallback;
    }
}
