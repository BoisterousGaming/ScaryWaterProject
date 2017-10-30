using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflySpriteScr : MonoBehaviour 
{
	RectTransform mTempRectTransform;
	float mfLerpingSpeed = 2f;

	public GameplayAreaUIHandler _GameplayAreaUIHandlerScr;

	public void Initialize()
	{
		RectTransform tButterflyRect = this.GetComponent<RectTransform>();
		this.transform.SetParent(_GameplayAreaUIHandlerScr.transform);
		tButterflyRect.localScale = new Vector3(0.5f, 0.5f, 0.5f);

		if (PlayerManager.Instance._playerHandler._iLaneNumber == -1)
			tButterflyRect.position = _GameplayAreaUIHandlerScr._LeftLanePosRect.position;

		else if (PlayerManager.Instance._playerHandler._iLaneNumber == 0)
			tButterflyRect.position = _GameplayAreaUIHandlerScr._MiddleLanePosRect.position;

		else if (PlayerManager.Instance._playerHandler._iLaneNumber == 1)
			tButterflyRect.position = _GameplayAreaUIHandlerScr._RightLanePosRect.position;
	}

	void Update () 
    {
		mTempRectTransform = this.GetComponent<RectTransform>();
		mTempRectTransform.position = Vector3.Lerp(mTempRectTransform.position, _GameplayAreaUIHandlerScr._ButterflyTargetPosRect.position, Time.deltaTime * mfLerpingSpeed);

		if (Vector3.Distance(mTempRectTransform.position, _GameplayAreaUIHandlerScr._ButterflyTargetPosRect.position) < 1f)
			Destroy(this.gameObject);
	}
}
