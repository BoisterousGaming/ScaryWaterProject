using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class StoreMoneyUIHandler : GUIItemsManager 
{
    static StoreMoneyUIHandler mInstance;
    Dictionary<float, int> mDictOfCoinContent = new Dictionary<float, int>();
    Dictionary<float, int> mDictOfButterflyContent = new Dictionary<float, int>();
    List<int> mListOfCoinAmount = new List<int>();
    List<int> mListOfButterflyAmount = new List<int>();
    List<float> mListOfCoinPrice = new List<float>();
	List<float> mListOfButterflyPrice = new List<float>();

    public Text[] _arrOfCoinAmountText;
    public Text[] _arrOfCoinPriceText;
    public Text[] _arrOfButterflyAmountText;
    public Text[] _arrOfButterflyPriceText;

    public static StoreMoneyUIHandler Instance
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
        InitializeCoinData();
        InitializeButterflyData();
	}

    void InitializeCoinData()
    {
        mDictOfCoinContent.Add(0.99f, 1000);
        mDictOfCoinContent.Add(2.99f, 10000);
        mDictOfCoinContent.Add(4.99f, 100000);
        mDictOfCoinContent.Add(6.99f, 1000000);
        mDictOfCoinContent.Add(8.99f, 10000000);

        int tIndex = 0;
        foreach(KeyValuePair<float, int> element in mDictOfCoinContent)
        {
            _arrOfCoinAmountText[tIndex].text = element.Value.ToString() + " Coins";
            _arrOfCoinPriceText[tIndex].text = element.Key.ToString() + "$";
            tIndex += 1;
        }
    }

    void InitializeButterflyData()
    {
		mDictOfButterflyContent.Add(0.99f, 1000);
		mDictOfButterflyContent.Add(2.99f, 10000);
		mDictOfButterflyContent.Add(4.99f, 100000);
		mDictOfButterflyContent.Add(6.99f, 1000000);
		mDictOfButterflyContent.Add(8.99f, 10000000);

		int tIndex = 0;
		foreach (KeyValuePair<float, int> element in mDictOfButterflyContent)
		{
			_arrOfButterflyAmountText[tIndex].text = element.Value.ToString() + " Butterflies";
			_arrOfButterflyPriceText[tIndex].text = element.Key.ToString() + "$";
			tIndex += 1;
		}
    }

    public override void OnButtonCallBack(GUIItem item)
    {
        //Debug.Log("Button Pressed: " + item.gameObject.name);

        switch (item.gameObject.name)
        {
            case "Coin1KBuyBtn":
                AddCoin(0);             //cRamappa
                break;

			case "Coin10KBuyBtn":
                AddCoin(1);             //cRamappa
				break;

			case "Coin100KBuyBtn":
                AddCoin(2);             //cRamappa
				break;

			case "Coin1MBuyBtn":
                AddCoin(3);             //cRamappa
				break;

            case "Coin10MBuyBtn":
                AddCoin(4);             //cRamappa
				break;

			case "Butterfly1KBuyBtn":
                AddButterfly(0);        //cRamappa
				break;

			case "Butterfly10KBuyBtn":
                AddButterfly(1);        //cRamappa
				break;

			case "Butterfly100KBuyBtn":
                AddButterfly(2);        //cRamappa
				break;

			case "Butterfly1MBuyBtn":
                AddButterfly(3);        //cRamappa
				break;

			case "Butterfly10MBuyBtn":
                AddButterfly(4);        //cRamappa
				break;
        }
    }

    void AddCoin(int index)
    {
        //CEffectsPlayer.Instance.Play("BuyCash");
        DataManager.AddToTotalCoin(mDictOfCoinContent.Values.ElementAt(index));
    }

    void AddButterfly(int index)
    {
        //CEffectsPlayer.Instance.Play("BuyCash");
        DataManager.AddToTotalButterfly(mDictOfButterflyContent.Values.ElementAt(index));
    }
}
