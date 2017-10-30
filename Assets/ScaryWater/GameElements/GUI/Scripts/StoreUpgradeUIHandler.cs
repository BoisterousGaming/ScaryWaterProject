using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreUpgradeUIHandler : GUIItemsManager
{
    static StoreUpgradeUIHandler mInstance;
    int miLivePrice = 100;
    int miPoisonPrice = 100;
    int miAirwingPrice = 100;
    int miMagnetPrice = 100;
    int miTotalLivePrice;
    int miTotalPoisonPrice;
    int miTotalAirwingPrice;
    int miTotalMagnetPrice;
    int miLiveCount = 1;
    int miPoisonCount = 1;
    int miAirwingCount = 1;
    int miMagnetCount = 1;
    int miTotalLiveBought;
    int miTotalPoisonBought;
    int miTotalAirwingBought;
    int miTotalMagnetBought;
    int miTempCount = 1;

    public Text _LiveCount;
    public Text _LivePrice;
    public Text _PoisonCount;
    public Text _PoisonPrice;
    public Text _AirwingCount;
    public Text _AirwingPrice;
    public Text _MagnetCount;
    public Text _MagnetPrice;

    public static StoreUpgradeUIHandler Instance
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
        Initialized();
    }

    void Initialized()
    {
		_LiveCount.text = miLiveCount.ToString();
		_PoisonCount.text = miPoisonCount.ToString();
		_AirwingCount.text = miAirwingCount.ToString();
		_MagnetCount.text = miMagnetCount.ToString();

        miTotalLivePrice = miLiveCount * miLivePrice;
        _LivePrice.text = miTotalLivePrice.ToString();

        miTotalPoisonPrice = miPoisonCount * miPoisonPrice;
        _PoisonPrice.text = miTotalPoisonPrice.ToString();

        miTotalAirwingPrice = miAirwingCount * miAirwingPrice;
        _AirwingPrice.text = miTotalAirwingPrice.ToString();

        miTotalMagnetPrice = miMagnetCount * miMagnetPrice;
        _MagnetPrice.text = miTotalMagnetPrice.ToString();
    }

    public override void OnButtonCallBack(GUIItem item)
    {
        //Debug.Log("Button Pressed: " + item.gameObject.name);
        switch (item.gameObject.name)
        {
			case "LivesDecBtn":
                if (miLiveCount > 1)
                    miLiveCount -= 1;

                miTotalLivePrice = miLiveCount * miLivePrice;
                _LiveCount.text = miLiveCount.ToString();
                _LivePrice.text = miTotalLivePrice.ToString();
				break;

            case "LivesIncBtn":                 int tValue = DataManager.GetLiveAmount();                 int tCount = miLiveCount;                 miTempCount += 1;                 tValue += miTempCount;                 if (tValue <= 999)                 {                     miLiveCount += 1;                     miTotalLivePrice = miLiveCount * miLivePrice;                     _LiveCount.text = miLiveCount.ToString();                     _LivePrice.text = miTotalLivePrice.ToString();                 }                  else                     miTempCount = tCount;                 break;

            case "LivesBuyBtn":
                if (DataManager.GetTotalCoinAmount() >= miTotalLivePrice)
                {
                    DataManager.AddToLiveAmount(miLiveCount);
                    DataManager.SubstractFromTotalCoin(miTotalLivePrice);
                }
                break;

            case "PoisonDecBtn":
				if (miPoisonCount > 1)
					miPoisonCount -= 1;

				miTotalPoisonPrice = miPoisonCount * miPoisonPrice;
                _PoisonCount.text = miPoisonCount.ToString();
				_PoisonPrice.text = miTotalPoisonPrice.ToString();
                break;

            case "PoisonIncBtn":
				miPoisonCount += 1;
				miTotalPoisonPrice = miPoisonCount * miPoisonPrice;
                _PoisonCount.text = miPoisonCount.ToString();
				_PoisonPrice.text = miTotalPoisonPrice.ToString();
                break;

            case "PoisonBuyBtn":
				if (DataManager.GetTotalCoinAmount() >= miTotalPoisonPrice)
				{
                    DataManager.AddToPoisonAmount(miPoisonCount);
                    DataManager.SubstractFromTotalCoin(miTotalPoisonPrice);
				}
                break;

            case "AirwingsDecBtn":
				if (miAirwingCount > 1)
					miAirwingCount -= 1;

				miTotalAirwingPrice = miAirwingCount * miAirwingPrice;
                _AirwingCount.text = miAirwingCount.ToString();
				_AirwingPrice.text = miTotalAirwingPrice.ToString();
                break;

            case "AirwingsIncBtn":
				miAirwingCount += 1;
				miTotalAirwingPrice = miAirwingCount * miAirwingPrice;
                _AirwingCount.text = miAirwingCount.ToString();
				_AirwingPrice.text = miTotalAirwingPrice.ToString();
                break;

            case "AirwingsBuyBtn":
				if (DataManager.GetTotalCoinAmount() >= miTotalAirwingPrice)
				{
                    DataManager.AddToAirwingAmount(miAirwingCount);
                    DataManager.SubstractFromTotalCoin(miTotalAirwingPrice);
				}
                break;

            case "MagnetDecBtn":
				if (miMagnetCount > 1)
					miMagnetCount -= 1;

				miTotalMagnetPrice = miMagnetCount * miMagnetPrice;
                _MagnetCount.text = miMagnetCount.ToString();
				_MagnetPrice.text = miTotalMagnetPrice.ToString();
                break;

            case "MagnetIncBtn":
				miMagnetCount += 1;
				miTotalMagnetPrice = miMagnetCount * miMagnetPrice;
                _MagnetCount.text = miMagnetCount.ToString();
				_MagnetPrice.text = miTotalMagnetPrice.ToString();
                break;

            case "MagnetBuyBtn":
				if (DataManager.GetTotalCoinAmount() >= miTotalMagnetPrice)
				{
                    DataManager.AddToMagnetAmount(miMagnetCount);
                    DataManager.SubstractFromTotalCoin(miTotalMagnetPrice);
				}
                break;
        }
    }
}
