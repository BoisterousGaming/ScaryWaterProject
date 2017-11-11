using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayAreaUIHandler : GUIItemsManager 
{
    int miCurrentScore;
    CoinSpriteScr mCoinSpriteScr;
    ButterflySpriteScr mButterflySpriteScr;
    static GameplayAreaUIHandler mInstance;

    //public BarProgress _HealthBar;
    public Text[] _arrOfAllTextElement;
    public Button _magnetBtn;
    public Button _airwingBtn;
	public Image _CoinImage;
	public Image _ButterflyImage;
    public RectTransform _CenterPointRect;
	public RectTransform _CoinTargetPosRect;
	public RectTransform _ButterflyTargetPosRect;

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
        _arrOfAllTextElement[3].text = DataManager.GetLiveAmount().ToString();
        _arrOfAllTextElement[4].text = DataManager.GetPoisonAmount().ToString();
        _arrOfAllTextElement[5].text = DataManager.GetMagnetAmount().ToString();
        _arrOfAllTextElement[6].text = DataManager.GetAirwingAmount().ToString();

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

    public void DisplayCurrentScore()
    {
        _arrOfAllTextElement[0].text = DataManager.GetCSessionScore().ToString();
    }

    public void DisplayCoinCount()
    {
        _arrOfAllTextElement[1].text = DataManager.GetCSessionCoinAmount().ToString(); 
    }

    public void DisplayButterflyCount()
    {
        _arrOfAllTextElement[2].text = DataManager.GetCSessionButterflyAmount().ToString();
    }

    void PrintHealthCountCallback(int val)
    {
        _arrOfAllTextElement[3].text = val.ToString();
    }

    public void DisplayPoisonCount()
    {
        _arrOfAllTextElement[4].text = DataManager.GetPoisonAmount().ToString();
    }

    public void DisplayAirwingCount()
    {
        _arrOfAllTextElement[6].text = DataManager.GetAirwingAmount().ToString();
    }

    public override void OnButtonCallBack(GUIItem item)
    {
		//Debug.Log("Button Pressed: " + item.gameObject.name);

		switch (item.gameObject.name)
		{
			case "PauseBtn":
                CEffectsPlayer.Instance.Play("GeneralClick");
                UICanvasHandler.Instance.LoadScreen("PauseScreenCanvas", null, true);
				break;

            case "MagnetBtn":
                if (DataManager.GetMagnetAmount() > 0)
                {
                    CEffectsPlayer.Instance.Play("MagnetActive");
                    DataManager.SubstarctFromMagnetAmount(1);
                    CollectableAndFoodManager.Instance.EnableMagnet();
                    _arrOfAllTextElement[5].text = DataManager.GetMagnetAmount().ToString();
                }
                break;

			case "AirWingBtn":
                CEffectsPlayer.Instance.Play("AirwingsActive");
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
        UICanvasHandler.Instance.LoadScreen("PauseScreenCanvas", null, true);
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
