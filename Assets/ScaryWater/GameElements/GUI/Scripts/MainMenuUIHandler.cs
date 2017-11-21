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

        if (mbIsAnimating)
            return;

        //CEffectsPlayer.Instance.Play("GeneralClick");

        switch (item.name)
        {
            case "PlayBtn":
                UICanvasHandler.Instance.DestroyScreen(this.gameObject);
                UICanvasHandler.Instance.LoadScreen("EnvironmentSelectionCanvas");
                break;

            case "ChallengeBtn":
                if (DataManager.GetAllEnvPurchasedState() & _ChallengeCommenceTimerScr._bEnableChallenge)
                    SceneManager.LoadScene("Challenge_GamePlay_01", LoadSceneMode.Single);

                else if (!DataManager.GetAllEnvPurchasedState())
                {
                    DataManager.SetNonPurchasedEnvIDCheckState(1);
                    UICanvasHandler.Instance.DestroyScreen(this.gameObject);
                    UICanvasHandler.Instance.LoadScreen("EnvironmentSelectionCanvas");
                }
                break;

            case "SettingsBtn":
                DataManager.SetSettingsCanvasExitTarget(0);
                UICanvasHandler.Instance.DestroyScreen(this.gameObject);
                UICanvasHandler.Instance.LoadScreen("SettingsCanvas");
				break;

            case "FacebookBtn":
				break;

            case "StoreBtn":
				UICanvasHandler.Instance.DestroyScreen(this.gameObject);
				UICanvasHandler.Instance.LoadScreen("StoreTabCanvas");
				break;

            case "ClearPrefsBtn":
                PlayerPrefs.DeleteAll();
                break;
        }
	}
}
