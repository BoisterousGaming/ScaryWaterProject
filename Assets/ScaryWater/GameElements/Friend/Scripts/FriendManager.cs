using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendManager : MonoBehaviour
{
    AirWingsScr mAirWingsScr;

    public GameObject _airWingsPrefab;
    public List<AirWingsScr> _listOfAirWingsScr = new List<AirWingsScr>();
    public List<Transform> _listOfFriends = new List<Transform>();
    public static bool _bAirWingIsActive;
    public static bool _bPlayerIsWithAFriend;

    static FriendManager mInstance = null;

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
        if (_bPlayerIsWithAFriend)
            return;
        
        if (!_bAirWingIsActive)
        {
			GameObject goAirWings = Instantiate(_airWingsPrefab);
			goAirWings.transform.SetParent(this.transform);
			goAirWings.transform.position = new Vector3(PlayerManager.Instance._playerHandler.transform.position.x, 10f, PlayerManager.Instance._playerHandler.transform.position.z - 20f);
			mAirWingsScr = (AirWingsScr)goAirWings.GetComponent<AirWingsScr>();
			mAirWingsScr._friendManager = this;
            mAirWingsScr._landingXPos = xPos;
			_listOfAirWingsScr.Add(mAirWingsScr);
            _bAirWingIsActive = true;
        }
    }
}
