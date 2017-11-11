using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreenUIHandler : GUIItemsManager
{
    static PauseScreenUIHandler mInstance;

    public static PauseScreenUIHandler Instance
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
        Time.timeScale = 0.0f;
    }

    public override void OnButtonCallBack(GUIItem item)
    {
        //Debug.Log("Button Pressed: " + item.gameObject.name);

        //CEffectsPlayer.Instance.Play("GeneralClick");

        switch (item.gameObject.name)
        {
            case "ResumeBtn":
                OnPressBackBtn();
                break;

            case "SettingBtn":
                DataManager.SetSettingsCanvasExitTarget(1);
                UICanvasHandler.Instance.DestroyScreen(this.gameObject);
                UICanvasHandler.Instance.LoadScreen("SettingsCanvas");
                break;

            case "ReplayBtn":
                Time.timeScale = 1.0f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;

            case "HomeBtn":
                Time.timeScale = 1.0f;
                SceneManager.LoadScene(0, LoadSceneMode.Single);
                break;
        }
    }

    public override void OnBackButton()
    {
        OnPressBackBtn();
    }

    void OnPressBackBtn()
    {
        Time.timeScale = 1.0f;
        UICanvasHandler.Instance.DestroyScreen(this.gameObject);
    }
}