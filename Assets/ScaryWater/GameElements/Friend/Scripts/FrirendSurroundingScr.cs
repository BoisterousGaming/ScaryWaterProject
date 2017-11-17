using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrirendSurroundingScr : MonoBehaviour 
{
    void OnTriggerEnter(Collider other)
    {
        SetAirwingBtnState(other, false);
    }

    void OnTriggerExit(Collider other)
    {
        SetAirwingBtnState(other, true); 
    }

    void SetAirwingBtnState(Collider other = null, bool state = false)
    {
        if (FriendManager.GetPlayerIsWithFriendState())
            return;
        
        if (other.CompareTag("Player"))
        {
            FriendManager.SetIfPlayerIsCloseToAFriend(!state);
            if (DataManager.GetAirwingAmount() > 0)
            {
                GameObject tCanvas = UICanvasHandler.Instance.GetActiveCanvasByName("HUDCanvas");
                if (tCanvas != null)
                    tCanvas.GetComponent<GameplayAreaUIHandler>().SetAirwingBtnState(state);
            }
        }
    }
}
