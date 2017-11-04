using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform mtPlayerTransform;
    Transform mTransform;
    Vector3 mvOffset;
    Vector3 mvOriginalPosition;
    Vector3 mvOriginalLocalPosition;
    Vector3 mvTargetPosition;
    bool mbInitialized;

    public bool _bFollowPlayerY = false;

    void Initialize()
    {
        mTransform = transform;
		if (mtPlayerTransform == null)
		{
			mtPlayerTransform = PlayerManager.Instance._playerHandler._tPlayerTransform;
            mTransform.parent = mtPlayerTransform;
		}
        mvOriginalPosition = mTransform.position;
		//mvOffset = transform.position - mtPlayerTransform.position;
		//float resolutionRatio = (float)Screen.width / (float)Screen.height;
		//mvOffset.z *= 0.75f / resolutionRatio;
        //mvOffset.y *= 0.75f / resolutionRatio;
    }

    void LateUpdate ()
    {
        if(!mbInitialized)
        {
            mbInitialized = true;
            Initialize();
        }

        if (!_bFollowPlayerY)
        {
            Vector3 tPlayerPosition = mtPlayerTransform.position;
            Vector3 tCamPos = mTransform.position;
            //mvTargetPosition.x = mvOffset.x + tPlayerPosition.x * 0.75f;
            //mvTargetPosition.y = mvOffset.y;
            //mvTargetPosition.z = tPlayerPosition.z + mvOffset.z;
            //mvTargetPosition = tPlayerPosition + mvOffset;
            tPlayerPosition.y = mvOriginalPosition.y;
            tPlayerPosition.x = tCamPos.x;
            tPlayerPosition.z = tCamPos.z;

            transform.position = tPlayerPosition; //Vector3.Lerp(transform.position, mvTargetPosition, Time.fixedDeltaTime * 10.0f);
        }
        else
        {
            Vector3 tPlayerPosition = mtPlayerTransform.position;
            Vector3 tCamPos = mTransform.position;
            tPlayerPosition.y = tCamPos.y;
            tPlayerPosition.x = tCamPos.x;
            tPlayerPosition.z = tCamPos.z;
            transform.position = tPlayerPosition;
        }
    }
}
