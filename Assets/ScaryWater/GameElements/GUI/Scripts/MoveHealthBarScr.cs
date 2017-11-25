using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHealthBarScr : MonoBehaviour 
{
    RectTransform mTempRectTransform;
    float mfLerpingSpeed = 2f;

    public GameplayAreaUIHandler _GameplayAreaUIHandlerScr;
    public RectTransform _LeftLanePosRect;
    public RectTransform _MiddleLanePosRect;
    public RectTransform _RightLanePosRect;

    void Update()
    {
        if (PlayerManager.Instance._playerHandler._iLaneNumber == -1)
        {
            mTempRectTransform = this.GetComponent<RectTransform>();
            mTempRectTransform.position = Vector3.Lerp(mTempRectTransform.position, _LeftLanePosRect.position, Time.deltaTime * mfLerpingSpeed);
        }

        else if (PlayerManager.Instance._playerHandler._iLaneNumber == 0)
        {
            mTempRectTransform = this.GetComponent<RectTransform>();
            mTempRectTransform.position = Vector3.Lerp(mTempRectTransform.position, _MiddleLanePosRect.position, Time.deltaTime * mfLerpingSpeed);
        }

        else if (PlayerManager.Instance._playerHandler._iLaneNumber == 1)
        {
            mTempRectTransform = this.GetComponent<RectTransform>();
            mTempRectTransform.position = Vector3.Lerp(mTempRectTransform.position, _RightLanePosRect.position, Time.deltaTime * mfLerpingSpeed);
        }
    }
}
