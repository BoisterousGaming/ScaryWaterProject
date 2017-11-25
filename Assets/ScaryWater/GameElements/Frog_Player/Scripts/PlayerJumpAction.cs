using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerJumpAction 
{
    Action mAnimationCallback;
    Vector3 mvDiffVector;
    Vector3 mvTempVector;
    float mfPlayerJumpTime;
    float mfCount;
    float mfJumpHeight;
    string msAnimationName;

    public bool _bIsAnimating = false;
    public Vector3 _vStartPosition;
    public Vector3 _vEndPosition;
    public float _Progress;
    public PlayerHandler _PlayerHandler;

    //Jump to a particular position within time specified and do a callback once done
    public void JumpToPosition(Vector3 StartPosition, Vector3 EndPosition, float speed, float height, Action callback, string animationName)
	{
        msAnimationName = animationName;
        _vEndPosition = EndPosition;
        _vStartPosition = StartPosition;

        mAnimationCallback = callback;
        mvDiffVector = _vEndPosition - _vStartPosition;

        mfPlayerJumpTime = Mathf.Abs(mvDiffVector.z)/(float)speed;
        //Debug.Log("Diff: " + Vector3.Magnitude(mvDiffVector) + " StartPos: "+StartPosition+" EndPos: "+EndPosition+" Time required: "+mfPlayerJumpTime);
        mfJumpHeight = height;
        mfCount = 0.0f;
        _Progress = 0.0f;
        _bIsAnimating = true;
	}

    public void CustomUpdate()
    {
        PerformPalyerJump();
	}

    void PerformPalyerJump()
    {		
		//Debug.Log("target position: " + _vEndPosition);
		//As long as animation is running you need to update player position according to formula
		//Once it is over do a callback
		if (_bIsAnimating)
		{
            if (mfPlayerJumpTime > 0)
            {
                mfCount += Time.deltaTime;
                _Progress = mfCount / mfPlayerJumpTime;
                //Debug.Log(_Progress);
                if (_Progress >= 1)
                {
                    _Progress = 0.0f;
                    _bIsAnimating = false;
                    _PlayerHandler._vPlayerRequiredPosition.y = _vEndPosition.y;
                    _PlayerHandler._vPlayerRequiredPosition.z = _vEndPosition.z;
                    _PlayerHandler._vPlayerRequiredPosition.x = _vEndPosition.x;

					if (mAnimationCallback != null)
    					mAnimationCallback();
                }

                else
                {
                    mvTempVector = mvDiffVector * _Progress;
                    mvTempVector.y += Mathf.Abs(Mathf.Sin(_Progress * Mathf.PI) * mfJumpHeight);
                    _PlayerHandler._vPlayerRequiredPosition.y = _vStartPosition.y + mvTempVector.y;
                    _PlayerHandler._vPlayerRequiredPosition.z = _vStartPosition.z + mvTempVector.z;
                    _PlayerHandler._vPlayerRequiredPosition.x = _vStartPosition.x + mvTempVector.x;
                    _PlayerHandler._Animator.Play(msAnimationName, -1, _Progress);
                }
            }
            else
            {
                _Progress = 0.0f;
                _PlayerHandler._vPlayerRequiredPosition.y = _vEndPosition.y;
				_PlayerHandler._vPlayerRequiredPosition.z = _vEndPosition.z;
                _PlayerHandler._vPlayerRequiredPosition.x = _vEndPosition.x;
                _bIsAnimating = false;

				if (mAnimationCallback != null)
    				mAnimationCallback();
            }
		}		
    }

    public void StopJump(string animationName)
    {
        msAnimationName = animationName;
		_bIsAnimating = false; 
        mfCount = 0.0f;
		_Progress = 0.0f;
        _PlayerHandler._bLockUpdatingPosition = true;
        _PlayerHandler._Animator.Play(msAnimationName, -1, _Progress);
        _PlayerHandler._eControlState = eControlState.Deactive;
    }
}

