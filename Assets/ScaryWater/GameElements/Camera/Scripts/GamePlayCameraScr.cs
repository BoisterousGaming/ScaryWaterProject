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
    bool mbEnableCameraControl = false;

    public BZCObjectPath _BZCCameraPath;

    void OnEnable()
    {
        _BZCCameraPath._ArrivedAtTheEndPointCallback += Initialize;   
    }

    void OnDisable()
    {
        _BZCCameraPath._ArrivedAtTheEndPointCallback -= Initialize;
    }

    void Initialize(Vector3 vPos)
    {
        mTransform = transform;
        mInitialRotation = transform.rotation;
		if (mtPlayerTransform == null)
		{
			mtPlayerTransform = PlayerManager.Instance._playerHandler._tPlayerTransform;
            mTransform.parent = mtPlayerTransform;
		}
        mvOriginalPosition = mTransform.position;
        mbEnableCameraControl = true;
    }

    void LateUpdate ()
    {
        //if(!mbInitialized)
        //{
        //    mbInitialized = true;
        //    Initialize();
        //}

        if (!mbEnableCameraControl)
            return;

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
