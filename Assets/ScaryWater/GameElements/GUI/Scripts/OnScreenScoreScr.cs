using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OnScreenScoreScr : MonoBehaviour
{
    RectTransform mRectTransform;
    TextMeshProUGUI mOnScreenScoreText;
    float mfLerpingSpeed = 0.1f;
    float mfTextAlpha = 1f;
    Color mTextColor;

    public GameplayAreaUIHandler _GameplayAreaUIHandlerScr;

    public void Initialize(int Value = 0, RectTransform desireRect = null)
    {
        mOnScreenScoreText = this.GetComponent<TextMeshProUGUI>();
        mTextColor = mOnScreenScoreText.color;

        mRectTransform = this.GetComponent<RectTransform>();
        this.transform.SetParent(_GameplayAreaUIHandlerScr.transform);

        mOnScreenScoreText.text = "+" + Value.ToString();

        Vector3 tWorldPos = PlayerManager.Instance._playerHandler._tPlayerTransform.position;
        Vector2 tScreenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, tWorldPos);
        Vector2 tCanvasPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(desireRect, tScreenPos, UICanvasHandler.Instance._RenderingCamera, out tCanvasPos);
        tCanvasPos = 0.5f * desireRect.sizeDelta + tCanvasPos;
        mRectTransform.anchoredPosition = tCanvasPos;

        this.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
    }

    void Update()
    {
        UpdateTextPositionAndColor();
    }

    void UpdateTextPositionAndColor()
    {
        mRectTransform = this.GetComponent<RectTransform>();
        mRectTransform.position = Vector3.Lerp(mRectTransform.position, _GameplayAreaUIHandlerScr._ScoreTargetPosRect.position, Time.deltaTime * mfLerpingSpeed);

        mfTextAlpha = Mathf.Lerp(mfTextAlpha, 0f, Time.deltaTime * 1.5f);

        Color tColor;
        tColor.r = mTextColor.r;
        tColor.g = mTextColor.g;
        tColor.b = mTextColor.b;
        tColor.a = mfTextAlpha;
        mOnScreenScoreText.color = tColor;

        if (Vector3.Distance(mRectTransform.position, _GameplayAreaUIHandlerScr._ScoreTargetPosRect.position) < 1f)
            Destroy(this.gameObject);
    }
}
