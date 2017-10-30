using UnityEngine;
using System.Collections;

[RequireComponent (typeof (MenuItemEntryAnim))]
[RequireComponent (typeof (MenuItemExitAnim))]

public class GUIEditMode : MonoBehaviour {

	public enum eState
	{
		Start,
		Settled,
	}

	public eState _State = eState.Start;

	//Start of entry animation end of exit animation
	void SaveStartCondition()
	{
		//Start of entry animation
		MenuItemEntryAnim animEntry = GetComponent<MenuItemEntryAnim>();
		RectTransform tRectTransform = GetComponent<RectTransform> ();

		if(animEntry != null)
		{
			if(tRectTransform != null)
				animEntry._StartState._Position = tRectTransform.anchoredPosition;
			else
				animEntry._StartState._Position = transform.localPosition;
			
			animEntry._StartState._Scale = transform.localScale;
			animEntry._StartState._Rotation = transform.localEulerAngles;
			animEntry._StartState._Color = GetComponent<GUIItem>().GetColor();
		}

		MenuItemExitAnim animExit = GetComponent<MenuItemExitAnim>();
		if(animExit != null)
		{
			if(tRectTransform != null)
				animExit._EndState._Position = tRectTransform.anchoredPosition;
			else
				animExit._EndState._Position = transform.localPosition;
			
			animExit._EndState._Scale = transform.localScale;
			animExit._EndState._Rotation = transform.localEulerAngles;
			animExit._EndState._Color = GetComponent<GUIItem>().GetColor();
		}
	}

	//End of entry animation start of exit animation
	void SaveSettledCondition()
	{
		//Start of entry animation
		MenuItemEntryAnim animEntry = GetComponent<MenuItemEntryAnim>();
		RectTransform tRectTransform = GetComponent<RectTransform> ();

		if(animEntry != null)
		{
			if(tRectTransform != null)
				animEntry._EndState._Position = tRectTransform.anchoredPosition;
			else
				animEntry._EndState._Position = transform.localPosition;
			
			animEntry._EndState._Scale = transform.localScale;
			animEntry._EndState._Rotation = transform.localEulerAngles;
			animEntry._EndState._Color = GetComponent<GUIItem>().GetColor();
		}

		MenuItemExitAnim animExit = GetComponent<MenuItemExitAnim>();
		if(animExit != null)
		{
			if(tRectTransform != null)
				animExit._StartState._Position = tRectTransform.anchoredPosition;
			else
				animExit._StartState._Position = transform.localPosition;
			
			animExit._StartState._Scale = transform.localScale;
			animExit._StartState._Rotation = transform.localEulerAngles;
			animExit._StartState._Color = GetComponent<GUIItem>().GetColor();
		}
	}

	void ShowStartCondition()
	{
		MenuItemEntryAnim animEntry = GetComponent<MenuItemEntryAnim>();
		RectTransform tRectTransform = GetComponent<RectTransform> ();
		if(animEntry != null)
		{
			if(tRectTransform != null)
				tRectTransform.anchoredPosition = animEntry._StartState._Position;
			else
				transform.localPosition = animEntry._StartState._Position;
			
			transform.localScale = animEntry._StartState._Scale;
			transform.localEulerAngles = animEntry._StartState._Rotation;
			GetComponent<GUIItem>().SetColor(animEntry._StartState._Color);
		}
	}
	
	void ShowSettledCondition()
	{
		MenuItemEntryAnim animEntry = GetComponent<MenuItemEntryAnim>();
		RectTransform tRectTransform = GetComponent<RectTransform> ();
		if(animEntry != null)
		{
			if(tRectTransform != null)
				tRectTransform.anchoredPosition = animEntry._EndState._Position;
			else
				transform.localPosition = animEntry._EndState._Position;
			
			transform.localScale = animEntry._EndState._Scale;
			transform.localEulerAngles = animEntry._EndState._Rotation;
			GetComponent<GUIItem>().SetColor(animEntry._EndState._Color);
		}
	}

	public void DoSave()
	{
		if (_State == eState.Start)
			SaveStartCondition ();
		else if (_State == eState.Settled)
			SaveSettledCondition ();
	}

	public void DoShow()
	{
		if (_State == eState.Start)
			ShowStartCondition ();
		else if (_State == eState.Settled)
			ShowSettledCondition ();
	}

	public void NextState()
	{
		if (_State == eState.Start)
			_State = eState.Settled;
		else
			_State = eState.Start;
	}
}
