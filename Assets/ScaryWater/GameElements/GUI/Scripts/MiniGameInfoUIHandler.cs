using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameInfoUIHandler : GUIItemsManager 
{
    static MiniGameInfoUIHandler mInstance;

    public Text _miniGameInfoText;

    public static MiniGameInfoUIHandler Instance
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
}
