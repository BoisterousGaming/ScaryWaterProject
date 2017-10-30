using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinDataScr : MonoBehaviour 
{
    public ePlayerSkinID _ePlayerSkinID = ePlayerSkinID.Skin_1;
	//public ePlayerSkinLockState _ePlayerSkinLockState = ePlayerSkinLockState.Locked;
	//public ePlayerSkinPurchaseState _ePlayerSkinPurchaseState = ePlayerSkinPurchaseState.No;
	//public ePlayerSkinEquipState _ePlayerSkinEquipState = ePlayerSkinEquipState.No;
    public int _iSkinID;

    void Start()
    {
        _iSkinID = (int)_ePlayerSkinID;    
    }
}
