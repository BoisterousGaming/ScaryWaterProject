using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScreenHandler 
{
	static UIScreenHandler mInstance;
	Hashtable mScreenTable = new Hashtable();

	public static UIScreenHandler Instance
	{
		get{
			if (mInstance == null) {
				mInstance = new UIScreenHandler ();
			}
			return mInstance;
		}
	}

	void Initialize()
	{
		mScreenTable = new Hashtable();
	}

	void RegisterUIScreen(UIScreen uiScreen)
	{
		if (!mScreenTable.Contains (uiScreen))
			mScreenTable.Add (uiScreen.name, uiScreen);
		else
			Debug.LogWarning ("A screen with the same name has already been registered: " + uiScreen.name);
	}

	void UnRegisterScreen(UIScreen uiScreen)
	{
		if (mScreenTable.Contains (uiScreen))
			mScreenTable.Remove (uiScreen.name);
		else
			Debug.LogWarning ("The screen has already been removed or was not registered before: " + uiScreen.name);
	}

	public UIScreen GetUIScreenByTag(string uiScreenTag)
	{
		UIScreen tReqScreen = null;
		if (mScreenTable.Contains (uiScreenTag))
			tReqScreen = mScreenTable [uiScreenTag] as UIScreen;

		return tReqScreen;
	}
}
