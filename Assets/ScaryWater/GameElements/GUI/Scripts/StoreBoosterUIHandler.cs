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
    public Text _Coin2XPrice;
    public Text _MagnetTimePrice;
    public Text _PoisonRangePrice;
    public Button _Coin2XBtn;
    public Button _MagnetTimeBtn;
    public Button _PoisonRangeBtn;
    public Image[] _arrOfMagnetTimeMeter;
    public Image[] _arrOfPoisonRangeMeter;
    public int[] _arrOfMagnetTimePrice;
    public int[] _arrOfPoisonRangePrice;

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
        if (DataManager.GetCoinValue() != 1)
        {
            _Coin2XPrice.text = " ";
            _Coin2XBtn.interactable = false;
        }

        else
            _Coin2XPrice.text = "$" + mfCoinMultiplierPrice.ToString();


        MagnetMeterAndPriceInitializer();
        PoisonMeterAndPriceInitializer();
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

    public override void OnButtonCallBack(GUIItem item)
    {
        //Debug.Log("Button Pressed: " + item.gameObject.name);

        switch (item.gameObject.name)
        {
            case "Coin2XBuyBtn":
                DataManager.SetCoinValue(2);        //cRamappa
                _Coin2XPrice.text = " ";
                _Coin2XBtn.interactable = false;
                break;

            case "MagnetTimeBuyBtn":
                if (DataManager.GetMagnetTime() != -1)
                {
					int tIndex = DataManager.GetMagnetTime();
					tIndex += 1;

                    if (tIndex >= _arrOfMagnetTimePrice.Length)
                    {
                        MagnetMeterAndPriceInitializer();
                        return;
                    }

                    if (DataManager.GetTotalCoinAmount() >= _arrOfMagnetTimePrice[tIndex])
                    {
                        DataManager.SetMagnetTime(tIndex);
						_arrOfMagnetTimeMeter[tIndex].color = mFullColor;
						_MagnetTimePrice.text = _arrOfMagnetTimePrice[tIndex].ToString();
                        DataManager.SubstractFromTotalCoin(_arrOfMagnetTimePrice[tIndex]);
                    }

                    else
                        UICanvasHandler.Instance.LoadScreen("CoinWarningCanvas", null, true);
                }

                else
                {
                    if (DataManager.GetTotalCoinAmount() >= _arrOfMagnetTimePrice[0])
                    {
                        DataManager.SetMagnetTime(0);
                        _arrOfMagnetTimeMeter[0].color = mFullColor;
                        _MagnetTimePrice.text = _arrOfMagnetTimePrice[1].ToString();
                        DataManager.SubstractFromTotalCoin(_arrOfMagnetTimePrice[0]);
                    }

                    else
                        UICanvasHandler.Instance.LoadScreen("CoinWarningCanvas", null, true);
                }
                break;

            case "PoisonRangeBuyBtn":
				if (DataManager.GetPoisonRange() != -1)
				{
                    int tIndex = DataManager.GetPoisonRange();
					tIndex += 1;

					if (tIndex >= _arrOfPoisonRangePrice.Length)
                    {
                        PoisonMeterAndPriceInitializer();
                        return;
                    }

					if (DataManager.GetTotalCoinAmount() >= _arrOfPoisonRangePrice[tIndex])
					{
                        DataManager.SetPoisonRange(tIndex);
						_arrOfPoisonRangeMeter[tIndex].color = mFullColor;
						_PoisonRangePrice.text = _arrOfPoisonRangePrice[tIndex].ToString();
                        DataManager.SubstractFromTotalCoin(_arrOfPoisonRangePrice[tIndex]);
					}

					else
                        UICanvasHandler.Instance.LoadScreen("CoinWarningCanvas", null, true);
				}

				else
				{
					if (DataManager.GetTotalCoinAmount() >= _arrOfPoisonRangePrice[0])
					{
						DataManager.SetPoisonRange(0);
						_arrOfPoisonRangeMeter[0].color = mFullColor;
						_PoisonRangePrice.text = _arrOfPoisonRangePrice[1].ToString();
                        DataManager.SubstractFromTotalCoin(_arrOfPoisonRangePrice[0]);
					}

					else
                        UICanvasHandler.Instance.LoadScreen("CoinWarningCanvas", null, true);
				}
                break;
        }
    }
}
