using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIItemImage : GUIItem
{
	Image mImage;

	// Use this for initialization
	void Start() 
	{
		meItemType = eItemType.Image;
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