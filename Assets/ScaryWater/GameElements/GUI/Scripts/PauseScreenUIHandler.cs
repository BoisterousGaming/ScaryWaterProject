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

        switch (item.gameObject.name)
        {
            case "ResumeBtn":
                Time.timeScale = 1.0f;
                UICanvasHandler.Instance.DestroyScreen(this.gameObject);
                break;

            case "SettingBtn":
                break;

            case "ReplayBtn":
                Time.timeScale = 1.0f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;
        }
    }

    public override void OnBackButton()
    {
        Time.timeScale = 1.0f;
        UICanvasHandler.Instance.DestroyScreen(this.gameObject);
    }
}