using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MiniGameChoiceUIHandler : GUIItemsManager 
{
	static MiniGameChoiceUIHandler mInstance;

    public TextMeshProUGUI _InfoText;

	public static MiniGameChoiceUIHandler Instance
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
        Time.timeScale = 0f;    
    }

    public override void OnButtonCallBack(GUIItem item)
	{
		//Debug.Log("Button Pressed: " + item.gameObject.name);

        switch (item.gameObject.name)
        {
            case "YesBtn":
                //CEffectsPlayer.Instance.Play("PostitiveSound");
                Time.timeScale = 1f;
                MiniGameManager.Instance.ActivateMiniGame(Time.time);
                DestroyCanvas();
                break;

            case "NoBtn":
                //CEffectsPlayer.Instance.Play("NegativeSound");
				Time.timeScale = 1f;
                DestroyCanvas();
                break;
        }
	}

    void DestroyCanvas()
    {
        UICanvasHandler.Instance.DestroyScreen(this.gameObject);
        PlayerManager.Instance._playerHandler._eControlState = eControlState.Active;
    }
}
