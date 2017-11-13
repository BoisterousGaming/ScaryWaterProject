using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroUIHandler : GUIItemsManager 
{
    static IntroUIHandler mInstance;
    float mfTimerDuration = 10f;
    bool mbDisableTimer = false;

    public Text _TimerText;

    public static IntroUIHandler Instance
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

    void Update()
    {
        if (_TimerText != null)
        {
            if (!mbDisableTimer)
            {
                if (mfTimerDuration > 0.1f)
                    mfTimerDuration -= Time.deltaTime;

                else if (mfTimerDuration < 0.2f)
                {
                    mbDisableTimer = true;
                    LoadMainMenu();
                }

                _TimerText.text = Mathf.Round(mfTimerDuration).ToString();    
            }
        }
    }

    public override void OnButtonCallBack(GUIItem item)
    {
        //Debug.Log("Button Pressed: " + item.gameObject.name);

        //CEffectsPlayer.Instance.Play("GeneralClick");

        switch(item.gameObject.name)
        {
            case "SkipBtn":
                LoadMainMenu();
                break;
        }
    }

    void LoadMainMenu()
    {
        UICanvasHandler.Instance.CloseScreen(this.name, AfterScreenClosed);
    }

    void AfterScreenClosed()
    {
        UICanvasHandler.Instance.DestroyScreen(this.gameObject);
        UICanvasHandler.Instance.LoadScreen("MainMenuCanvas");
    }
}
