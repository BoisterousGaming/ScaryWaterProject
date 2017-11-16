using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ePlayerSkinID
{
	Skin_1 = 0,
	Skin_2,
	Skin_3,
	Skin_4,
	Skin_5,
	Skin_6,
	Skin_7,
	Skin_8,
	Skin_9,
	Skin_10,
	Skin_11,
	Skin_12,
	Skin_13,
	Skin_14,
	Skin_15,
	Skin_16,
	Skin_17,
	Skin_18,
	Skin_19,
	Skin_20,
	Skin_21,
	Skin_22,
	Skin_23,
	Skin_24,
	Skin_25,
	Skin_26,
	Skin_27,
	Skin_28,
	Skin_29,
	Skin_30
}

public enum ePlayerSkinLockState
{
	Locked = 0,
	Unlocked
}

public enum ePlayerSkinPurchaseState
{
	No = 0,
	Yes
}

public enum ePlayerSkinEquipState
{
	No = 0,
	Yes
}

public class StorePlayerSkinUIHandler : GUIItemsManager
{
    static StorePlayerSkinUIHandler mInstance;
    GameObject mDemoPlayer;
    string msRegularTextColor = "#FFFFFFFF";
    string msEquipedTextColor = "#2D8199FF";

    public int _SelectedSkinID;
    public Dictionary<ePlayerSkinID, int> _dictOfSkinPrice = new Dictionary<ePlayerSkinID, int>();
    public List<SkinDataScr> _listOfSkins = new List<SkinDataScr>();
    public Text _SkinInfo;
    public Text _SkinCost;
    public Image _CoinImg;
    public Image _SelectedSkinBorderImg;
    public Button _BuyBtn;
    public Button _EquipBtn;
    public DemoPlayerScr _DemoPlayerScr;

