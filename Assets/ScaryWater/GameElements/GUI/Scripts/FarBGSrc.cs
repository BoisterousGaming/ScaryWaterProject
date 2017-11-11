using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarBGSrc : MonoBehaviour 
{
    Vector3 mvInitialPosition;
    Vector3 mvTempPos;

    public Transform _PlayerTransform;

    void Start()
    {
        mvInitialPosition = this.transform.position;    
    }

    void LateUpdate()
    {
        mvTempPos.x = mvInitialPosition.x;
        mvTempPos.y = mvInitialPosition.y;
        mvTempPos.z = _PlayerTransform.position.z + mvInitialPosition.z;

        this.transform.position = mvTempPos;
    }
}
