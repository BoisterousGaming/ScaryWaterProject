using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BarProgressSprite : MonoBehaviour 
{
    static BarProgressSprite mInstance;
	float mfTargetValue = 3;
	float mfCurTargetValue = 3;
	float mfStartValue = 3;
	float mfDifference = 0;
	float mfTime = 1;
	float mfCount = 0;
	float mfProgress = 0;
    float mfKingfisherBarPosYAxis = 2.25f;
    float mfDragonflyBarPosYAxis = 2.25f;
    float mfDuckBarPosYAxis = 2.2f;
    float mfBarInitialPosYAxis = 1.5f;
	int miNumberOfFills;
	int miCurNumberOfFills;
	bool mbAnimating = false;
    bool mbEmptyTheBar = false;
    Vector3 mvTempVector;

    public delegate void FillsCountChanged(int val);
    public FillsCountChanged _FillCountChangedCallback;
    //public Gradient _Gradient;
    public float _TargetValue = 3;
    public float _Speed = 10;
    public float _CurrentFillAmount;
    public GameObject _ProgressBar;
    public PlayerHandler _PlayerHandlerScr;

    public static BarProgressSprite Instance
    {
        get { return mInstance; }
    }

    void Awake()
    {
        mInstance = this;
        _TargetValue = DataManager.GetLiveAmount() + 0.99f;
        mfTargetValue = _TargetValue;
        mfCurTargetValue = mfTargetValue;
        miCurNumberOfFills = DataManager.GetLiveAmount() + 1;
        mfBarInitialPosYAxis = this.transform.position.y;
    }

    void Update()
    {
        //Debug.Log("Current Fill amount: " + _ProgressBar.fillAmount);
        CheckNewTargetValue();
        if (Mathf.Abs(_CurrentFillAmount - _ProgressBar.transform.localScale.x) >= 0.01f)
            _CurrentFillAmount = _ProgressBar.transform.localScale.x;

        if (mbAnimating && _ProgressBar != null)
        {
            mfCount += Time.deltaTime;
            mfProgress = mfCount / mfTime;

            if (mfProgress >= 1)
            {
                mfProgress = 1.0f;
                mbAnimating = false;
                mfCurTargetValue = mfTargetValue;
                //Debug.Log("CurTargetValue: " + mfCurTargetValue);
                miNumberOfFills = (int)mfCurTargetValue;

                if (miNumberOfFills != miCurNumberOfFills)
                {
                    miCurNumberOfFills = miNumberOfFills;
                    DataManager.SetLiveAmount(miCurNumberOfFills);
                    if (_FillCountChangedCallback != null)
                        _FillCountChangedCallback(miCurNumberOfFills);
                }

                mvTempVector.x = 1.0f;
                mvTempVector.y = 1.0f;
                mvTempVector.x = mfCurTargetValue - miNumberOfFills;
                _ProgressBar.transform.localScale = mvTempVector;
            }
            else
            {
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

                mvTempVector.x = 1.0f;
                mvTempVector.y = 1.0f;
                mvTempVector.x = mfCurTargetValue - miNumberOfFills;
                _ProgressBar.transform.localScale = mvTempVector;
            }
            //_ProgressBar.color = _Gradient.Evaluate(_ProgressBar.fillAmount);
        }

        //HideProgressBar();
        SetBarPosition();
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
        _TargetValue = pTargetValue;
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

    void HideProgressBar()
    {
        Color tColor;

        if (!FriendManager._bPlayerIsWithAFriend)
            tColor = new Color(1f, 1f, 1f, 1f);
        else
            tColor = new Color(1f, 1f, 1f, 0f);
        
        GetComponent<SpriteRenderer>().color = tColor;
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = tColor;
    }

    void SetBarPosition()
    {
        if (FriendManager._bPlayerIsWithAFriend)
        {
            if (FriendManager._eFriendType == eFriendType.Duck)
                SetPos(mfDuckBarPosYAxis);

            else if (FriendManager._eFriendType == eFriendType.Kingfisher)
                SetPos(mfKingfisherBarPosYAxis);

            else if (FriendManager._eFriendType == eFriendType.Dragonfly)
                SetPos(mfDragonflyBarPosYAxis);
        }

        else
            SetPos(mfBarInitialPosYAxis);
    }

    void SetPos(float offset)
    {
        mvTempVector.x = this.transform.position.x;
        mvTempVector.y = _PlayerHandlerScr.transform.position.y + offset;
        mvTempVector.z = this.transform.position.z;

        this.transform.position = mvTempVector;
    }
}
