using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BZCDuckScr : MonoBehaviour 
{
    Vector3 mvTempPos = Vector3.zero;
    bool mbSkipChecking = false;

    public BZCObjectPath _BZCObjectPathScr;

    void OnEnable()
    {
        FriendManager.Instance._listOfFriends.Add(this.transform);
        _BZCObjectPathScr._ArrivedAtTheDropPointCallback += UpdateFriendManagerAndDuckStateOnDrop;
        _BZCObjectPathScr._ArrivedAtTheDropPointCallback += UpdatePlayerStatesOnDrop;
    }

    void OnTriggerEnter(Collider other)
    {
        if (mbSkipChecking)
            return;
        if (other.CompareTag("Player"))
            InitiateDuck();    
    }

    void InitiateDuck()
    {
        mbSkipChecking = true;
        _BZCObjectPathScr.ResumePath();
        UpdateFriendManagerStatesOnPlayerPickup();
        UpdatePlayerStatesOnPickup();
        UpdatePlayerPositionOnTopOfDuck();
    }

    void UpdateFriendManagerStatesOnPlayerPickup()
    {
        FriendManager.SetPlayerIsWithFriendState(true);
        FriendManager.SetFriendType(eFriendType.Duck);
    }

    void UpdatePlayerStatesOnPickup()
    {
        PlayerManager.Instance._playerHandler._playerPropertiesScr.SetPlayerParent(this.transform);
        PlayerManager.Instance._CameraControllerScr.CameraFollowPlayerOnYAxis();
        PlayerManager.Instance._playerHandler._playerPropertiesScr.SetPlayerIdle();
        PlayerManager.Instance._playerHandler._playerPropertiesScr.SetPlayerPhysic();
    }

    void UpdatePlayerStatesOnDrop(Vector3 vPosition)
    {
        PlayerManager.Instance._playerHandler._playerPropertiesScr.SetPlayerParent();
        PlayerManager.Instance._playerHandler._playerPropertiesScr.SetPlayerRotation();
        PlayerManager.Instance._playerHandler._playerPropertiesScr.SetPlayerPhysic(true);
        PlayerManager.Instance._playerHandler._playerPropertiesScr.SetPlayerControlState();
        PlayerManager.Instance._playerHandler._playerPropertiesScr.SetPlayerRequiredAndNextPlatformPosition(vPosition);
        PlayerManager.Instance._CameraControllerScr.CameraFollowPlayerOnYAxis(false);
        PlayerManager.Instance._playerHandler.DoSingleJump();
    }

    void UpdateFriendManagerAndDuckStateOnDrop(Vector3 vPosition)
    {
        GetComponent<BoxCollider>().enabled = false;
        FriendManager.SetPlayerIsWithFriendState(false);
    }

    void UpdatePlayerPositionOnTopOfDuck()
    {
        Vector3 duckPosition = transform.position;
        mvTempPos.x = duckPosition.x;
        mvTempPos.y = duckPosition.y + 0.2f;
        mvTempPos.z = duckPosition.z - 0.65f;
        PlayerManager.Instance._playerHandler._playerPropertiesScr.SetPlayerPosition(mvTempPos.x, mvTempPos.y, mvTempPos.z);
    }

    void OnDisable()
    {
        FriendManager.Instance._listOfFriends.Remove(this.transform);

        if (_BZCObjectPathScr._ArrivedAtTheDropPointCallback != null)
        {
            _BZCObjectPathScr._ArrivedAtTheDropPointCallback -= UpdateFriendManagerAndDuckStateOnDrop;
            _BZCObjectPathScr._ArrivedAtTheDropPointCallback -= UpdatePlayerStatesOnDrop;
        }
    }
}
