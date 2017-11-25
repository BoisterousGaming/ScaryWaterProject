using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eFriendType
{
    None = 0,
    Spider,
    Duck,
    Kingfisher,
    Dragonfly,
    Airwing
}

public class FriendManager : MonoBehaviour
{
    AirWingsScr mAirWingsScr;
    static bool mbPlayerIsWithAFriend = false;
    static bool mbAirWingIsActive = false;
    static bool mbPlayerIsColseToAnotherFriend = false;
    static FriendManager mInstance = null;
    static eFriendType meFriendType = eFriendType.None;

    public GameObject _airWingsPrefab;
    public GameObject _airWingsMovingPointsPrefab;
    public GameObject[] _arrOfAirWingsMovingPoints;
    public List<Transform> _listOfFriends = new List<Transform>();
    public PlayerManager _playerManager; 

    public static FriendManager Instance
    {
        get { return mInstance; }
    }

    void Awake()
    {
        mInstance = this;    
    }

    void Start()
    {
        SetPlayerIsWithFriendState(false);    
        SetAirwingActiveState(false);
        SetFriendType(eFriendType.None);
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
            mAirWingsScr._fLandingPadXPos = xPos;
            mbAirWingIsActive = true;
        }
    }

    public void RemoveOtherFriendsIfWithinAirwingsDropPointRange()
    {
        for (int i = 0; i < _listOfFriends.Count; i++)
        {
            BirdHandler tBirdScr = _listOfFriends[i].GetComponent<BirdHandler>();
            if (tBirdScr != null)
            {
                if (mAirWingsScr._vLandingPadPosition.z >= tBirdScr._fPickUpPositionOnZAxis)
                    Destroy(tBirdScr.gameObject);
            }

            DuckHandler tDuckScr = _listOfFriends[i].GetComponent<DuckHandler>();
            if (tDuckScr != null)
            {
                if (mAirWingsScr._vLandingPadPosition.z >= tDuckScr._fPickUpPositionOnZAxis)
                    Destroy(tDuckScr.gameObject);
            }
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

    public static void SetIfPlayerIsCloseToAFriend(bool state = true)
    {
        mbPlayerIsColseToAnotherFriend = state;
    }

    public static bool GetIfPlayerIsCloseToAFriend()
    {
        return mbPlayerIsColseToAnotherFriend;
    }

    public static void SetFriendType(eFriendType eType)
    {
        meFriendType = eType;
    }

    public static eFriendType GetFriendType()
    {
        return meFriendType;
    }
}
