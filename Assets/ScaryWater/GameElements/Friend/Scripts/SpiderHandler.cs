using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderHandler : MonoBehaviour 
{
    bool mbSkipChecking;

    void OnEnable()
    {
        FriendManager.Instance._listOfFriends.Add(this.transform);
    }

    void OnDestroy()
    {
        FriendManager.Instance._listOfFriends.Remove(this.transform);
    }

    void OnTriggerEnter(Collider other)
    {
        if (FriendManager._bPlayerIsWithAFriend)
            return;

		if (other.CompareTag("Player"))
		{
            if (!mbSkipChecking)
            {
                mbSkipChecking = true;

				if (ScoreHandler._OnScoreEventCallback != null)
					ScoreHandler._OnScoreEventCallback(eScoreType.Spider);
                
				PlayerManager.Instance._playerHandler.DoSpiderJump();

				if (MiniGameManager.Instance.AutoImplementedProperties_eMiniGameState == eMiniGameState.AcceptFriendHelp || MiniGameManager.Instance.AutoImplementedProperties_eMiniGameState == eMiniGameState.AvoidFriend)
					MiniGameManager.Instance._iFriendsHelpAccepted += 1;

                Destroy(this.gameObject);
			}
		}
	}
}
