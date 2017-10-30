using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveEnvironmentScr : MonoBehaviour 
{
    public eEnvID _eEnvID = eEnvID.Env_1;
    public EnvironmentUIHandler _environmentUIHandler;
    //public eEnvLockState _eEnvLockState = eEnvLockState.Locked;
    //public eEnvPurchasedState _eEnvPurchasedState = eEnvPurchasedState.No;
    //public eEnvActiveState _eEnvActiveState = eEnvActiveState.No;

    void OnEnable() 
    {
        _environmentUIHandler._iCurrentEnvID = (int)_eEnvID;
        _environmentUIHandler._sCurrentEnvName = _eEnvID.ToString();
	}
}
