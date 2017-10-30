using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class GUIItemButton : GUIItem 
{
	Action<GUIItem> mCallBack;
	Button mButton;
	Image mImage;

	void Awake () 
	{
		meItemType = eItemType.Button;
		mButton = GetComponent<Button>();
		mButton.onClick.AddListener(OnClickCallback);
	}

	//Callback when button is clicked
	void OnClickCallback()
	{
		if(mCallBack != null)
			mCallBack(this);
	}

	//Set callback function to be called when click happens
	public void SetCallback(Action<GUIItem> callBack)
	{
		mCallBack = callBack;
	}

	//Set if button is interactable
	public void SetInteractable(bool interactable)
	{
		mButton.interactable = interactable;
	}

	public override Color GetColor()
	{
		if(mImage == null)
			mImage = GetComponent<Image>();
		
		return mImage.color;
	}

	public override void SetColor(Color color)
	{
		if(mImage == null)
			mImage = GetComponent<Image>();

		mImage.color = color;
	}

	public override bool HasColorParameter()
	{
		return true;
	}

	public void OnButtonClicked()
	{
		if(mButton.interactable)
			transform.localScale = new Vector3 (1.1f, 1.1f, 1.1f);
	}

	public void OnButtonReleased()
	{
		transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
	}

}


