using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderHandler : MonoBehaviour 
{
    bool mbSkipChecking;
    bool mbShouldDestroy = false;

    void OnEnable()
    {
        FriendManager.Instance._listOfFriends.Add(this.transform);
    }

    void Update()
    {
        if (mbShouldDestroy)
            Destroy(this.gameObject);
    }

    void OnDestroy()
    {
        FriendManager.Instance._listOfFriends.Remove(this.transform);
    }

    void OnTriggerEnter(Collider other)
    {
        if (FriendManager.GetPlayerIsWithFriendState())
            return;

		if (other.CompareTag("Player"))
		{
            if (!mbSkipChecking)
            {
                mbSkipChecking = true;

				if (ScoreHandler._OnScoreEventCallback != null)
					ScoreHandler._OnScoreEventCallback(eScoreType.Spider);
                
				PlayerManager.Instance._playerHandler.DoSpiderJump();

				if (MiniGameManager.Instance._eMiniGameState == eMiniGameState.AcceptFriendHelp || MiniGameManager.Instance._eMiniGameState == eMiniGameState.AvoidFriend)
					MiniGameManager.Instance._iFriendsHelpAccepted += 1;

                mbShouldDestroy = true;
			}
		}
	}
}
