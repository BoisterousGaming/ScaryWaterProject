using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrokenHeartScr : MonoBehaviour 
{
    RectTransform mRectTransform;
    float mfLerpingSpeed = 2f;
    float mfHeartAlpha = 1f;
    Color mLeftSideColor;
    Color mRightSideColor;
    Color mTempColor;

    public GameplayAreaUIHandler _GameplayAreaUIHandlerScr;
    public RectTransform _LeftSideRect;
    public RectTransform _RightSideRect;
    public RectTransform _LeftSideTargetRect;
    public RectTransform _RightSideTargetRect;

    public void Initialize(RectTransform desireRect = null)
    {
        mLeftSideColor = _LeftSideRect.GetComponent<Image>().color;
        mRightSideColor = _RightSideRect.GetComponent<Image>().color;

        mRectTransform = this.GetComponent<RectTransform>();
        this.transform.SetParent(_GameplayAreaUIHandlerScr.transform);

        Vector3 tWorldPos = PlayerManager.Instance._playerHandler._tPlayerTransform.position;
        //Vector2 tScreenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, tWorldPos);
        //Vector2 tCanvasPos;
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(desireRect, tScreenPos, UICanvasHandler.Instance._RenderingCamera, out tCanvasPos);
        //tCanvasPos = 0.5f * desireRect.sizeDelta + tCanvasPos;
        //mRectTransform.anchoredPosition = tCanvasPos;
        //_LeftSideRect.anchoredPosition = tCanvasPos;
        //_RightSideRect.anchoredPosition = tCanvasPos;
        //_LeftSideTargetRect.anchoredPosition = tCanvasPos;
        //_RightSideTargetRect.anchoredPosition = tCanvasPos;

        this.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
    }

    void Update()
    {
        SetColorAndPosition();
    }

    void SetColorAndPosition()
    {
        _LeftSideRect.position = Vector3.Lerp(_LeftSideRect.position, _LeftSideTargetRect.position, Time.deltaTime);
        _RightSideRect.position = Vector3.Lerp(_RightSideRect.position, _RightSideTargetRect.position, Time.deltaTime);

        _LeftSideRect.rotation = Quaternion.Lerp(_LeftSideRect.rotation, _LeftSideTargetRect.rotation, Time.deltaTime);
        _RightSideRect.rotation = Quaternion.Lerp(_RightSideRect.rotation, _RightSideTargetRect.rotation, Time.deltaTime);

        mfHeartAlpha = Mathf.Lerp(mfHeartAlpha, 0f, Time.deltaTime * 1.5f);

        mLeftSideColor = _LeftSideRect.GetComponent<Image>().color;
        mTempColor.r = mLeftSideColor.r;
        mTempColor.b = mLeftSideColor.b;
        mTempColor.g = mLeftSideColor.g;
        mTempColor.a = mfHeartAlpha;
        _LeftSideRect.GetComponent<Image>().color = mTempColor;

        mRightSideColor = _RightSideRect.GetComponent<Image>().color;
        mTempColor.r = mRightSideColor.r;
        mTempColor.b = mRightSideColor.b;
        mTempColor.g = mRightSideColor.g;
        mTempColor.a = mfHeartAlpha;
        _RightSideRect.GetComponent<Image>().color = mTempColor;
    }
}
