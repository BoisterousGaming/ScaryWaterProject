using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameRewardUIHandler : GUIItemsManager 
{
	static MiniGameRewardUIHandler mInstance;

	public Text _InfoText;

	public static MiniGameRewardUIHandler Instance
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
			case "OkBtn":
                Time.timeScale = 1f;
                UICanvasHandler.Instance.DestroyScreen(this.gameObject);
                MiniGameManager.Instance.GiveRewardToPlayer();
                PlayerManager.Instance._playerHandler._eControlState = eControlState.Active;
				break;
		}
	}
}
