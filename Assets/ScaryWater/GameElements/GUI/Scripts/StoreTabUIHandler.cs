using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoreTabUIHandler : GUIItemsManager
{
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
        ChangeButtonImage(item);

        switch (item.gameObject.name)
        {
            case "PlayerSkinBtn":
                for (int i = 0; i < UICanvasHandler.Instance._ActiveCanvas.Count; i++)
                {
                    GameObject tCanvas = UICanvasHandler.Instance._ActiveCanvas[i];

                    if (tCanvas.name.Equals("StoreUpgradeCanvas") | tCanvas.name.Equals("StoreBoosterCanvas") | tCanvas.name.Equals("StoreMoneyCanvas"))
                        UICanvasHandler.Instance.DestroyScreen(tCanvas);
                }

                UICanvasHandler.Instance.LoadScreen("StorePlayerSkinCanvas", null, true);
                break;

            case "UpgradeBtn":
				for (int i = 0; i < UICanvasHandler.Instance._ActiveCanvas.Count; i++)
				{
					GameObject tCanvas = UICanvasHandler.Instance._ActiveCanvas[i];

                    if (tCanvas.name.Equals("StorePlayerSkinCanvas") | tCanvas.name.Equals("StoreBoosterCanvas") | tCanvas.name.Equals("StoreMoneyCanvas"))
                        UICanvasHandler.Instance.DestroyScreen(tCanvas);
				}

				UICanvasHandler.Instance.LoadScreen("StoreUpgradeCanvas", null, true);
                break;

            case "BoosterBtn":
				for (int i = 0; i < UICanvasHandler.Instance._ActiveCanvas.Count; i++)
				{
					GameObject tCanvas = UICanvasHandler.Instance._ActiveCanvas[i];

					if (tCanvas.name.Equals("StorePlayerSkinCanvas") | tCanvas.name.Equals("StoreUpgradeCanvas") | tCanvas.name.Equals("StoreMoneyCanvas"))
						UICanvasHandler.Instance.DestroyScreen(tCanvas);
				}

				UICanvasHandler.Instance.LoadScreen("StoreBoosterCanvas", null, true);
                break;

            case "CoinBtn":
				for (int i = 0; i < UICanvasHandler.Instance._ActiveCanvas.Count; i++)
				{
					GameObject tCanvas = UICanvasHandler.Instance._ActiveCanvas[i];

					if (tCanvas.name.Equals("StorePlayerSkinCanvas") | tCanvas.name.Equals("StoreUpgradeCanvas") | tCanvas.name.Equals("StoreBoosterCanvas"))
						UICanvasHandler.Instance.DestroyScreen(tCanvas);
				}

				UICanvasHandler.Instance.LoadScreen("StoreMoneyCanvas", null, true);
                break;
        }
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
        UICanvasHandler.Instance.RemoveAllActiveCanvas();

        if (SceneManager.GetActiveScene().buildIndex == 0)
            UICanvasHandler.Instance.LoadScreen("MainMenuCanvas", null, true);

        else
            UICanvasHandler.Instance.LoadScreen("GameOverCanvas", null, true);
    }

    public void DisplayCoinAmount()
    {
        _CoinCountText.text = DataManager.GetTotalCoinAmount().ToString();
    }

    public void DisplayButterflyAmount()
    {
        _ButterflyCountText.text = DataManager.GetTotalButterflyAmount().ToString();
    }
}
