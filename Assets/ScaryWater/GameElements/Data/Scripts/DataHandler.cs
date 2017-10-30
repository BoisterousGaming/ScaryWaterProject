using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataHandler
{
    #region PlayerHandler Variables
    public const float _fPlayerAutoJumpHeight = 1.5f;
    public const float _fPlayerHighAndLongJumpHeight = 4f;
    public const float _fPlayerSpiderInitiatedJumpHeight = 5f;
    public const float _fPlayerJumpDistance = 5f;
    public const float _fPlayerRegularSpeed = 17.5f; // 17.5
    public const float _fPlayerSpeedDuringLaneChange = 40f; // 40
    public const float _fPlayerSpeedIntiatedBySpider = 17.5f; // 17.5
    public const float _fPlayerCoinCollectionDistanceForMagnet = 25f;
    public const float _fPlayerRespawnWaitTimeInSecond = 2f;
    public const float _fSpaceBetweenLanes = 5f;
    public const float _fPlayerDistanceFromTheFriendOnPositiveYAxis = 1f;
    public const float _fPlayerDistanceFromTheFriendOnNegativeYAxis = 1f;
    #endregion

    #region PlayerManager Variables
    public const float _fReduceHealthWhilePerformingHighAndLongJump = 3f;
    public const float _fReduceHealthWhenInContactWithObstacle = 50f;
    public const float _fIncreaseHealthWhenConsumingFood = 10f;
    public const int _iNumberOfHealthBarToIncreaseOnTouchingOfPotion = 1;
    public const int _iNumberOfHealthBarToDecreaseOnTouchingOfInstantKill = 1;
    #endregion

    #region EnvironmentManager Variables
    public const float _fEnvironmentSetLength = 120f;
    #endregion

    #region MiscScript Variables
    public const float _fMaxDistanceAllowedForChecking = 0.5f;
    #endregion

    #region StoreContentPrice Variables
    public const int _iSkinPrice = 100;
    public const int _iLifePrice = 100;
    public const int _iPoisonPrice = 50;
    public const int _iAirwingPrice = 50;
    public const int _iMagnetPrice = 50;
    public const int _iMagnetTimePrice = 200;
    public const int _iPoisonRangePrice = 200;
    #endregion

    #region Process For SingletonClass
    //static DataHandler mInsatnce = null;

    //private DataHandler()
    //{

    //}

    //public static DataHandler Instance
    //{
    //    get
    //    {
    //        if (mInsatnce == null)
    //        {
    //            mInsatnce = new DataHandler();
    //        }
    //        return mInsatnce;
    //    }
    //}
    #endregion
}
