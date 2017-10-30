using UnityEngine;
using System;
using System.Collections;

public enum eItemType
{
	Item = 0,
	Image,
	Text,
	Button,
	Toggle,
	Panel,
}

public class GUIItem : MonoBehaviour 
{	
	protected eItemType meItemType;
	protected Color mColor;

	void Awake()
	{
		meItemType = eItemType.Item;
	}

	public eItemType ItemType
	{
		get
		{
			return meItemType;
		}
	}

	public virtual Color GetColor(){return Color.white;}
	public virtual void SetColor(Color color){}

	public virtual bool BeginEntryAnimation(Action callBack)
	{
		bool willAnimate = false;
		MenuItemEntryAnim tAnim = GetComponent<MenuItemEntryAnim>();
		if(tAnim != null)
		{
			willAnimate = true;
			tAnim.AnimationWithCallBack(callBack);
		}
		return willAnimate;
	}

	public virtual bool BeginExitAnimation(Action callBack)
	{
		bool willAnimate = false;
		MenuItemExitAnim tAnim = GetComponent<MenuItemExitAnim>();
		if(tAnim != null)
		{
			willAnimate = true;
			tAnim.AnimationWithCallBack(callBack);
		}
		return willAnimate;
	}

	public virtual void StopAllActions()
	{
		MenuItemEntryAnim tAnimEntry = GetComponent<MenuItemEntryAnim>();
		if(tAnimEntry != null)
			tAnimEntry.StopAllActions();

		MenuItemExitAnim tAnimExit = GetComponent<MenuItemExitAnim>();
		if(tAnimExit != null)
			tAnimExit.StopAllActions();
	}

	public virtual bool HasColorParameter()
	{
		return false;
	}
}