    public static StorePlayerSkinUIHandler Instance
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
        Color tColor = new Color();
        ColorUtility.TryParseHtmlString(msEquipedTextColor, out tColor);
        SkinAndBtnState(false, false, "Skin is equipped", tColor, " ", false);
        InitializeSkinPrice();
	}

    void OnDestroy()
    {
        Destroy(mDemoPlayer);    
    }

    void InitializeSkinPrice()
    {
		_dictOfSkinPrice.Add(ePlayerSkinID.Skin_1, 100);
		_dictOfSkinPrice.Add(ePlayerSkinID.Skin_2, 200);
		_dictOfSkinPrice.Add(ePlayerSkinID.Skin_3, 150);
		_dictOfSkinPrice.Add(ePlayerSkinID.Skin_4, 100);
		_dictOfSkinPrice.Add(ePlayerSkinID.Skin_5, 250);
		_dictOfSkinPrice.Add(ePlayerSkinID.Skin_6, 100);
		_dictOfSkinPrice.Add(ePlayerSkinID.Skin_7, 100);
		_dictOfSkinPrice.Add(ePlayerSkinID.Skin_8, 100);
		_dictOfSkinPrice.Add(ePlayerSkinID.Skin_9, 100);
		_dictOfSkinPrice.Add(ePlayerSkinID.Skin_10, 100);
		_dictOfSkinPrice.Add(ePlayerSkinID.Skin_11, 100);
		_dictOfSkinPrice.Add(ePlayerSkinID.Skin_12, 100);
		_dictOfSkinPrice.Add(ePlayerSkinID.Skin_13, 100);
		_dictOfSkinPrice.Add(ePlayerSkinID.Skin_14, 100);
		_dictOfSkinPrice.Add(ePlayerSkinID.Skin_15, 100);
		_dictOfSkinPrice.Add(ePlayerSkinID.Skin_16, 100);
		_dictOfSkinPrice.Add(ePlayerSkinID.Skin_17, 100);
		_dictOfSkinPrice.Add(ePlayerSkinID.Skin_18, 100);
		_dictOfSkinPrice.Add(ePlayerSkinID.Skin_19, 100);
		_dictOfSkinPrice.Add(ePlayerSkinID.Skin_20, 100);
		_dictOfSkinPrice.Add(ePlayerSkinID.Skin_21, 100);
		_dictOfSkinPrice.Add(ePlayerSkinID.Skin_22, 100);
		_dictOfSkinPrice.Add(ePlayerSkinID.Skin_23, 100);
		_dictOfSkinPrice.Add(ePlayerSkinID.Skin_24, 100);
		_dictOfSkinPrice.Add(ePlayerSkinID.Skin_25, 100);
		_dictOfSkinPrice.Add(ePlayerSkinID.Skin_26, 100);
		_dictOfSkinPrice.Add(ePlayerSkinID.Skin_27, 100);
		_dictOfSkinPrice.Add(ePlayerSkinID.Skin_28, 100);
		_dictOfSkinPrice.Add(ePlayerSkinID.Skin_29, 100);
		_dictOfSkinPrice.Add(ePlayerSkinID.Skin_30, 100);
    }

    public void OnUnlockingSkin(ePlayerSkinID skinID)
    {
        DataManager.SetSkinStates(skinID.ToString(), 1, 0, 0);
    }

	public void OnPurchaseFinished()
	{
        for (int i = 0; i < _listOfSkins.Count; i++)
        {
            SkinDataScr tSkinDataScr = _listOfSkins[i];

            if (_SelectedSkinID == (int)tSkinDataScr._ePlayerSkinID)
            {
                DataManager.SetSkinStates(tSkinDataScr._ePlayerSkinID.ToString(), 1, 1, 0);
                Color tColor = new Color();
                ColorUtility.TryParseHtmlString(msRegularTextColor, out tColor);
                SkinAndBtnState(false, true, "Skin is not equipped", tColor, " ", false);
            }     
        }
	}

    void OnClickingPurchaseSkin()
    {
        UICanvasHandler.Instance.LoadScreen("PurchaseSkinCanvas", null, true);
    }

	public void OnClickingEquipSkin()
	{
		for (int i = 0; i < _listOfSkins.Count; i++)
		{
			SkinDataScr tSkinDataScr = _listOfSkins[i];

            if (tSkinDataScr._iSkinID == _SelectedSkinID)
            {
                DataManager.SetSkinStates(tSkinDataScr._ePlayerSkinID.ToString(), 1, 1, 1);
                DataManager.SetEquipedSkinID(_SelectedSkinID);
                Color tColor = new Color();
                ColorUtility.TryParseHtmlString(msEquipedTextColor, out tColor);
                SkinAndBtnState(false, false, "Skin is equipped", tColor, " ", false);
            }

            else
            {
                SkinStates tSkinStates = DataManager.GetSkinStates(tSkinDataScr._ePlayerSkinID.ToString());
                DataManager.SetSkinStates(tSkinDataScr._ePlayerSkinID.ToString(), tSkinStates._iSkinLockState, tSkinStates._iSkinPurchaseState, 0);
            }
		}
	}

    public override void OnButtonCallBack(GUIItem item)
    {
		//Debug.Log("Button Pressed: " + item.gameObject.name);
		SkinDataScr tSkinData = item.GetComponent<SkinDataScr>();

		if (tSkinData != null)
		{
            //CEffectsPlayer.Instance.Play("SkinTapEquip");
            _SelectedSkinBorderImg.transform.SetParent(tSkinData.transform);
            _SelectedSkinBorderImg.GetComponent<RectTransform>().position = tSkinData.transform.GetChild(0).GetComponent<RectTransform>().position;
			SkinDataHandler(tSkinData);
			_SelectedSkinID = tSkinData._iSkinID;
            _DemoPlayerScr.EquipSkin(_SelectedSkinID, true);
		}

        switch (item.gameObject.name)
        { 
            case "BuyBtn":
                //CEffectsPlayer.Instance.Play("BuyCoin");
                foreach(SkinDataScr element in _listOfSkins)
                {
                    if (element._iSkinID == _SelectedSkinID)
                    {
                        SkinStates tSkinStates = DataManager.GetSkinStates(element._ePlayerSkinID.ToString());
                        int isUnlocked = tSkinStates._iSkinLockState;
						if (isUnlocked == 1)
						{
							int isPurchased = tSkinStates._iSkinPurchaseState;
							if (isPurchased == 1)
							{
                                // Do nothing
							}

							else
    							OnClickingPurchaseSkin();
						}
                        else
                        {
						    // Do nothing
						}
                    }
                }
                break;

            case "EquipBtn":
                //CEffectsPlayer.Instance.Play("SkinTapEquip");
                foreach(SkinDataScr element in _listOfSkins)
                {
                    if (element._iSkinID == _SelectedSkinID)
                    {
                        SkinStates tSkinStates = DataManager.GetSkinStates(element._ePlayerSkinID.ToString());
                        int isEquiped = tSkinStates._iSkinEquipState;
                        if (isEquiped == 1)
                        {
                            // Do nothing 
                        }

                        else if (isEquiped == 0)
                            OnClickingEquipSkin();
                    }
                }
                break;
        }
    }

    void SkinDataHandler(SkinDataScr skinData)
    {
        SkinStates tSkinStates = DataManager.GetSkinStates(skinData._ePlayerSkinID.ToString());
        int isUnlocked = tSkinStates._iSkinLockState;
        if (isUnlocked == 1)
        {
            int isPurchased = tSkinStates._iSkinPurchaseState;
            if (isPurchased == 1)
            {
                int isEquiped = tSkinStates._iSkinEquipState;
                if (isEquiped == 1)
                {
                    Color tColor = new Color();
                    ColorUtility.TryParseHtmlString(msEquipedTextColor, out tColor);
                    SkinAndBtnState(false, false, "Skin is equipped", tColor, " ", false);
                }

                else
                {
                    Color tColor = new Color();
                    ColorUtility.TryParseHtmlString(msRegularTextColor, out tColor);
                    SkinAndBtnState(false, true, "Skin is not equipped", tColor, " ", false);
                }
            }

            else
            {
                foreach (KeyValuePair<ePlayerSkinID, int> element in _dictOfSkinPrice)
                {
                    if (element.Key.Equals(skinData._ePlayerSkinID))
                    {
                        Color tColor = new Color();
                        ColorUtility.TryParseHtmlString(msRegularTextColor, out tColor);
                        SkinAndBtnState(true, false, "Purchase the skin", tColor, element.Value.ToString(), true);
                    }     
                }
            }
        }

        else
        {
            Color tColor = new Color();
            ColorUtility.TryParseHtmlString(msRegularTextColor, out tColor);
            SkinAndBtnState(false, false, "Skin is locked", tColor, " ", false);
        }
	}

    void SkinAndBtnState(bool buyBtnState, bool equipBtnState, string skinInfo, Color skininfoColor, string skinCost, bool coinImageState)
    {
        _BuyBtn.gameObject.SetActive(buyBtnState);
        _EquipBtn.gameObject.SetActive(equipBtnState);
        _SkinInfo.text = skinInfo;
        _SkinInfo.color = skininfoColor;
        _SkinCost.text = skinCost;
        _CoinImg.GetComponent<Image>().enabled = coinImageState;
    }
}
