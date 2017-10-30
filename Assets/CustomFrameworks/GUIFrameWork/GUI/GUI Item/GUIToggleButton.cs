using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

[System.Serializable]
public class GUIToggleButtonData
{
	public Sprite _Sprite;
	public string _StateName = "Default";
}

public class GUIToggleButton : GUIItem 
{
	Action<GUIItem> mCallBack;
	public GUIToggleButtonData[] _ToggleData;
	public GUIItemButton _NextButton;
	public GUIItemButton _PreviousButton;
	public string _StorageName = null;
	public bool _Cyclic;

	Image mImage;
	string mCurState;
	int miCurrentStateIndex;

	void Awake()
	{
		meItemType = eItemType.Toggle;
		Button tButton = GetComponent<Button>();
		tButton.onClick.AddListener(OnClickCallback);
	}

	// Use this for initialization
	void Start () 
	{
		mCurState = "Default";
		miCurrentStateIndex = 0;
		if(_StorageName != "")
			miCurrentStateIndex = PlayerPrefs.GetInt(_StorageName,0);
		mImage = GetComponent<Image>();
	
		SetState(miCurrentStateIndex);
	}

	//Set callback function to be called when click happens
	public void SetCallback(Action<GUIItem> callBack)
	{
		mCallBack = callBack;
	}

	//Callback when button is clicked
	void OnClickCallback()
	{
		if(mCallBack != null)
			mCallBack(this);
	}

	//Set state of toggle
	void SetState(int stateIndex)
	{
		if(_ToggleData.Length <= 0)
			return;

		miCurrentStateIndex = stateIndex;

		if(miCurrentStateIndex < _ToggleData.Length)
		{
			mCurState = _ToggleData[miCurrentStateIndex]._StateName;
			mImage.sprite = _ToggleData[miCurrentStateIndex]._Sprite;
		}
			
		if(_StorageName != "")
			PlayerPrefs.SetInt(_StorageName,miCurrentStateIndex);

		if(!_Cyclic)
		{
			if(stateIndex == 0)
			{
				if(_NextButton != null) _NextButton.SetInteractable(true);
				if(_PreviousButton != null) _PreviousButton.SetInteractable(false);
			}
			else if(stateIndex == _ToggleData.Length - 1)
			{
				if(_NextButton != null) _NextButton.SetInteractable(false);
				if(_PreviousButton != null) _PreviousButton.SetInteractable(true);
			}
			else
			{
				if(_NextButton != null) _NextButton.SetInteractable(true);
				if(_PreviousButton != null) _PreviousButton.SetInteractable(true);
			}
		}
	}
	
	public void onNextButton(GUIItem tItem)
	{
		//WHEN NEXT BUTTON IS PRESSED
		int nextSelectionIndex = miCurrentStateIndex+1;
		nextSelectionIndex %= _ToggleData.Length;
		SetState(nextSelectionIndex);
	}
	
	public void onPreviousButton(GUIItem tItem)
	{
		//WHEN NEXT BUTTON IS PRESSED
		int previousSelectionIndex = miCurrentStateIndex-1;
		if(previousSelectionIndex < 0)
			previousSelectionIndex = _ToggleData.Length - 1;
		SetState(previousSelectionIndex);
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
}