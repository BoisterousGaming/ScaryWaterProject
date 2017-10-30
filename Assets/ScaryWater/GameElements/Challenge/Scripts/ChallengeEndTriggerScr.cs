using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeEndTriggerScr : MonoBehaviour 
{
    bool mbStopChecking;

    void OnTriggerEnter(Collider other)
    {
        if (!mbStopChecking)    
        {
            if (other.CompareTag("Player"))
                ChallengeManager.ChallengeIsComplete();
        }
    }
}
