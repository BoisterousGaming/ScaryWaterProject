using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerOnZ : MonoBehaviour 
{
    Vector3 mvPosition;

    public Transform _PlayerTransform;

	void LateUpdate () 
    {
        mvPosition.x = 0f;
        mvPosition.y = 0f;
        mvPosition.z = _PlayerTransform.position.z;
        transform.position = mvPosition;
    }
}
