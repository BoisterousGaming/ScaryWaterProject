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
    int miTempCount = 1;

    public Text[] _arrOfAllTextElement;
    public Button[] _arrOfAllBtnElement;

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
        DataManager.SetActiveStoreTab(this.gameObject.name);
        Initialized();
    }

    void Initialized()
    {
		_arrOfAllTextElement[0].text = miLiveCount.ToString();
		_arrOfAllTextElement[2].text = miPoisonCount.ToString();
		_arrOfAllTextElement[4].text = miAirwingCount.ToString();
		_arrOfAllTextElement[6].text = miMagnetCount.ToString();
        _arrOfAllTextElement[8].text = DataManager.GetLiveAmount().ToString();
        _arrOfAllTextElement[9].text = DataManager.GetPoisonAmount().ToString();
        _arrOfAllTextElement[10].text = DataManager.GetAirwingAmount().ToString();
        _arrOfAllTextElement[11].text = DataManager.GetMagnetAmount().ToString();

        miTotalLivePrice = miLiveCount * miLivePrice;
        _arrOfAllTextElement[1].text = miTotalLivePrice.ToString();

        miTotalPoisonPrice = miPoisonCount * miPoisonPrice;
        _arrOfAllTextElement[3].text = miTotalPoisonPrice.ToString();

        miTotalAirwingPrice = miAirwingCount * miAirwingPrice;
        _arrOfAllTextElement[5].text = miTotalAirwingPrice.ToString();

        miTotalMagnetPrice = miMagnetCount * miMagnetPrice;
        _arrOfAllTextElement[7].text = miTotalMagnetPrice.ToString();

        _arrOfAllBtnElement[0].interactable = false;
        _arrOfAllBtnElement[2].interactable = false;
        _arrOfAllBtnElement[4].interactable = false;
        _arrOfAllBtnElement[6].interactable = false;
    }

    public override void OnButtonCallBack(GUIItem item)
    {
        //Debug.Log("Button Pressed: " + item.gameObject.name);
        switch (item.gameObject.name)
        {
			case "LivesDecBtn":
                //CEffectsPlayer.Instance.Play("PlusMinusSound");
                if (miLiveCount > 1)
                    miLiveCount -= 1;

                miTotalLivePrice = miLiveCount * miLivePrice;
                _arrOfAllTextElement[0].text = miLiveCount.ToString();
                _arrOfAllTextElement[1].text = miTotalLivePrice.ToString();

                if (miLiveCount == 1)
                    _arrOfAllBtnElement[0].interactable = false;
				break;

            case "LivesIncBtn":
                //CEffectsPlayer.Instance.Play("PlusMinusSound");                 int tValue = DataManager.GetLiveAmount();                 int tCount = miLiveCount;                 miTempCount += 1;                 tValue += miTempCount;                 if (tValue <= 999)                 {                     miLiveCount += 1;                     miTotalLivePrice = miLiveCount * miLivePrice;                     _arrOfAllTextElement[0].text = miLiveCount.ToString();                     _arrOfAllTextElement[1].text = miTotalLivePrice.ToString();
                    _arrOfAllBtnElement[0].interactable = true;                 }                  else                     miTempCount = tCount;                 break;

            case "LivesBuyBtn":
                //CEffectsPlayer.Instance.Play("BuyCoin");
                if (DataManager.GetTotalCoinAmount() >= miTotalLivePrice)
                {
                    DataManager.AddToLiveAmount(miLiveCount);
                    DataManager.SubstractFromTotalCoin(miTotalLivePrice);
                    _arrOfAllTextElement[8].text = DataManager.GetLiveAmount().ToString();
                }

                else
                    UICanvasHandler.Instance.LoadScreen("CoinWarningCanvas");
                break;

            case "PoisonDecBtn":
                //CEffectsPlayer.Instance.Play("PlusMinusSound");
				if (miPoisonCount > 1)
					miPoisonCount -= 1;

				miTotalPoisonPrice = miPoisonCount * miPoisonPrice;
                _arrOfAllTextElement[2].text = miPoisonCount.ToString();
				_arrOfAllTextElement[3].text = miTotalPoisonPrice.ToString();

                if (miPoisonCount == 1)
                    _arrOfAllBtnElement[2].interactable = false;
                break;

            case "PoisonIncBtn":
                //CEffectsPlayer.Instance.Play("PlusMinusSound");
				miPoisonCount += 1;
				miTotalPoisonPrice = miPoisonCount * miPoisonPrice;
                _arrOfAllTextElement[2].text = miPoisonCount.ToString();
				_arrOfAllTextElement[3].text = miTotalPoisonPrice.ToString();
                _arrOfAllBtnElement[2].interactable = true;
                break;

            case "PoisonBuyBtn":
                //CEffectsPlayer.Instance.Play("BuyCoin");
				if (DataManager.GetTotalCoinAmount() >= miTotalPoisonPrice)
				{
                    DataManager.AddToPoisonAmount(miPoisonCount);
                    DataManager.SubstractFromTotalCoin(miTotalPoisonPrice);
                    _arrOfAllTextElement[9].text = DataManager.GetPoisonAmount().ToString();
				}

                else
                    UICanvasHandler.Instance.LoadScreen("CoinWarningCanvas");
                break;

            case "AirwingsDecBtn":
                //CEffectsPlayer.Instance.Play("PlusMinusSound");
				if (miAirwingCount > 1)
					miAirwingCount -= 1;

				miTotalAirwingPrice = miAirwingCount * miAirwingPrice;
                _arrOfAllTextElement[4].text = miAirwingCount.ToString();
				_arrOfAllTextElement[5].text = miTotalAirwingPrice.ToString();

                if (miAirwingCount == 1)
                    _arrOfAllBtnElement[4].interactable = false;
                break;

            case "AirwingsIncBtn":
                //CEffectsPlayer.Instance.Play("PlusMinusSound");
				miAirwingCount += 1;
				miTotalAirwingPrice = miAirwingCount * miAirwingPrice;
                _arrOfAllTextElement[4].text = miAirwingCount.ToString();
				_arrOfAllTextElement[5].text = miTotalAirwingPrice.ToString();
                _arrOfAllBtnElement[4].interactable = true;
                break;

            case "AirwingsBuyBtn":
                //CEffectsPlayer.Instance.Play("BuyCoin");
				if (DataManager.GetTotalCoinAmount() >= miTotalAirwingPrice)
				{
                    DataManager.AddToAirwingAmount(miAirwingCount);
                    DataManager.SubstractFromTotalCoin(miTotalAirwingPrice);
                    _arrOfAllTextElement[10].text = DataManager.GetAirwingAmount().ToString();
				}

                else
                    UICanvasHandler.Instance.LoadScreen("CoinWarningCanvas");
                break;

            case "MagnetDecBtn":
                //CEffectsPlayer.Instance.Play("PlusMinusSound");
				if (miMagnetCount > 1)
					miMagnetCount -= 1;

				miTotalMagnetPrice = miMagnetCount * miMagnetPrice;
                _arrOfAllTextElement[6].text = miMagnetCount.ToString();
				_arrOfAllTextElement[7].text = miTotalMagnetPrice.ToString();

                if (miMagnetCount == 1)
                    _arrOfAllBtnElement[6].interactable = false;
                break;

            case "MagnetIncBtn":
                //CEffectsPlayer.Instance.Play("PlusMinusSound");
				miMagnetCount += 1;
				miTotalMagnetPrice = miMagnetCount * miMagnetPrice;
                _arrOfAllTextElement[6].text = miMagnetCount.ToString();
				_arrOfAllTextElement[7].text = miTotalMagnetPrice.ToString();
                _arrOfAllBtnElement[6].interactable = true;
                break;

            case "MagnetBuyBtn":
                //CEffectsPlayer.Instance.Play("BuyCoin");
				if (DataManager.GetTotalCoinAmount() >= miTotalMagnetPrice)
				{
                    DataManager.AddToMagnetAmount(miMagnetCount);
                    DataManager.SubstractFromTotalCoin(miTotalMagnetPrice);
                    _arrOfAllTextElement[11].text = DataManager.GetMagnetAmount().ToString();
				}

                else
                    UICanvasHandler.Instance.LoadScreen("CoinWarningCanvas");
                break;
        }
    }
}
