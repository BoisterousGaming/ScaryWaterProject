using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayCameraScr : MonoBehaviour
{
    Transform mtPlayerTransform;
    Transform mTransform;
    Quaternion mInitialRotation;
    Vector3 mvOriginalPosition;
    bool mbInitialized = false;
    bool mbFollowPlayerY = false;

    void Initialize()
    {
        mTransform = transform;
        mInitialRotation = transform.rotation;
		if (mtPlayerTransform == null)
		{
			mtPlayerTransform = PlayerManager.Instance._playerHandler._tPlayerTransform;
            mTransform.parent = mtPlayerTransform;
		}
        mvOriginalPosition = mTransform.position;
    }

    void LateUpdate ()
    {
        if(!mbInitialized)
        {
            mbInitialized = true;
            Initialize();
        }

        if (!mbFollowPlayerY)
        {
            Vector3 tPlayerPosition = mtPlayerTransform.position;
            Vector3 tCamPos = mTransform.position;
            tPlayerPosition.y = mvOriginalPosition.y;
            tPlayerPosition.x = tCamPos.x;
            tPlayerPosition.z = tCamPos.z;
            transform.position = tPlayerPosition;
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

        if (!FriendManager.GetPlayerIsWithFriendState())
            mTransform.rotation = mInitialRotation;
    }

    public void CameraFollowPlayerOnYAxis(bool state = true)
    {
        mbFollowPlayerY = state;
    }
}
