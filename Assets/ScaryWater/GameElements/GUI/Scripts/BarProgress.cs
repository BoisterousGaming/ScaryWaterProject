using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BarProgress : MonoBehaviour 
{
    static BarProgress mInstance;
	float mfTargetValue = 3;
	float mfCurTargetValue = 3;
	float mfStartValue = 3;
	float mfDifference = 0;
	float mfTime = 1;
	float mfCount = 0;
	float mfProgress = 0;
	int miNumberOfFills;
	int miCurNumberOfFills;
	bool mbAnimating = false;

    public delegate void FillsCountChanged(int val);
    public FillsCountChanged _FillCountChangedCallback;
    //public Gradient _Gradient;
    public float _TargetValue = 3;
    public float _Speed = 10;
    public Image _ProgressBar;
    public float _CurrentFillAmount;

    public static BarProgress Instance
    {
        get { return mInstance; }
    }

    void Awake()
    {
        mInstance = this;
        _TargetValue = DataManager.GetLiveAmount();
        mfTargetValue = _TargetValue;
        mfCurTargetValue = mfTargetValue;
        miCurNumberOfFills = DataManager.GetLiveAmount();
    }

    void Update()
    {
        //Debug.Log("Current Fill amount: " + _ProgressBar.fillAmount);
        CheckNewTargetValue();

        if (Mathf.Abs(_CurrentFillAmount - _ProgressBar.fillAmount) >= 0.01f)
            _CurrentFillAmount = _ProgressBar.fillAmount;

        if (mbAnimating && _ProgressBar != null)
        {
            mfCount += Time.deltaTime;
            mfProgress = mfCount / mfTime;

			if (mfProgress >= 1)
			{
				mfProgress = 1.0f;
				mbAnimating = false;
			}
            mfCurTargetValue = mfStartValue + mfDifference * mfProgress;
            //Debug.Log("CurTargetValue: " + mfCurTargetValue);
            miNumberOfFills = (int)mfCurTargetValue;
            
            if (miNumberOfFills != miCurNumberOfFills)
            {
                miCurNumberOfFills = miNumberOfFills;
                DataManager.SetLiveAmount(miCurNumberOfFills);
                if (_FillCountChangedCallback != null)
                    _FillCountChangedCallback(miCurNumberOfFills);
            }
            _ProgressBar.fillAmount = mfCurTargetValue - miNumberOfFills;
            //_ProgressBar.color = _Gradient.Evaluate(_ProgressBar.fillAmount);
        }
    }

    void CheckNewTargetValue()
    {
        if (Mathf.Abs(_TargetValue - mfTargetValue) >= 0.01f)
            SetTargetValue(_TargetValue);
    }

    public void AddDamage(float damage, Action GameOver)
    {
        if (mfTargetValue - damage < 0)
            GameOver();
        
        else
            _TargetValue -= damage;
    }

	public void AddHealth(float health, Action CannotAddAnyMore)
	{
		if (_TargetValue + health > 999)
			CannotAddAnyMore();
        
		else
            _TargetValue += health;
	}

	public void SetTargetValue(float pTargetValue)
	{
		mfTargetValue = pTargetValue;
        mfStartValue = mfCurTargetValue;

		mfDifference = mfTargetValue - mfCurTargetValue;
		mfTime = Mathf.Abs(mfDifference) / _Speed;

		mfProgress = 0;
        mfCount = 0;

		mbAnimating = true;
	}

    public int GetNumberOfFills()
    {
        return miCurNumberOfFills;
    }
}
