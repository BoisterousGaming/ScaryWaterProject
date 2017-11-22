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

    public int _fWETimeLimitMinValue = 20;
    public int _fWETimeLimitMaxValue = 100;
    public int _fCloudWEStopMinTimeValue = 2;
    public int _fCloudWEStopMaxTimeValue = 4;
    public int _fRainWEStopMinTimeValue = 3;
    public int _fRainWEStopMaxTimeValue = 7;
    public int _fThunderWEStopMinTimeValue = 5;
    public int _fThunderWEStopMaxTimeValue = 10;
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
                mfWETimeLimit = _GenerateRandomValueScr.Random(_fWETimeLimitMinValue, _fWETimeLimitMaxValue);
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
        miWEToPlay = _GenerateRandomValueScr.Random(1, 2);
        mfTimeStamp = _GenerateRandomValueScr.Random(50, 500);
        mfTimeToStartWE = 0f;
        mbEnableWE = true;
    }

    void PlayWE()
    {
        //Debug.Log("HowManyWEToPlay: " + miWEToPlay);
        StartCoroutine(ICloudWEState(true));
        int tValue = 0;
        for (int i = 0; i < miWEToPlay; i++)
        {
            tValue = _GenerateRandomValueScr.Random(1, 2);
            if (tValue == 1)
                StartCoroutine(IRainWEState(true));
            else if (tValue == 2)
                StartCoroutine(IThunderWEState(true));
        }
        SetWEPlayingState();
    }

    void StopWE()
    {
        StartCoroutine(IThunderWEState(false, _GenerateRandomValueScr.Random(_fCloudWEStopMinTimeValue, _fCloudWEStopMaxTimeValue)));
        StartCoroutine(IRainWEState(false, _GenerateRandomValueScr.Random(_fRainWEStopMinTimeValue, _fRainWEStopMaxTimeValue)));
        StartCoroutine(ICloudWEState(false, _GenerateRandomValueScr.Random(_fThunderWEStopMinTimeValue, _fThunderWEStopMaxTimeValue)));
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
