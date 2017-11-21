using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreBoosterUIHandler : GUIItemsManager
{
    static StoreBoosterUIHandler mInstance;
    float mfCoinMultiplierPrice = 0.99f;
    Color mFullColor = new Color(0f, 1f, 0f, 1f);

    public Image _MagnetCoinImage;
    public Image _PoisonCoinImage;
    public Image _AirwingCoinImage;
    public Text _Coin2XPrice;
    public Text _MagnetTimePrice;
    public Text _PoisonRangePrice;
    public Text _AirwingRangePrice;
    public Button _Coin2XBtn;
    public Button _MagnetTimeBtn;
    public Button _PoisonRangeBtn;
    public Button _AirwingRangeBtn;
    public Image[] _arrOfMagnetTimeMeter;
    public Image[] _arrOfPoisonRangeMeter;
    public Image[] _arrOfAirwingRangeMeter;
    public int[] _arrOfMagnetTimePrice;
    public int[] _arrOfPoisonRangePrice;
    public int[] _arrOfAirwingRangePrice;

    public static StoreBoosterUIHandler Instance
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
        DataManager.SetActiveStoreTab(this.gameObject.name);
        if (DataManager.GetCoinValue() != 1)
        {
            _Coin2XPrice.text = " ";
            _Coin2XBtn.interactable = false;
        }

        else
            _Coin2XPrice.text = "$" + mfCoinMultiplierPrice.ToString();


        MagnetMeterAndPriceInitializer();
        PoisonMeterAndPriceInitializer();
        AirwingMeterAndPriceInitializer();
    }

    void MagnetMeterAndPriceInitializer()
    {
		if (DataManager.GetMagnetTime() != -1)
		{
			int tIndex = DataManager.GetMagnetTime();

            if (tIndex >= _arrOfMagnetTimePrice.Length - 1)
            {
                _MagnetTimePrice.text = " ";
                _MagnetCoinImage.enabled = false;
                _MagnetTimeBtn.interactable = false;
            }

            else
                _MagnetTimePrice.text = _arrOfMagnetTimePrice[tIndex].ToString();

			for (int i = 0; i <= tIndex; i++)
                _arrOfMagnetTimeMeter[i].color = mFullColor;	
		}

		else
			_MagnetTimePrice.text = _arrOfMagnetTimePrice[0].ToString();
    }

    void PoisonMeterAndPriceInitializer()
    {
		if (DataManager.GetPoisonRange() != -1)
		{
			int tIndex = DataManager.GetPoisonRange();

            if (tIndex >= _arrOfPoisonRangePrice.Length - 1)
            {
                _PoisonRangePrice.text = " ";
                _PoisonCoinImage.enabled = false;
                _PoisonRangeBtn.interactable = false;
            }

            else
			    _PoisonRangePrice.text = _arrOfPoisonRangePrice[tIndex].ToString();

			for (int i = 0; i <= tIndex; i++)
                _arrOfPoisonRangeMeter[i].color = mFullColor;
		}

		else
			_PoisonRangePrice.text = _arrOfPoisonRangePrice[0].ToString();
    }

    void AirwingMeterAndPriceInitializer()
    {
        if (DataManager.GetAirwingRange() != -1)
        {
            int tIndex = DataManager.GetAirwingRange();

            if (tIndex >= _arrOfAirwingRangePrice.Length - 1)
            {
                _AirwingRangePrice.text = " ";
                _AirwingCoinImage.enabled = false;
                _AirwingRangeBtn.interactable = false;
            }

            else
                _AirwingRangePrice.text = _arrOfAirwingRangePrice[tIndex].ToString();

            for (int i = 0; i <= tIndex; i++)
                _arrOfAirwingRangeMeter[i].color = mFullColor;
        }

        else
            _AirwingRangePrice.text = _arrOfAirwingRangePrice[0].ToString();
    }

    public override void OnButtonCallBack(GUIItem item)
    {
        //Debug.Log("Button Pressed: " + item.gameObject.name);

        switch (item.gameObject.name)
        {
            case "Coin2XBuyBtn":
                //CEffectsPlayer.Instance.Play("BuyCash");
                DataManager.SetCoinValue(2);                                    //cRamappa
                _Coin2XPrice.text = " ";
                _Coin2XBtn.interactable = false;
                break;

            case "MagnetTimeBuyBtn":
                //CEffectsPlayer.Instance.Play("BuyCoin");
                if (DataManager.GetMagnetTime() != -1)
                {
					int tIndex = DataManager.GetMagnetTime();
					tIndex += 1;

                    if (DataManager.GetTotalCoinAmount() >= _arrOfMagnetTimePrice[tIndex])
                    {
                        //CEffectsPlayer.Instance.Play("UpgradeSound");
                        DataManager.SetMagnetTime(tIndex);
						_arrOfMagnetTimeMeter[tIndex].color = mFullColor;
						_MagnetTimePrice.text = _arrOfMagnetTimePrice[tIndex].ToString();
                        DataManager.SubstractFromTotalCoin(_arrOfMagnetTimePrice[tIndex]);
                    }

                    else
                        UICanvasHandler.Instance.LoadScreen("CoinWarningCanvas");

                    if (tIndex >= _arrOfMagnetTimePrice.Length - 1)
                        MagnetMeterAndPriceInitializer();
                }

                else
                {
                    if (DataManager.GetTotalCoinAmount() >= _arrOfMagnetTimePrice[0])
                    {
                        //CEffectsPlayer.Instance.Play("UpgradeSound");
                        DataManager.SetMagnetTime(0);
                        _arrOfMagnetTimeMeter[0].color = mFullColor;
                        _MagnetTimePrice.text = _arrOfMagnetTimePrice[1].ToString();
                        DataManager.SubstractFromTotalCoin(_arrOfMagnetTimePrice[0]);
                    }

                    else
                        UICanvasHandler.Instance.LoadScreen("CoinWarningCanvas");
                }
                break;

            case "PoisonRangeBuyBtn":
                //CEffectsPlayer.Instance.Play("BuyCoin");
				if (DataManager.GetPoisonRange() != -1)
				{
                    int tIndex = DataManager.GetPoisonRange();
					tIndex += 1;

					if (DataManager.GetTotalCoinAmount() >= _arrOfPoisonRangePrice[tIndex])
					{
                        //CEffectsPlayer.Instance.Play("UpgradeSound");
                        DataManager.SetPoisonRange(tIndex);
						_arrOfPoisonRangeMeter[tIndex].color = mFullColor;
						_PoisonRangePrice.text = _arrOfPoisonRangePrice[tIndex].ToString();
                        DataManager.SubstractFromTotalCoin(_arrOfPoisonRangePrice[tIndex]);
					}

					else
                        UICanvasHandler.Instance.LoadScreen("CoinWarningCanvas");

                    if (tIndex >= _arrOfPoisonRangePrice.Length - 1)
                        PoisonMeterAndPriceInitializer();
				}

				else
				{
					if (DataManager.GetTotalCoinAmount() >= _arrOfPoisonRangePrice[0])
					{
                        //CEffectsPlayer.Instance.Play("UpgradeSound");
						DataManager.SetPoisonRange(0);
						_arrOfPoisonRangeMeter[0].color = mFullColor;
						_PoisonRangePrice.text = _arrOfPoisonRangePrice[1].ToString();
                        DataManager.SubstractFromTotalCoin(_arrOfPoisonRangePrice[0]);
					}

					else
                        UICanvasHandler.Instance.LoadScreen("CoinWarningCanvas");
				}
                break;

            case "AirwingRangeBuyBtn":
                //CEffectsPlayer.Instance.Play("BuyCoin");
                if (DataManager.GetAirwingRange() != -1)
                {
                    int tIndex = DataManager.GetAirwingRange();
                    tIndex += 1;

                    if (DataManager.GetTotalCoinAmount() >= _arrOfAirwingRangePrice[tIndex])
                    {
                        //CEffectsPlayer.Instance.Play("UpgradeSound");
                        DataManager.SetAirwingRange(tIndex);
                        _arrOfAirwingRangeMeter[tIndex].color = mFullColor;
                        _AirwingRangePrice.text = _arrOfAirwingRangePrice[tIndex].ToString();
                        DataManager.SubstractFromTotalCoin(_arrOfAirwingRangePrice[tIndex]);
                    }

                    else
                        UICanvasHandler.Instance.LoadScreen("CoinWarningCanvas");

                    if (tIndex >= _arrOfAirwingRangePrice.Length - 1)
                        AirwingMeterAndPriceInitializer();
                }

                else
                {
                    if (DataManager.GetTotalCoinAmount() >= _arrOfAirwingRangePrice[0])
                    {
                        //CEffectsPlayer.Instance.Play("UpgradeSound");
                        DataManager.SetAirwingRange(0);
                        _arrOfAirwingRangeMeter[0].color = mFullColor;
                        _AirwingRangePrice.text = _arrOfAirwingRangePrice[1].ToString();
                        DataManager.SubstractFromTotalCoin(_arrOfAirwingRangePrice[0]);
                    }

                    else
                        UICanvasHandler.Instance.LoadScreen("CoinWarningCanvas");
                }
                break;
        }
    }
}
