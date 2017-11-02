using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeCommenceTimer : MonoBehaviour 
{
    int miTimerActive;
    DateTime mTargetDate;

    public ChallengeActiveTimer _ChallengeActiveTimerScr;
    public Button _ChallengeButton;
    public Text _InfoText;
    public double _TimeInSeconds = 604800;
    public bool _bDisableEnvLock = false;
    public bool _bEnableChallenge = false;
    public Sprite[] _arrOfChallengeBtnSprites;

    void Start()
    {
        _ChallengeButton.interactable = true;

        miTimerActive = DataManager.GetChallengeCommenceTimerState();

        if (miTimerActive == 1)
            mTargetDate = Convert.ToDateTime(DataManager.GetChallengeCommenceTimeStampTarget());

        else
            ActivateTimer();

        if (DataManager.GetAllEnvPurchasedState())
            _bDisableEnvLock = true;

        else
        {
            _bDisableEnvLock = false;
            _InfoText.text = "LOCKED";
            _ChallengeButton.GetComponent<Image>().sprite = _arrOfChallengeBtnSprites[0];
        }     
    }

    void ActivateTimer()
    {
        _ChallengeButton.interactable = true;
        _ChallengeButton.GetComponent<Image>().sprite = _arrOfChallengeBtnSprites[1];

        mTargetDate = DateTime.Now.AddSeconds(_TimeInSeconds);
        DataManager.SetChallengeCommenceTimeStampTarget(Convert.ToString(mTargetDate));

        miTimerActive = 1;
        DataManager.SetChallengeCommenceTimerState(miTimerActive);
    }

    void Update()
    {
        if (miTimerActive == 1)
        {
            TimeSpan tDifference = mTargetDate.Subtract(DateTime.Now);
            //Debug.Log("CCT: " + tDifference);

            if (_bDisableEnvLock)
            {
                string tDays = tDifference.Days.ToString("D2");
                string tHours = tDifference.Hours.ToString("D2");
                string tMin = tDifference.Minutes.ToString("D2");
                string tSec = tDifference.Seconds.ToString("D2");
                _InfoText.text = tDays + ":" + tHours + ":" + tMin + ":" + tSec;
            }

            if (tDifference.TotalSeconds <= 0)
                OnChallengeAvailable();
        }
    }

    void OnChallengeAvailable()
    {
        miTimerActive = 0;
        _bEnableChallenge = true;
        _ChallengeActiveTimerScr.Initialize();
    }

    public void ResetChallengeCommenceTimer()
    {
        miTimerActive = DataManager.GetChallengeCommenceTimerState();
        ActivateTimer();
        _ChallengeActiveTimerScr.ResetChallengeActiveTimer();
    }
}
