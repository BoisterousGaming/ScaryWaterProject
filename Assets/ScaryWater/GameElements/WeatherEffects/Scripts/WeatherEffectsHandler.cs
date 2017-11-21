using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherEffectsHandler : MonoBehaviour 
{
    float mfWETimeLimit = 0f;
    float mfTimeInCurWEState = 0f;
    float mfTimeStamp = 0f;
    float mfTimeToStartWE = 0f;
    int miWEToPlay = 0;
    bool mbWEIsEnable = false;
    bool mbWETimeLimitIsSet = false;
    bool mbEnableWE = false;
    bool mbDisableWE = false;

    public PlayerManager _playerManager;

    public GenerateRandomValueScr _GenerateRandomValueScr;

	void Start () 
    {
        _GenerateRandomValueScr = new GenerateRandomValueScr();
        SetWEStartTimeAndType();
	}

    void Update()
    {
        CheckWEPlayingState();
    }

    void SetWEPlayingState()
    {
        if (mbWEIsEnable)
        {
            if (!mbWETimeLimitIsSet)
            {
                mbWETimeLimitIsSet = true;
                mfWETimeLimit = _GenerateRandomValueScr.Random(20, 100);
                mfTimeInCurWEState = 0f;
                mbDisableWE = true;
            }
        }

        else
            SetWEStartTimeAndType();
    }

    void CheckWEPlayingState()
    {
        if (mbEnableWE)
        {
            if (mfTimeToStartWE < mfTimeStamp)
                mfTimeToStartWE += Time.deltaTime;

            else
            {
                mbEnableWE = false;
                PlayWE();
            }
        }

        if (mbDisableWE)
        {
            if (mfTimeInCurWEState < mfWETimeLimit)
                mfTimeInCurWEState += Time.deltaTime;
                
            else
            {
                mbDisableWE = false;
                StopWE();
            }
        }
    }

    void SetWEStartTimeAndType()
    {
        mbWEIsEnable = true;
        miWEToPlay = _GenerateRandomValueScr.Random(1, 3);
        mfTimeStamp = _GenerateRandomValueScr.Random(50, 500);
        mfTimeToStartWE = 0f;
        mbEnableWE = true;
    }

    void PlayWE()
    {
        //Debug.Log("HowManyWEToPlay: " + miWEToPlay);
        int tValue = 0;
        for (int i = 0; i < miWEToPlay; i++)
        {
            tValue = _GenerateRandomValueScr.Random(1, 3);
            if (tValue == 1)
                StartCoroutine(ICloudWEState(true));
            else if (tValue == 2)
                StartCoroutine(IRainWEState(true));
            else if (tValue == 3)
                StartCoroutine(IThunderWEState(true));
        }
        SetWEPlayingState();
    }

    void StopWE()
    {
        StartCoroutine(IThunderWEState(false, _GenerateRandomValueScr.Random(2, 4)));
        StartCoroutine(IRainWEState(false, _GenerateRandomValueScr.Random(3, 7)));
        StartCoroutine(ICloudWEState(false, _GenerateRandomValueScr.Random(5, 10)));
        mbWEIsEnable = false;
        mbWETimeLimitIsSet = false;
        SetWEPlayingState();
    }

    IEnumerator ICloudWEState(bool bState = false, float fTimeDelay = 0f)
    {
        if (bState)
            fTimeDelay = _GenerateRandomValueScr.Random(0, 2);
        yield return new WaitForSeconds(fTimeDelay);
        if (_playerManager._playerHandler._AttachedComponentScr._Cloud != null)
        {
            if (bState)
                _playerManager._playerHandler._AttachedComponentScr._Cloud.Play();
            else
                _playerManager._playerHandler._AttachedComponentScr._Cloud.Stop();
        }
    }

    IEnumerator IRainWEState(bool bState = false, float fTimeDelay = 0f)
    {
        if (bState)
            fTimeDelay = _GenerateRandomValueScr.Random(2, 5);
        yield return new WaitForSeconds(fTimeDelay);
        if (_playerManager._playerHandler._AttachedComponentScr._Rain != null)
        {
            if (bState)
                _playerManager._playerHandler._AttachedComponentScr._Rain.Play();
            else
                _playerManager._playerHandler._AttachedComponentScr._Rain.Stop();
        }
    }

    IEnumerator IThunderWEState(bool bState = false, float fTimeDelay = 0f)
    {
        if (bState)
            fTimeDelay = _GenerateRandomValueScr.Random(3, 6);
        yield return new WaitForSeconds(fTimeDelay);
        if (_playerManager._playerHandler._AttachedComponentScr._Thunder != null)
        {
            if (bState)
                _playerManager._playerHandler._AttachedComponentScr._Thunder.Play();
            else
                _playerManager._playerHandler._AttachedComponentScr._Thunder.Stop();
        }
    }
}
