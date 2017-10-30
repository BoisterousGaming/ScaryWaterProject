using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChallengeManager : MonoBehaviour 
{
    static ChallengeManager mInstance;
    bool mbSkipSetting = false;

    public static ChallengeManager Instance
    {
        get { return mInstance; }
    }

    void Awake()
    {
        if (mInstance == null)
            mInstance = this;

        else if (mInstance != this)
            Destroy(this.gameObject);
    }

    void Start()
    {
        EnvironmentManager.Instance.ChallengeTypeSetInstantiation();
    }

    void Update()
    {
        GameObject tCanvas = UICanvasHandler.Instance.GetActiveCanvasByName("HUDCanvas");
        if (tCanvas != null)
        {
            if (!mbSkipSetting)
            {
                mbSkipSetting = true;
                tCanvas.GetComponent<GameplayAreaUIHandler>()._HealthBar._FillCountChangedCallback += TerminateChallengeCallback;
            }
        }
    }

    public static DateTime GetCurrentDateAndTime()
    {
        return DateTime.Now;
    }

    public static void ChallengeIsComplete()
    {
        PlayerManager.Instance._playerHandler._jumpActionScr.StopJump("death");
        GameObject tCanvas = UICanvasHandler.Instance.GetActiveCanvasByName("HUDCanvas");

        if (tCanvas != null)
            UICanvasHandler.Instance.DestroyScreen(tCanvas);

        UICanvasHandler.Instance.LoadScreen("GameOverCanvas", null, true);
    }

    public static void TerminateChallengeCallback(int val)
    {
        PlayerManager.Instance._playerHandler._jumpActionScr.StopJump("death");
        GameObject tCanvas = UICanvasHandler.Instance.GetActiveCanvasByName("HUDCanvas");

        if (tCanvas != null)
            UICanvasHandler.Instance.DestroyScreen(tCanvas);

        UICanvasHandler.Instance.LoadScreen("GameOverCanvas", null, true);
    }

    void OnDestroy()
    {
        //if (BarProgress.Instance._FillCountChangedCallback != null)
            //BarProgress.Instance._FillCountChangedCallback -= TerminateChallengeCallback;    
    }
}
