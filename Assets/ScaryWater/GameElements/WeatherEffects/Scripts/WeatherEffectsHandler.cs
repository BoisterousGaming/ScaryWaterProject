using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherEffectsHandler : MonoBehaviour 
{
    float mfWETimeLimit = 0f;
    float mfTimeInCurWEState = 0f;
    float mfTimeStamp = 0f;
    float mfTimeToStartWE = 0f;
    int[] mArrOfWET = { 1, 2, 3 };
    float[] mArrOfTimeInterval = { 250, 500, 350, 400, 650 };
    float[] mArrOfTimeToPlayWE = { 50, 20, 100, 70, 35 };
    int miWEToPlay = 0;
    int miWEStartTime = 0;
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
                int tIndex = _GenerateRandomValueScr.Random(0, mArrOfTimeToPlayWE.Length - 1);
                mfWETimeLimit = mArrOfTimeToPlayWE[tIndex];
                mfTimeInCurWEState = 0f;
                mbDisableWE = true;
            }
        }

        else
        {
            int tProbability = _GenerateRandomValueScr.Random(0, 10);
            if (tProbability > 7)
                SetWEStartTimeAndType();    
        }
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
        miWEStartTime = _GenerateRandomValueScr.Random(0, mArrOfTimeInterval.Length - 1);
        miWEToPlay = _GenerateRandomValueScr.Random(0, mArrOfWET.Length - 1);
        mfTimeStamp = mArrOfTimeInterval[miWEStartTime];
        mfTimeToStartWE = 0f;
        mbEnableWE = true;
    }

    void PlayWE()
    {
        switch (miWEToPlay)
        {
            case 1:
                StartCoroutine(ICloudWEState(0f, true));
                StartCoroutine(IRainWEState(5f, true));
                StartCoroutine(IThunderWEState(2f, true));
                break;

            case 2:
                StartCoroutine(ICloudWEState(0f, true));
                StartCoroutine(IRainWEState(3f, true));
                break;

            case 3:
                StartCoroutine(ICloudWEState(0f, true));
                StartCoroutine(IThunderWEState(5f, true));
                break;
        }
        SetWEPlayingState();
    }

    void StopWE()
    {
        StartCoroutine(IThunderWEState(2f));
        StartCoroutine(IRainWEState(5f));
        StartCoroutine(ICloudWEState(5f));
        mbWEIsEnable = false;
        mbWETimeLimitIsSet = false;
        SetWEPlayingState();
    }

    IEnumerator IThunderWEState(float fTimeDelay, bool bState = false)
    {
        yield return new WaitForSeconds(fTimeDelay);
        if (_playerManager._playerHandler._WETScr._Thunder != null)
        {
            if (bState)
                _playerManager._playerHandler._WETScr._Thunder.Play();
            else
                _playerManager._playerHandler._WETScr._Thunder.Stop();
        }
    }

    IEnumerator IRainWEState(float fTimeDelay, bool bState = false)
    {
        yield return new WaitForSeconds(fTimeDelay);
        if (_playerManager._playerHandler._WETScr._Rain != null)
        {
            if (bState)
                _playerManager._playerHandler._WETScr._Rain.Play();
            else
                _playerManager._playerHandler._WETScr._Rain.Stop();
        }
    }

    IEnumerator ICloudWEState(float fTimeDelay, bool bState = false)
    {
        yield return new WaitForSeconds(fTimeDelay);
        if (_playerManager._playerHandler._WETScr._Cloud != null)
        {
            if (bState)
                _playerManager._playerHandler._WETScr._Cloud.Play();
            else
                _playerManager._playerHandler._WETScr._Cloud.Stop();
        }
    }
}
