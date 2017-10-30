using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinSpriteScr : MonoBehaviour 
{
	RectTransform mTempRectTransform;
    float mfLerpingSpeed = 2f;

    public GameplayAreaUIHandler _GameplayAreaUIHandlerScr;

    public void Initialize()
    {
		RectTransform tCoinRect = this.GetComponent<RectTransform>();
		this.transform.SetParent(_GameplayAreaUIHandlerScr.transform);
		tCoinRect.localScale = new Vector3(0.5f, 0.5f, 0.5f);

		if (PlayerManager.Instance._playerHandler._iLaneNumber == -1)
			tCoinRect.position = _GameplayAreaUIHandlerScr._LeftLanePosRect.position;

		else if (PlayerManager.Instance._playerHandler._iLaneNumber == 0)
			tCoinRect.position = _GameplayAreaUIHandlerScr._MiddleLanePosRect.position;

		else if (PlayerManager.Instance._playerHandler._iLaneNumber == 1)
			tCoinRect.position = _GameplayAreaUIHandlerScr._RightLanePosRect.position;
    }

	void Update () 
    {
		mTempRectTransform = this.GetComponent<RectTransform>();
		mTempRectTransform.position = Vector3.Lerp(mTempRectTransform.position, _GameplayAreaUIHandlerScr._CoinTargetPosRect.position, Time.deltaTime * mfLerpingSpeed);

		if (Vector3.Distance(mTempRectTransform.position, _GameplayAreaUIHandlerScr._CoinTargetPosRect.position) < 0.01f)
			Destroy(this.gameObject);
	}
}
