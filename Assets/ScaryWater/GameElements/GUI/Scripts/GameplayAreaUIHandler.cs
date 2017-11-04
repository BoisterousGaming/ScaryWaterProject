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
    public Text _Score;
    public Text _HealthCount;
    public Text _CoinCount;
    public Text _ButterflyCount;
    public Text _SwipeInfo;
	public Image _CoinImage;
	public Image _ButterflyImage;
    public RectTransform _LeftLanePosRect;
    public RectTransform _MiddleLanePosRect;
    public RectTransform _RightLanePosRect;
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
        _HealthCount.text = DataManager.GetLiveAmount().ToString();
        Invoke("InitializeCallback", 1.0f);
    }

    void InitializeCallback()
    {
        PlayerManager.Instance._BarProgressSpriteScr._FillCountChangedCallback += PrintHealthCountCallback;
    }


	public void InstantiateCoin()
	{
		Image tGoCoin = Instantiate(_CoinImage);
        mCoinSpriteScr = tGoCoin.GetComponent<CoinSpriteScr>();
        mCoinSpriteScr._GameplayAreaUIHandlerScr = this;
        mCoinSpriteScr.Initialize();
	}

	public void InstantiateButterfly()
	{
		Image tGoButterfly = Instantiate(_ButterflyImage);
        mButterflySpriteScr = tGoButterfly.GetComponent<ButterflySpriteScr>();
        mButterflySpriteScr._GameplayAreaUIHandlerScr = this;
        mButterflySpriteScr.Initialize();
	}

    void PrintHealthCountCallback(int val)
    {
        if (_HealthCount != null)
            _HealthCount.text = val.ToString();
    }

    public void DisplayCoin()
    {
        if (_CoinCount != null)
            _CoinCount.text = DataManager.GetCSessionCoinAmount().ToString();
    }

    public void DisplayButterfly()
    {
        if (_ButterflyCount != null)
            _ButterflyCount.text = DataManager.GetCSessionButterflyAmount().ToString();
    }

    public void DisplayScore()
    {
        if (_Score != null)
            _Score.text = DataManager.GetCSessionScore().ToString();
    }

    public override void OnButtonCallBack(GUIItem item)
    {
		//Debug.Log("Button Pressed: " + item.gameObject.name);

		switch (item.gameObject.name)
		{
			case "PauseBtn":
                UICanvasHandler.Instance.LoadScreen("PauseScreenCanvas", null, true);
				break;

            case "MagnetBtn":
                if (DataManager.GetMagnetAmount() > 0)
                {
                    DataManager.SubstarctFromMagnetAmount(1);
                    CollectableAndFoodManager.Instance.EnableMagnet();
                }
                break;

			case "AirWingBtn":
                if (DataManager.GetAirwingAmount() > 0)
                {
                    DataManager.SubstarctFromAirwingAmount(1);
                    FriendManager.Instance.InstantiateAirWings(PlayerManager.Instance._playerHandler._tPlayerTransform.position.x);
                }
				break;
		}
    }

    public override void OnBackButton()
    {
        UICanvasHandler.Instance.LoadScreen("PauseScreenCanvas", null, true);
    }

    void OnDisable()
    {
        if (PlayerManager.Instance._BarProgressSpriteScr._FillCountChangedCallback != null)
            PlayerManager.Instance._BarProgressSpriteScr._FillCountChangedCallback -= PrintHealthCountCallback;
    }
}
