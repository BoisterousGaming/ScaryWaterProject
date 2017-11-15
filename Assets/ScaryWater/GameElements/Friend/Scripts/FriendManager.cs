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

    public static eFriendType _eFriendType = eFriendType.None;
    public GameObject _airWingsPrefab;
    public List<AirWingsScr> _listOfAirWingsScr = new List<AirWingsScr>();
    public List<Transform> _listOfFriends = new List<Transform>();
    public static bool _bAirWingIsActive = false;
    public static bool _bPlayerIsWithAFriend = false;
    public static bool _bPlayerIsWithinFriendSurrounding = false;
    public static bool _bPlayerPressedTheAirwingBtn = false;
    public PlayerManager _playerManager; 

    static FriendManager mInstance = null;

    public static FriendManager Instance
    {
        get { return mInstance; }
    }

    void Awake()
    {
        mInstance = this;    
    }

    void Update()
    {
        if (_bPlayerPressedTheAirwingBtn)
            InstantiateAirWings(PlayerManager.Instance._playerHandler._tPlayerTransform.position.x);
    }

    public void InstantiateAirWings(float xPos)
    {
        if (_playerManager._bPlayerIsDead | _bPlayerIsWithAFriend | _bPlayerIsWithinFriendSurrounding)
            return;
        
        if (!_bAirWingIsActive)
        {
            _bPlayerPressedTheAirwingBtn = false;
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
            _bAirWingIsActive = true;
        }
    }
}
