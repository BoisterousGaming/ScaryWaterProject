using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties : MonoBehaviour 
{
    public PlayerHandler _playerHandler;

    public void SetPlayerPhysic(bool pGravity = false, bool pIsKinematic = true, float pMass = 1f, float pDrag = 0f, float pAngDrag = 0.05f)
    {
        _playerHandler.GetComponent<Rigidbody>().useGravity = pGravity;
        _playerHandler.GetComponent<Rigidbody>().isKinematic = pIsKinematic;
        _playerHandler.GetComponent<Rigidbody>().mass = pMass;
        _playerHandler.GetComponent<Rigidbody>().drag = pDrag;
        _playerHandler.GetComponent<Rigidbody>().angularDrag = pAngDrag;
    }

    public void SetPlayerParent(Transform parentTransform = null)
    {
        if (parentTransform != null)
            _playerHandler.transform.SetParent(parentTransform);
        else
            _playerHandler.transform.SetParent(_playerHandler._playerManager.transform);
    }

    public void SetPlayerPosition(float pPosX = 0f, float pPosY = 0f, float pPosZ = 0f)
    {
        Vector3 tPosition = Vector3.zero;
        tPosition.x = pPosX;
        tPosition.y = pPosY;
        tPosition.z = pPosZ;
        _playerHandler.transform.position = tPosition;
    }

    public void SetPlayerRotation(float pRotX = 0f, float pRotY = 0f, float pRotZ = 0f, float pRotW = 0f)
    {
        Quaternion tRotation = Quaternion.identity;
        tRotation.x = pRotX;
        tRotation.y = pRotY;
        tRotation.z = pRotZ;
        tRotation.w = pRotW;
        _playerHandler.transform.rotation = tRotation;
    }

    public void SetPlayerScale(float pScaleX = 0f, float pScaleY = 0f, float pScaleZ = 0f)
    {
        Vector3 tScale = Vector3.zero;
        tScale.x = pScaleX;
        tScale.y = pScaleY;
        tScale.z = pScaleZ;
        _playerHandler.transform.localScale = tScale;
    }

    public void SetPlayerIdle()
    {
        _playerHandler._jumpActionScr.StopJump("Armature|idle");
    }

    public void SetPlayerControlState(eControlState pControl = eControlState.Active)
    {
        _playerHandler._eControlState = pControl;
    }

    public void SetPlayerRequiredAndNextPlatformPosition(Vector3 pDesiredPosition)
    {
        _playerHandler._vPlayerRequiredPosition = pDesiredPosition;
        _playerHandler._vNextPlatformPosition = pDesiredPosition;
    }
}
