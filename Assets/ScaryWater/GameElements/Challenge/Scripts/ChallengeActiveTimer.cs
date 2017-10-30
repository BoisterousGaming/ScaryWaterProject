using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeActiveTimer : MonoBehaviour 
{
    int miTimerActive;
    DateTime mTargetDate;

    public ChallengeCommenceTimer _ChallengeCommenceTimerScr;
    public Button _ChallengeButton;
    public Text _InfoText;
    public double _TimeInSeconds = 604800;

    public void Initialize()
    {
        miTimerActive = DataManager.GetChallengeActiveTimerState();

        if (miTimerActive == 1)
        {
            ChallengeBtnState();
            mTargetDate = Convert.ToDateTime(DataManager.GetChallengeActiveTimeStampTarget());
        }

        else
            ActivateTimer();
    }

    void ActivateTimer()
    {
        ChallengeBtnState();

        mTargetDate = DateTime.Now.AddSeconds(_TimeInSeconds);
        DataManager.SetChallengeActiveTimeStampTarget(Convert.ToString(mTargetDate));

        miTimerActive = 1;
        DataManager.SetChallengeActiveTimerState(miTimerActive);
    }

    void ChallengeBtnState()
    {
        if (_ChallengeCommenceTimerScr._bDisableEnvLock)
        {
            _ChallengeButton.interactable = true;
            _InfoText.text = "Challenge Mode";
        }

        else
        {
            _ChallengeButton.interactable = false;
            _InfoText.text = "LOCKED";
        }
    }

    void Update()
    {
        if (miTimerActive == 1)
        {
            TimeSpan tDifference = mTargetDate.Subtract(DateTime.Now);
            //Debug.Log("CAT: " + tDifference);

            if (tDifference.TotalSeconds <= 0)
                OnChallengeNotAvailable();
        }
    }

    void OnChallengeNotAvailable()
    {
        miTimerActive = 0;
        _InfoText.text = "LOCKED";
        _ChallengeButton.interactable = false;
    }

    public void ResetChallengeActiveTimer()
    {
        miTimerActive = DataManager.GetChallengeActiveTimerState();
        ActivateTimer();
    }
}
