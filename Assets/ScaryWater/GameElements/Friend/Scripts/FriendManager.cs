using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eFriendType
{
    None = 0,
    Spider,
    Duck,
    Kingfisher,
    Dragonfly
}

public class FriendManager : MonoBehaviour
{
    AirWingsScr mAirWingsScr;
    static bool mbPlayerIsWithAFriend = false;
    static bool mbAirWingIsActive = false;
    static FriendManager mInstance = null;

    public static eFriendType _eFriendType = eFriendType.None;
    public GameObject _airWingsPrefab;
    public GameObject _airWingsMovingPointsPrefab;
    public List<AirWingsScr> _listOfAirWingsScr = new List<AirWingsScr>();
    public List<Transform> _listOfFriends = new List<Transform>();
    public PlayerManager _playerManager; 
    public static bool _PlayerIsColseToAnotherFriend = false;

    public static FriendManager Instance
    {
        get { return mInstance; }
    }

    void Awake()
    {
        mInstance = this;    
    }

    public void InstantiateAirWings(float xPos)
    {
        if (_playerManager.GetPlayerDeadState() | mbPlayerIsWithAFriend)
            return;

        if (!mbAirWingIsActive)
        {
            DataManager.SubstarctFromAirwingAmount(1);

            GameObject tCanvas = UICanvasHandler.Instance.GetActiveCanvasByName("HUDCanvas");
            if (tCanvas != null)
            {
                GameplayAreaUIHandler tScr = tCanvas.GetComponent<GameplayAreaUIHandler>();
                tScr.DisplayAirwingCount();
                tScr.SetAirwingBtnState(false);
            }

			GameObject goAirWings = Instantiate(_airWingsPrefab);
			goAirWings.transform.SetParent(this.transform);
            goAirWings.transform.position = new Vector3(_playerManager._playerHandler.transform.position.x, 10f, _playerManager._playerHandler.transform.position.z - 20f);
			mAirWingsScr = (AirWingsScr)goAirWings.GetComponent<AirWingsScr>();
			mAirWingsScr._friendManager = this;
            mAirWingsScr._landingXPos = xPos;
			_listOfAirWingsScr.Add(mAirWingsScr);
            mbAirWingIsActive = true;
        }
    }

    public static void SetPlayerIsWithFriendState(bool state = true)
    {
        mbPlayerIsWithAFriend = state;
    }

    public static bool GetPlayerIsWithFriendState()
    {
        return mbPlayerIsWithAFriend;
    }

    public static void SetAirwingActiveState(bool state = true)
    {
        mbAirWingIsActive = state;
    }
}
