using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoreTabUIHandler : GUIItemsManager
{
    bool mbSuccessState = false;
    static StoreTabUIHandler mInstance;

    public List<GUIItem> _listOfBtn = new List<GUIItem>();
    public List<Sprite> _listOfActiveBtnSprite = new List<Sprite>();
    public List<Sprite> _listOfDeactiveBtnSprite = new List<Sprite>();
    public Text _CoinCountText;
    public Text _ButterflyCountText;

    public static StoreTabUIHandler Instance
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
        UICanvasHandler.Instance.LoadScreen("StorePlayerSkinCanvas", null, true);  
        DisplayCoinAmount();
        DisplayButterflyAmount();
    }

    public override void OnButtonCallBack(GUIItem item)
    {
        //Debug.Log("Button Pressed: " + item.gameObject.name);

        //CEffectsPlayer.Instance.Play("GeneralClick");

        ChangeButtonImage(item);

        switch (item.gameObject.name)
        {
            case "PlayerSkinBtn":
                DestroyAndLoadCanvas("StorePlayerSkinCanvas");
                break;

            case "UpgradeBtn":
                DestroyAndLoadCanvas("StoreUpgradeCanvas");
                break;

            case "BoosterBtn":
                DestroyAndLoadCanvas("StoreBoosterCanvas");
                break;

            case "CoinBtn":
                DestroyAndLoadCanvas("StoreMoneyCanvas");
                break;

            case "BackBtn":
                OnPressBackBtn();
                break;
        }
    }

    bool mbChangingTab = false;
    void DestroyAndLoadCanvas(string canvasToLoad)
    {
        if (mbChangingTab)
            return;
        
        string tCanvasName = DataManager.GetActiveStoreTab();
        if (!tCanvasName.Equals(canvasToLoad))
        {
            mbChangingTab = true;
            GameObject tCanvas = UICanvasHandler.Instance.GetActiveCanvasByName(tCanvasName);
            if (tCanvas != null)
                UICanvasHandler.Instance.DestroyScreen(tCanvas);
            UICanvasHandler.Instance.LoadScreen(canvasToLoad, DidFinishLoadingTab);
        }
    }

    void DidFinishLoadingTab()
    {
        mbChangingTab = false;
    }


    void ChangeButtonImage(GUIItem btnItem)
    {
        for (int i = 0; i < _listOfBtn.Count; i++)
        {
            if (btnItem.gameObject.name == _listOfBtn[i].gameObject.name)
                btnItem.GetComponent<Image>().sprite = _listOfActiveBtnSprite[i];

            else
                _listOfBtn[i].GetComponent<Image>().sprite = _listOfDeactiveBtnSprite[i];
        }
    }

    public override void OnBackButton() 
    {
        OnPressBackBtn();
    }

    public void DisplayCoinAmount()
    {
        _CoinCountText.text = DataManager.GetTotalCoinAmount().ToString();
    }

    public void DisplayButterflyAmount()
    {
        _ButterflyCountText.text = DataManager.GetTotalButterflyAmount().ToString();
    }

    void OnPressBackBtn()
    {
        UICanvasHandler.Instance.RemoveAllActiveCanvas();

        if (SceneManager.GetActiveScene().buildIndex == 0)
            UICanvasHandler.Instance.LoadScreen("MainMenuCanvas", null, true);

        else
            UICanvasHandler.Instance.LoadScreen("GameOverCanvas", null, true);
    }
}
