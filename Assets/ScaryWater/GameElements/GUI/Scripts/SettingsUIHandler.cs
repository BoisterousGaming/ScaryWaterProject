using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SettingsUIHandler : GUIItemsManager 
{
    static SettingsUIHandler mInstance;

    public Button _MusicBtn;
    public Button _SFXBtn;
    public TextMeshProUGUI _MusicBtnText;
    public TextMeshProUGUI _SFXBtnText;
    public Color _MusicBtnOnColor;
    public Color _SFXBtnOnColor;
    public Color _BtnOffColor;
    public Color _MusicBtnOnTextColor;
    public Color _SFXBtnOnTextColor;
    public Color _BtnOffTextColor;

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

    void Start()
    {
        SetMusicBtnColorState(CMusicPlayer.Instance.Mute);
        SetSFXBtnColorState(CEffectsPlayer.Instance.Mute);
    }

    public override void OnButtonCallBack(GUIItem item)
	{
		//Debug.Log("Button Pressed: " + item.gameObject.name);

        switch (item.gameObject.name)
        {
            case "MusicBtn":
                if (CMusicPlayer.Instance.Mute | CAmbientSFX.Instance.Mute | CEffectsPlayer.Instance.Mute)
                    SetMusicState(false);
                else
                    SetMusicState(true);
                break;

            case "SFXBtn":
                if (CEffectsPlayer.Instance.Mute | CAmbientSFX.Instance.Mute)
                    SetSFXState(false);
                else
                    SetSFXState(true);
                break;

            case "AdvGraphicBtn":
                break;

            case "GraphicBtn":
                break;

            case "StatBtn":
                break;

            case "GameInfoBtn":
                break;

            case "BackBtn":
                OnClickBackBtn();
                break;
        }
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


    public void SetMusicState(bool state)
    {
        CMusicPlayer.Instance.Mute = state;
        CAmbientSFX.Instance.Mute = state;
        CEffectsPlayer.Instance.Mute = state;
        SetSFXState(state);
        SetMusicBtnColorState(state);
    }

    public void SetSFXState(bool state)
    {
        CAmbientSFX.Instance.Mute = state;
        CEffectsPlayer.Instance.Mute = state;
        SetSFXBtnColorState(state);
    }

    void SetMusicBtnColorState(bool state)
    {
        if (state)
        {
            _MusicBtn.GetComponent<Image>().color = _BtnOffColor;
            _MusicBtnText.color = _BtnOffTextColor;
            _MusicBtnText.text = "Music : Off";
        }
        else
        {
            _MusicBtn.GetComponent<Image>().color = _MusicBtnOnColor;
            _MusicBtnText.color = _MusicBtnOnTextColor;
            _MusicBtnText.text = "Music : On";
        }
    }

    void SetSFXBtnColorState(bool state)
    {
        if (state)
        {
            _SFXBtn.GetComponent<Image>().color = _BtnOffColor;
            _SFXBtnText.color = _BtnOffTextColor;
            _SFXBtnText.text = "Sfx : Off";
        }
        else
        {
            _SFXBtn.GetComponent<Image>().color = _SFXBtnOnColor;
            _SFXBtnText.color = _SFXBtnOnTextColor;
            _SFXBtnText.text = "Sfx : On";
        }
    }
}
