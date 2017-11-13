﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinSpriteScr : MonoBehaviour 
{
    RectTransform mRectTransform;
    float mfLerpingSpeed = 2f;

    public GameplayAreaUIHandler _GameplayAreaUIHandlerScr;

    public void Initialize(Transform coinTransform = null, RectTransform desireRect = null, bool state = true)
    {
        mRectTransform = this.GetComponent<RectTransform>();
        this.transform.SetParent(_GameplayAreaUIHandlerScr.transform);

        if (state)
        {
            Vector3 tWorldPos = coinTransform.position;
            Vector2 tScreenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, tWorldPos);
            Vector2 tCanvasPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(desireRect, tScreenPos, UICanvasHandler.Instance._RenderingCamera, out tCanvasPos);
            tCanvasPos = 0.5f * desireRect.sizeDelta + tCanvasPos;
            mRectTransform.anchoredPosition = tCanvasPos;
        }

        else
            mRectTransform = desireRect;

        this.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

	void Update () 
    {
		//mRectTransform = this.GetComponent<RectTransform>();
		mRectTransform.position = Vector3.Lerp(mRectTransform.position, _GameplayAreaUIHandlerScr._CoinTargetPosRect.position, Time.deltaTime * mfLerpingSpeed);

		if (Vector3.Distance(mRectTransform.position, _GameplayAreaUIHandlerScr._CoinTargetPosRect.position) < 1f)
			Destroy(this.gameObject);
	}
}
