using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinWarningUIHandler : GUIItemsManager 
{
    static CoinWarningUIHandler mInstance;

    public static CoinWarningUIHandler Instance
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

        CEffectsPlayer.Instance.Play("GeneralClick");

        switch (item.gameObject.name)
        {
            case "CloseBtn":
                UICanvasHandler.Instance.DestroyScreen(this.gameObject);
                break;
        }
	}
}
