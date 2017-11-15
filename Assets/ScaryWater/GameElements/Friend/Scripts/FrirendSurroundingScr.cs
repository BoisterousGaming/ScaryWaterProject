using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrirendSurroundingScr : MonoBehaviour 
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            FriendManager._bPlayerIsWithinFriendSurrounding = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FriendManager._bPlayerIsWithinFriendSurrounding = false;
            GetComponent<Collider>().enabled = false;    
        }
    }
}
