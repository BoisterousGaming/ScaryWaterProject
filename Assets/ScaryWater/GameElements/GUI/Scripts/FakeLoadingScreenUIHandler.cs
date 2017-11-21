using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FakeLoadingScreenUIHandler : GUIItemsManager 
{
    static FakeLoadingScreenUIHandler mInstance;
	float mfTimeInGameLoadingBufferState;
    float mfLerpingSpeed = 0.21f;
	bool mbInGameLoadingBufferState = true;

    public Image _loadingBar;

    public static FakeLoadingScreenUIHandler Instance
    {
        get { return mInstance; }
    }

    void Awake()
    {
        base.Init();

		_loadingBar.fillAmount = 0;

        if (mInstance == null)
            mInstance = this;

        else if (mInstance != this)
            Destroy(this.gameObject);
    }

    void Start()
    {
        mbInGameLoadingBufferState = true;    
    }

    void Update()
    {
		if (mbInGameLoadingBufferState)
		{
			mfTimeInGameLoadingBufferState += Time.deltaTime;
			if (mfTimeInGameLoadingBufferState > 0.25f)
    			mbInGameLoadingBufferState = false;
			return;
		}

        if (_loadingBar != null)
            _loadingBar.fillAmount = Mathf.Lerp(_loadingBar.fillAmount, 1.0f, Time.deltaTime * mfLerpingSpeed);

        if (_loadingBar.fillAmount >= 0.95f)
        {
            UICanvasHandler.Instance.LoadScreen("HUDCanvas");
            UICanvasHandler.Instance.DestroyScreen(this.gameObject);
        }
    }
}