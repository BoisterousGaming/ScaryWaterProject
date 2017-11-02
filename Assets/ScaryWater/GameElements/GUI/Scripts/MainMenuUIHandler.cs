using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIHandler : GUIItemsManager 
{
    static MainMenuUIHandler mInstance;

    public ChallengeCommenceTimer _ChallengeCommenceTimerScr;

	public static MainMenuUIHandler Instance
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

    public override void OnButtonCallBack(GUIItem item)
	{
		//Debug.Log("Button Pressed: " + item.gameObject.name);

        switch (item.name)
        {
            case "PlayBtn":
                UICanvasHandler.Instance.DestroyScreen(this.gameObject);
                UICanvasHandler.Instance.LoadScreen("EnvironmentSelectionCanvas", null, true);
                break;

            case "ChallengeBtn":
                if (DataManager.GetAllEnvPurchasedState() & _ChallengeCommenceTimerScr._bEnableChallenge)
                    SceneManager.LoadScene(2, LoadSceneMode.Single);

                else if (!DataManager.GetAllEnvPurchasedState())
                {
                    UICanvasHandler.Instance.DestroyScreen(this.gameObject);
                    UICanvasHandler.Instance.LoadScreen("EnvironmentSelectionCanvas");
                }
                break;

            case "SettingsBtn":
				break;

            case "FacebookBtn":
				break;

            case "StoreBtn":
				UICanvasHandler.Instance.DestroyScreen(this.gameObject);
				UICanvasHandler.Instance.LoadScreen("StoreTabCanvas", null, true);
				break;

            case "ClearPrefsBtn":
                PlayerPrefs.DeleteAll();
                break;
        }
	}
}
