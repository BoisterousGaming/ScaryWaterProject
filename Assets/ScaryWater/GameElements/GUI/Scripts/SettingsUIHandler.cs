using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsUIHandler : GUIItemsManager 
{
    static SettingsUIHandler mInstance;

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
}
