using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class GUIItemsManager : MonoBehaviour 
{
	public GUIItem[] _AnimatingItems = new GUIItem[0];
	public string _BackToScreen = "";

	protected Action mAnimationCallBack;
	protected int miCallBackReceived = 0;
	protected int miRequiredCallback = 0;
	protected bool mbIsAnimating;
	protected Animator mAnimator;

	//Set all button call backs
	public void Init()
	{
		GUIItemButton[] tArrButtons = GetComponentsInChildren<GUIItemButton> ();
		for (int i = 0; i < tArrButtons.Length; i++) {
			tArrButtons [i].SetCallback (OnButtonCallBack);
		}

		GUIToggleButton[] tArrToggleButtons = GetComponentsInChildren<GUIToggleButton> ();
		for (int i = 0; i < tArrToggleButtons.Length; i++) {
			tArrToggleButtons [i].SetCallback (OnButtonCallBack);
		}

		mAnimator = GetComponent<Animator> ();
	}

	public void TriggerAnimatorParam(string trigger)
	{
		if (mAnimator != null) {
			mAnimator.SetTrigger (trigger);
		} else {
			Debug.LogWarning ("Animator not found in this canvas: " + gameObject.name);
		}
	}

	//Called once an item finishes entry animation
	public void OnItemEnterComplete()
	{
		miCallBackReceived++;
		if(miCallBackReceived >= miRequiredCallback)
		{
			mbIsAnimating = false;
			OnEntryAnimationCompleted();
			if(mAnimationCallBack != null)
				mAnimationCallBack();
		}
	}
		
	//Called once an item finishes exit animation
	public void OnItemExitComplete()
	{
		miCallBackReceived++;
		if(miCallBackReceived >= miRequiredCallback)
		{
			mbIsAnimating = false;
			OnExitAnimationCompleted();
			if(mAnimationCallBack != null)
				mAnimationCallBack();
		}
	}

	//Start the entry animation
	public void BeginEntryAnimation(Action callBack)
	{
		mAnimationCallBack = callBack;
		miRequiredCallback = 0;
		miCallBackReceived = 0;
		mbIsAnimating = true;

		for(int i = 0; i < _AnimatingItems.Length ; i++)
		{
			if(_AnimatingItems[i].BeginEntryAnimation(OnItemEnterComplete))
				miRequiredCallback++;
		}

		//No animating objects
		if(miRequiredCallback == 0)
			OnItemEnterComplete();
	}

	//start the exit animation
	public void BeginExitAnimation(Action callBack)
	{
		mAnimationCallBack = callBack;
		miRequiredCallback = 0;
		miCallBackReceived = 0;
		mbIsAnimating = true;

		for(int i = 0; i < _AnimatingItems.Length ; i++)
		{
			if(_AnimatingItems[i].BeginExitAnimation(OnItemExitComplete))
				miRequiredCallback++;
		}

		//No animating objects
		if(miRequiredCallback == 0)
			OnItemExitComplete();
	}

	//Stop all animations
	void StopAllActions()
	{
		mAnimationCallBack = null;
		miRequiredCallback = 0;
		miCallBackReceived = 0;
		mbIsAnimating = false;

		for(int i = 0; i < _AnimatingItems.Length ; i++)
		{
			_AnimatingItems[i].StopAllActions();
		}
	}

	void LateUpdate()
	{
		if (!mbIsAnimating)
			checkUserButtonInput();
	}

	void checkUserButtonInput()
	{
		if (Input.GetKeyDown(KeyCode.Escape)) {
			OnBackButton ();
		}
		else if (Input.anyKeyDown) 
        {
			OnKeyPressed();
        }
	}

	//Called once all Entry animations have been completed
	public virtual void OnEntryAnimationCompleted()	{}

	//Called once all Exit animations have been completed
	public virtual void OnExitAnimationCompleted() {}

	//Called if any key has been pressed except for the escape key
	public virtual void OnKeyPressed() {}

	//Called if escape key has been pressed
	public virtual void OnBackButton() {}

	//Called by default when a gui button is clicked
	public virtual void OnButtonCallBack(GUIItem item)
	{
		Debug.Log("Button Pressed: "+item.gameObject.name);
	}
}
