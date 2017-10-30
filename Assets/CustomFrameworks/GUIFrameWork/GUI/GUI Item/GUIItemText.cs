using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIItemText : GUIItem 
{
	public bool _Localized = false;
	Text mText;

	void Awake()
	{
		meItemType = eItemType.Text;
		mText = GetComponent<Text>();
	}

	public override bool HasColorParameter()
	{
		return true;
	}

	public override Color GetColor()
	{
		if(mText == null)
			mText = GetComponent<Text>();

		return mText.color;
	}

	public override void SetColor(Color color)
	{
		if(mText == null)
			mText = GetComponent<Text>();

		mText.color = color;
	}
}
