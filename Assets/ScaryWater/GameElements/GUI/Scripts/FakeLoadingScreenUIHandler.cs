using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FakeLoadingScreenUIHandler : GUIItemsManager 
{
    static FakeLoadingScreenUIHandler mInstance;
    float mfTimeInGameLoadingBufferState = 5f;
    float mfRotationSpeed = -60f;
	bool mbInGameLoadingBufferState = true;
    bool mbSkipSetting = false;

    public Image _snakeLoadingBar;
    public Button _PlayBtn;
    public TextMeshProUGUI _PlayBtnText;

    public static FakeLoadingScreenUIHandler Instance
    {
        get { return mInstance; }
    }

    void Awake()
    {
        base.Init();

        if (mInstance == null)
            mInstance = this;

        else if (mInstance != this)
            Destroy(this.gameObject);
    }

    void Start()
    {
        mbInGameLoadingBufferState = true;
        _PlayBtn.enabled = false;
        _PlayBtnText.enabled = false;
    }

    void Update()
    {
		if (mbInGameLoadingBufferState)
		{
			mfTimeInGameLoadingBufferState -= Time.deltaTime;
			if (mfTimeInGameLoadingBufferState < 0.2f)
    			mbInGameLoadingBufferState = false;
            
			_snakeLoadingBar.rectTransform.Rotate(0f, 0f, 6.0f * mfRotationSpeed * Time.deltaTime);
		}

        else if (!mbInGameLoadingBufferState)
        {
            if (!mbSkipSetting)
            {
                mbSkipSetting = true;
                _snakeLoadingBar.gameObject.SetActive(false);
                _PlayBtn.enabled = true;
                _PlayBtnText.enabled = true;
            }
        }
    }

    public override void OnButtonCallBack(GUIItem item)
    {
        //Debug.Log("Button Pressed: " + item.gameObject.name);

        switch (item.gameObject.name)
        {
            case "PlayBtn":
                UICanvasHandler.Instance.LoadScreen("HUDCanvas");
                UICanvasHandler.Instance.DestroyScreen(this.gameObject);
                PlayerManager.Instance.SetupPlayerForFirstJump();
                break;
        }
    }
}