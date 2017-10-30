using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform mtPlayerTransform;
    Vector3 mvOffset;
    Vector3 mvTargetPosition;
    bool mbInitialized;

    void Initialize()
    {
		if (mtPlayerTransform == null)
		{
			mtPlayerTransform = PlayerManager.Instance._playerHandler._tPlayerTransform;
		}

		mvOffset = transform.position - mtPlayerTransform.position;
		float resolutionRatio = (float)Screen.width / (float)Screen.height;
		mvOffset.z *= 0.75f / resolutionRatio;
        mvOffset.y *= 0.75f / resolutionRatio;
    }

    void LateUpdate ()
    {
        if(!mbInitialized)
        {
            mbInitialized = true;
            Initialize();
        } 

        Vector3 tPlayerPosition = mtPlayerTransform.position;
        mvTargetPosition.x = mvOffset.x;
		mvTargetPosition.y = mvOffset.y;
		mvTargetPosition.z = tPlayerPosition.z + mvOffset.z;

        transform.position = Vector3.Lerp(transform.position, mvTargetPosition, Time.fixedDeltaTime * 10.0f);
    }
}
