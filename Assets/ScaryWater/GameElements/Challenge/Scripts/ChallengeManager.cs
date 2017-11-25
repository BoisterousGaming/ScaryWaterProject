using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeManager : MonoBehaviour 
{
    bool mbChallengeIsComplete = false;
    static ChallengeManager mInstance;

    public PlayerManager _PlayerManagerScr;
    public int _iHowManySetsToCover = 6;

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

        _iHowManySetsToCover *= (int)DataHandler._fEnvironmentSetLength;
    }

    void Update()
    {
        if (_PlayerManagerScr._playerHandler._tPlayerTransform.position.z > _iHowManySetsToCover)
            ChallengeIsComplete();    
    }

    public void ChallengeIsComplete()
    {
        PlayerManager.Instance._playerHandler._jumpActionScr.StopJump("death");
        GameObject tCanvas = UICanvasHandler.Instance.GetActiveCanvasByName("HUDCanvas");

        if (tCanvas != null)
            UICanvasHandler.Instance.DestroyScreen(tCanvas);

        UICanvasHandler.Instance.LoadScreen("GameOverCanvas");
        mbChallengeIsComplete = true;
    }

    public void TerminateChallengeCallback(int val)
    {
        if (mbChallengeIsComplete)
            return;
        
        PlayerManager.Instance._playerHandler._jumpActionScr.StopJump("death");
        GameObject tCanvas = UICanvasHandler.Instance.GetActiveCanvasByName("HUDCanvas");

        if (tCanvas != null)
            UICanvasHandler.Instance.DestroyScreen(tCanvas);

        UICanvasHandler.Instance.LoadScreen("GameOverCanvas");
    }

    void OnDisable()
    {
        if (_PlayerManagerScr._BarProgressSpriteScr._FillCountChangedCallback != null)
            _PlayerManagerScr._BarProgressSpriteScr._FillCountChangedCallback -= TerminateChallengeCallback;
    }
}
