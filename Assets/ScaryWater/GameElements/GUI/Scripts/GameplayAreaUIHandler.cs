using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayAreaUIHandler : GUIItemsManager 
{
    int miCurrentScore;
    CoinSpriteScr mCoinSpriteScr;
    ButterflySpriteScr mButterflySpriteScr;
    OnScreenScoreScr mOnScreenScoreScr;
    static GameplayAreaUIHandler mInstance;

    public TextMeshProUGUI[] _arrOfAllTMPTextElement;
    public Button _magnetBtn;
    public Button _airwingBtn;
	public Image _CoinImage;
	public Image _ButterflyImage;
    public TextMeshProUGUI _OnScreenScore;
    public RectTransform _CenterPointRect;
	public RectTransform _CoinTargetPosRect;
	public RectTransform _ButterflyTargetPosRect;
    public RectTransform _ScoreTargetPosRect;

	public static GameplayAreaUIHandler Instance
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
        _arrOfAllTMPTextElement[3].text = DataManager.GetLiveAmount().ToString();
        _arrOfAllTMPTextElement[4].text = DataManager.GetPoisonAmount().ToString();
        _arrOfAllTMPTextElement[5].text = DataManager.GetMagnetAmount().ToString();
        _arrOfAllTMPTextElement[6].text = DataManager.GetAirwingAmount().ToString();

        Invoke("InitializeCallback", 1.0f);
    }

    void InitializeCallback()
    {
        PlayerManager.Instance._BarProgressSpriteScr._FillCountChangedCallback += PrintHealthCountCallback;
    }


	public void InstantiateCoin(Transform coinTransform = null, bool state = false)
	{
        Image tGoCoin = Instantiate(_CoinImage);
        mCoinSpriteScr = tGoCoin.GetComponent<CoinSpriteScr>();
        mCoinSpriteScr._GameplayAreaUIHandlerScr = this;
        RectTransform tRectTransform = this.GetComponent<RectTransform>();
        if (state)
            mCoinSpriteScr.Initialize(null, _CenterPointRect, false);
        else
            mCoinSpriteScr.Initialize(coinTransform, tRectTransform);
	}

	public void InstantiateButterfly(Transform butterflyTransform = null, bool state = false)
	{
        Image tGoButterfly = Instantiate(_ButterflyImage);
        mButterflySpriteScr = tGoButterfly.GetComponent<ButterflySpriteScr>();
        mButterflySpriteScr._GameplayAreaUIHandlerScr = this;
        RectTransform tRectTransform = this.GetComponent<RectTransform>();
        if (state)
            mButterflySpriteScr.Initialize(null, _CenterPointRect, false);

        else
            mButterflySpriteScr.Initialize(butterflyTransform, tRectTransform);
	}

    public void DisplayOnScreenScore(int scoreValue = 0)
    {
        TextMeshProUGUI tGoScore = Instantiate(_OnScreenScore);
        mOnScreenScoreScr = tGoScore.GetComponent<OnScreenScoreScr>();
        mOnScreenScoreScr._GameplayAreaUIHandlerScr = this;
        RectTransform tRectTransform = this.GetComponent<RectTransform>();
        mOnScreenScoreScr.Initialize(scoreValue, tRectTransform);
    }

    public void DisplayCurrentScore()
    {
        _arrOfAllTMPTextElement[0].text = DataManager.GetCSessionScore().ToString();
    }

    public void DisplayCoinCount()
    {
        _arrOfAllTMPTextElement[1].text = DataManager.GetCSessionCoinAmount().ToString(); 
    }

    public void DisplayButterflyCount()
    {
        _arrOfAllTMPTextElement[2].text = DataManager.GetCSessionButterflyAmount().ToString();
    }

    void PrintHealthCountCallback(int val)
    {
        _arrOfAllTMPTextElement[3].text = val.ToString();
    }

    public void DisplayPoisonCount()
    {
        _arrOfAllTMPTextElement[4].text = DataManager.GetPoisonAmount().ToString();
    }

    public void DisplayAirwingCount()
    {
        _arrOfAllTMPTextElement[6].text = DataManager.GetAirwingAmount().ToString();
    }

    public override void OnButtonCallBack(GUIItem item)
    {
		//Debug.Log("Button Pressed: " + item.gameObject.name);

		switch (item.gameObject.name)
		{
			case "PauseBtn":
                //CEffectsPlayer.Instance.Play("GeneralClick");
                UICanvasHandler.Instance.LoadScreen("PauseScreenCanvas");
				break;

            case "MagnetBtn":
                if (DataManager.GetMagnetAmount() > 0)
                {
                    //CEffectsPlayer.Instance.Play("MagnetActive");
                    DataManager.SubstarctFromMagnetAmount(1);
                    CollectableAndFoodManager.Instance.EnableMagnet();
                    _arrOfAllTMPTextElement[5].text = DataManager.GetMagnetAmount().ToString();
                }
                break;

			case "AirWingBtn":
                //CEffectsPlayer.Instance.Play("AirwingsActive");
                if (DataManager.GetAirwingAmount() > 0)
                    FriendManager.Instance.InstantiateAirWings(PlayerManager.Instance._playerHandler._tPlayerTransform.position.x);
				break;
		}
    }

    public override void OnBackButton()
    {
        for (int i = 0; i < UICanvasHandler.Instance._ActiveCanvas.Count; i++)
        {
            GameObject tCanvas = UICanvasHandler.Instance._ActiveCanvas[i];
            if (tCanvas.GetComponent<Canvas>().sortingOrder > 0)
                return;
        }
        UICanvasHandler.Instance.LoadScreen("PauseScreenCanvas");
    }

    void OnDisable()
    {
        if (PlayerManager.Instance._BarProgressSpriteScr._FillCountChangedCallback != null)
            PlayerManager.Instance._BarProgressSpriteScr._FillCountChangedCallback -= PrintHealthCountCallback;
    }

    public void SetMagnetBtnState(bool state = true)
    {
        _magnetBtn.interactable = state;
    }

    public void SetAirwingBtnState(bool state = true)
    {
        _airwingBtn.interactable = state;
    }
}
