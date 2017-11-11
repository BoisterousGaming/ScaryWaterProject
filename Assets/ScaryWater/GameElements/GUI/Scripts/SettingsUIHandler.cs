using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SettingsUIHandler : GUIItemsManager 
{
    static SettingsUIHandler mInstance;

    public bool _bLoadingFromGameOverScreen = false;

    public static SettingsUIHandler Instance
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
	}

    public override void OnBackButton()
    {
        OnClickBackBtn();
    }

    void OnClickBackBtn()
    {
        int tValue = DataManager.GetSettingsCanvasExitTarget();

        if (tValue == 0)
            LoadSelectedUICanvas("MainMenuCanvas");
        else if (tValue == 1)
            LoadSelectedUICanvas("PauseScreenCanvas");
        else if (tValue == 2)
            LoadSelectedUICanvas("GameOverCanvas");
    }

    void LoadSelectedUICanvas(string canvasName)
    {
        UICanvasHandler.Instance.DestroyScreen(this.gameObject);
        UICanvasHandler.Instance.LoadScreen(canvasName);
    }
}
