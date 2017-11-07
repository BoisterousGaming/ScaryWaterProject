using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChallengeManager : MonoBehaviour 
{
    static ChallengeManager mInstance;

    public PlayerManager _PlayerManagerScr;

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
        if (_PlayerManagerScr._BarProgressSpriteScr != null)
            _PlayerManagerScr._BarProgressSpriteScr._FillCountChangedCallback += TerminateChallengeCallback;
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
        if (_PlayerManagerScr._BarProgressSpriteScr._FillCountChangedCallback != null)
            _PlayerManagerScr._BarProgressSpriteScr._FillCountChangedCallback -= TerminateChallengeCallback;
    }
}
