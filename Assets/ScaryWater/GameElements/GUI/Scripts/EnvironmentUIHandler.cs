using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum eEnvID
{
    Env_1 = 0,
    Env_2,
    Env_3
}

public enum eEnvLockState
{
    Locked = 0,
    Unlocked
}

public enum eEnvPurchasedState
{
    No = 0,
    Yes
}

public enum eEnvActiveState
{
    No = 0,
    Yes
}

public class EnvironmentUIHandler : GUIItemsManager
{
    int miCurrentIndex;
    int miSelectedLevelToLoad;
    static EnvironmentUIHandler mInstance;
    Dictionary<eEnvID, float> mDictOfEnvPrice = new Dictionary<eEnvID, float>();

    public List<Image> _listOfEnvironmentPreview = new List<Image>();
    public List<string> _listOfEnvName = new List<string>();
    public List<string> _listOfEnvInfo = new List<string>();
    public Button _playBtn;
    public Button _buyBtn;
    public Button _leftArrowButton;
    public Button _rightArrowButton;
    public Text _EnvNameText;
    public Text _EnvInfoText;
    public Text _EnvPriceText;
    public Image _CoinImage;
    public int _iCurrentEnvID = 0;
    public string _sCurrentEnvName;

    public static EnvironmentUIHandler Instance
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
        int tActiveID;
        if (DataManager.GetNonPurchasedEnvIDCheckState() == 1)
            tActiveID = DataManager.GetNonPurchasedEnvID();
        
        else
            tActiveID = DataManager.GetActiveEnvID();
        
        miCurrentIndex = tActiveID;
        EnvPriceInitialize();
        EnvironmentSetup();
        DataManager.SetNonPurchasedEnvIDCheckState(0);
    }

    void Update()
    {
        ArrowBtnIntractabilityState();    
    }

    public override void OnButtonCallBack(GUIItem item)
    {
        //Debug.Log("Button Pressed: " + item.gameObject.name);

        switch (item.gameObject.name)
        {
            case "LeftBtn":
                if (miCurrentIndex > 0)
                {
                    miCurrentIndex -= 1;
                    EnvironmentSetup(); 
                }
                break;

            case "RightBtn":
                if (miCurrentIndex < _listOfEnvironmentPreview.Count - 1)
                {
                    miCurrentIndex += 1;
                    EnvironmentSetup();
                }
                break;

            case "PlayBtn":
                DataManager.SetActiveEnvID(_iCurrentEnvID);
                miSelectedLevelToLoad = _iCurrentEnvID;
                int tSceneIndex = ++miSelectedLevelToLoad;
                SceneManager.LoadScene(tSceneIndex, LoadSceneMode.Single);
                break;

            case "BuyBtn":
                foreach (KeyValuePair<eEnvID, float> element in mDictOfEnvPrice)
                {
                    if ((int)element.Key == _iCurrentEnvID)
                    {
                        int tValue = (int)Mathf.Round(element.Value);
                        if (DataManager.GetTotalCoinAmount() >= tValue)
                        {
                            DataManager.SetEnvStates(_sCurrentEnvName, 1, 1);
                            DataManager.SubstractFromTotalCoin(tValue);
                            EnvBtnState(true, false, false, " ");
                        }

                        else
                            UICanvasHandler.Instance.LoadScreen("CoinWarningCanvas", null, true);

                        break;
                    }
                }
                break;
        }
    }

	void EnvironmentSetup()
	{
        for (int i = 0; i < _listOfEnvironmentPreview.Count; i++)
        {
            if (miCurrentIndex == i)
            {
                _listOfEnvironmentPreview[i].gameObject.SetActive(true);
                _EnvNameText.text = _listOfEnvName[i];
                _EnvInfoText.text = _listOfEnvInfo[i];
            }

            else
                _listOfEnvironmentPreview[i].gameObject.SetActive(false);
        }

        EnvStates tEnvStates = DataManager.GetEnvStates(_sCurrentEnvName);

        if (tEnvStates._iEnvLockState == 1 & tEnvStates._iEnvPurchaseState == 0)
        {
            foreach (KeyValuePair<eEnvID, float> element in mDictOfEnvPrice)
            {
                if ((int)element.Key == _iCurrentEnvID)
                {
                    EnvBtnState(false, true, true, element.Value.ToString());
                    break;
                }
            }
        }

        if (tEnvStates._iEnvLockState == 1 & tEnvStates._iEnvPurchaseState == 1)
            EnvBtnState(true, false, false, " ");
	}

    void EnvBtnState(bool playBtnState, bool buyBtnState, bool coinImageState, string envPriceText)
    {
        _playBtn.gameObject.SetActive(playBtnState);
        _buyBtn.gameObject.SetActive(buyBtnState);
        _CoinImage.enabled = coinImageState;
        _EnvPriceText.text = envPriceText;
    }

    void ArrowBtnIntractabilityState()
    {
        if (miCurrentIndex > 0)
            _leftArrowButton.interactable = true;

        else
            _leftArrowButton.interactable = false;

        if (miCurrentIndex < _listOfEnvironmentPreview.Count - 1)
            _rightArrowButton.interactable = true;

        else
            _rightArrowButton.interactable = false;
    }

    void EnvPriceInitialize()
    {
        mDictOfEnvPrice.Add(eEnvID.Env_1, 100000f);
        mDictOfEnvPrice.Add(eEnvID.Env_2, 2500000f);
        mDictOfEnvPrice.Add(eEnvID.Env_3, 5000000f);
    }

    public override void OnBackButton()
    {
        UICanvasHandler.Instance.DestroyScreen(this.gameObject);
        UICanvasHandler.Instance.LoadScreen("MainMenuCanvas");
    }
}