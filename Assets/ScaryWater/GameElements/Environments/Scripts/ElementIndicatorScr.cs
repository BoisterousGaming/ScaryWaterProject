using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementIndicatorScr : MonoBehaviour 
{
    int miCount = 0;
    bool mbCurrentState = false;
    float mfTimeDelay = 1f;
    float mfTimeInCurBlinkState = 0.0f;
    Transform mtPlayerPos;

    void Start()
    {
        mtPlayerPos = PlayerManager.Instance._playerHandler._tPlayerTransform;
        mfTimeInCurBlinkState = 0.0f;
    }

    void LateUpdate()
    {
        if (transform.position.z - PlayerManager.Instance._playerHandler._tPlayerTransform.position.z < 60f)
        //if (Vector3.Distance(transform.position, mtPlayerPos.position) < 60f)
        {
            if (mfTimeInCurBlinkState < mfTimeDelay)
            {
                mfTimeInCurBlinkState += Time.deltaTime;

                if (miCount < 20)
                    SetVisibility();
                else
                    transform.GetComponent<MeshRenderer>().enabled = false; 
            }
        }
    }

    void SetVisibility()
    {
        if (mbCurrentState)
        {
            mbCurrentState = false;
            transform.GetComponent<MeshRenderer>().enabled = true;
            mfTimeInCurBlinkState = 0.0f;
        }
        else
        {
            mbCurrentState = true;
            transform.GetComponent<MeshRenderer>().enabled = false;
            mfTimeInCurBlinkState = 0.0f;
        }
        miCount++;
    }
}
