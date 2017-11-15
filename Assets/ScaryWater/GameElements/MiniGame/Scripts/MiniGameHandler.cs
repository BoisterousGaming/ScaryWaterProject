using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum eMiniGameTypes
{
	None = -1,
	CollectCoins,
	CollectButterflies,
	StunEnemy,
	AcceptFriendHelp,
	EatFood,
	CollectPowerUp,
	LeftRightMovement,
	HighAndLongJump,
	AvoidEnemy,
	AvoidFriend,
	AvoidFood,
	AvoidPowerUp,
	AvoidObstacles,
	AvoidDying
}

public class MiniGameHandler : MonoBehaviour 
{
    bool mbLockTrigger = false;
    bool mbRenderIsDisable = false;

    public eMiniGameTypes _eMiniGameTypes = eMiniGameTypes.None;
    public int _iCoinsNeedsToCollect;
    public int _iButterfliesNeedsToCollect;
    public int _iEnemiesNeedsToStun;
    public int _iFriendsHelpIsRequired;
    public int _iLeftRightMovementNeedsToPerform;
    public int _iHighAndLongJumpNeedsToPerform;
    public int _iFoodsNeedsToEat;
    public int _iPowerUpsNeedsToCollect;
    public int _iReward_CoinsAmount;
    public int _iReward_ButterfliesAmount;
    public float _fMinGameTimeLengthInSecond;

    void OnEnable()
    {
        MiniGameManager.Instance._listOfMinGameHandlers.Add(this); 
    }

    void Update()
    {
        SetVisibility();
    }

    void SetVisibility()
    {
        if (MiniGameManager._bMinGameIsActive & !mbRenderIsDisable)
            VisibilityState(true, false);

        else if (!MiniGameManager._bMinGameIsActive & mbRenderIsDisable)
            VisibilityState(false, true);
    }

    void VisibilityState(bool state, bool renderState)
    {
        mbRenderIsDisable = state;
        this.GetComponent<MeshRenderer>().enabled = renderState;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!mbLockTrigger)
        {
            mbLockTrigger = true;

            if (FriendManager.GetPlayerIsWithFriendState())
				return;

            if (other.CompareTag("Player"))
                MiniGameManager.Instance.CheckIfMiniGameCanBeActivated(this);
        }
    }

    void OnDestroy()
    {
        MiniGameManager.Instance._listOfMinGameHandlers.Remove(this);
    }
}

// Runtime code here
#if UNITY_EDITOR
// Editor specific code here
[CustomEditor(typeof(MiniGameHandler))]
public class MiniGameEditor : Editor
{
	public override void OnInspectorGUI()
	{
		MiniGameHandler mMiniGameHandlerScr = (MiniGameHandler)target;

		mMiniGameHandlerScr._eMiniGameTypes = (eMiniGameTypes)EditorGUILayout.EnumPopup("MiniGame Type", mMiniGameHandlerScr._eMiniGameTypes);

		switch (mMiniGameHandlerScr._eMiniGameTypes)
		{
			case eMiniGameTypes.CollectCoins:
				mMiniGameHandlerScr._iCoinsNeedsToCollect = EditorGUILayout.IntField("Coin Amount", mMiniGameHandlerScr._iCoinsNeedsToCollect);
				break;

			case eMiniGameTypes.CollectButterflies:
				mMiniGameHandlerScr._iButterfliesNeedsToCollect = EditorGUILayout.IntField("Butterfly Amount", mMiniGameHandlerScr._iButterfliesNeedsToCollect);
				break;

			case eMiniGameTypes.StunEnemy:
				mMiniGameHandlerScr._iEnemiesNeedsToStun = EditorGUILayout.IntField("Stunt Enemy Amount", mMiniGameHandlerScr._iEnemiesNeedsToStun);
				break;

			case eMiniGameTypes.AcceptFriendHelp:
				mMiniGameHandlerScr._iFriendsHelpIsRequired = EditorGUILayout.IntField("Friend Help Amount", mMiniGameHandlerScr._iFriendsHelpIsRequired);
				break;

			case eMiniGameTypes.LeftRightMovement:
				mMiniGameHandlerScr._iLeftRightMovementNeedsToPerform = EditorGUILayout.IntField("Left&Right Amount", mMiniGameHandlerScr._iLeftRightMovementNeedsToPerform);
				break;

			case eMiniGameTypes.HighAndLongJump:
				mMiniGameHandlerScr._iHighAndLongJumpNeedsToPerform = EditorGUILayout.IntField("High&Long Jump Amount", mMiniGameHandlerScr._iHighAndLongJumpNeedsToPerform);
				break;

			case eMiniGameTypes.EatFood:
				mMiniGameHandlerScr._iFoodsNeedsToEat = EditorGUILayout.IntField("Food Amount", mMiniGameHandlerScr._iFoodsNeedsToEat);
				break;

			case eMiniGameTypes.CollectPowerUp:
				mMiniGameHandlerScr._iPowerUpsNeedsToCollect = EditorGUILayout.IntField("PowerUp Amount", mMiniGameHandlerScr._iPowerUpsNeedsToCollect);
				break;
		}

		mMiniGameHandlerScr._iReward_CoinsAmount = EditorGUILayout.IntField("Coin Reward Amount", mMiniGameHandlerScr._iReward_CoinsAmount);
		mMiniGameHandlerScr._iReward_ButterfliesAmount = EditorGUILayout.IntField("Butterfly Reward Amount", mMiniGameHandlerScr._iReward_ButterfliesAmount);
		mMiniGameHandlerScr._fMinGameTimeLengthInSecond = EditorGUILayout.FloatField("MiniGame Time Limit", mMiniGameHandlerScr._fMinGameTimeLengthInSecond);
	}
}
#endif
// Runtime code here
