using UnityEngine;
using System;
using System.Collections;

public enum ePanelState
{
	Settled,
	Animating,
}

public class GUIItemPanel : MonoBehaviour 
{
	public GUIItem[] _Items = new GUIItem[0];
	Action mAnimationCallBack;

	int miCallBackReceived = 0;
	int miRequiredCallback = 0;

	//Animate the panel first and then animate the items if required
	public void ShowPanel(Action callBack)
	{
		mAnimationCallBack = callBack;
		GetComponent<MenuItemEntryAnim>().AnimationWithCallBack(BeginItemsEntryAnimation);
	}

	//Animate the items first and then animate the panel
	public void HidePanel(Action callBack)
	{
		mAnimationCallBack = callBack;
		BeginPanelExitAnimation();
	}

	void BeginItemsEntryAnimation()
	{
		miRequiredCallback = 0;
		for(int i = 0; i < _Items.Length ; i++)
		{
			if(_Items[i].BeginEntryAnimation(OnItemEnterComplete))
				miRequiredCallback++;
		}
	}

	void BeginPanelExitAnimation()
	{
		miRequiredCallback = 0;
		for(int i = 0; i < _Items.Length ; i++)
		{
			if(_Items[i].BeginExitAnimation(OnItemExitComplete))
				miRequiredCallback++;
		}
	}

	//Once all items have finished enter animation
	void OnItemEnterComplete()
	{
		miCallBackReceived++;
		if(miCallBackReceived >= _Items.Length)
		{
			if(mAnimationCallBack != null)
				mAnimationCallBack();
		}
	}

	//Once all items have finished exit animation
	void OnItemExitComplete()
	{
		miCallBackReceived++;
		if(miCallBackReceived >= _Items.Length)
		{
			GetComponent<MenuItemEntryAnim>().AnimationWithCallBack(mAnimationCallBack);
		}
	}
}
