using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUIHandler : GUIItemsManager 
{
	int miCoinCount;
	int miButterflyCount;
    static GameOverUIHandler mInstance;

	public Text _CoinCount;
	public Text _ButterflyCount;
    public Text _CurrentScore;
    public Text _HighScore;
    public Text _NotificationText;
	
    public static GameOverUIHandler Instance
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
        if (_CoinCount != null)
            _CoinCount.text = DataManager.GetCSessionCoinAmount().ToString();

        if (_ButterflyCount != null)
            _ButterflyCount.text = DataManager.GetCSessionButterflyAmount().ToString();

        if (_CurrentScore != null)
            _CurrentScore.text = DataManager.GetCSessionScore().ToString();

        if (_HighScore != null)
            _HighScore.text = DataManager.GetHighScore().ToString();
    }

    public void GetNotification(string notificationString)
    {
        _NotificationText.text = notificationString;
    }

    public override void OnButtonCallBack(GUIItem item)
    {
        //Debug.Log("Button Pressed: " + item.gameObject.name);

        switch (item.gameObject.name)
        {
            case "ReplayBtn":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;

            case "SettingsBtn":
                break;

            case "HomeBtn":
                SceneManager.LoadScene(0, LoadSceneMode.Single);
                break;

            case "StoreBtn":
                UICanvasHandler.Instance.DestroyScreen(this.gameObject);
                UICanvasHandler.Instance.LoadScreen("StoreTabCanvas", null, true);
                break;
        }
    }

	public override void OnBackButton()
	{
		SceneManager.LoadScene(0, LoadSceneMode.Single);
	}
}
