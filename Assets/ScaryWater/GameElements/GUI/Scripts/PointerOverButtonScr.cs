using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerOverButtonScr : MonoBehaviour 
{
    public void PointerEnter()
    {
        PlayerManager.Instance._playerHandler._touchInputHandlerScr._bPointerOverBtn = true;
    }

    public void PointerExit()
    {
        PlayerManager.Instance._playerHandler._touchInputHandlerScr._bPointerOverBtn = false;
    }

    void OnDisable()
    {
        PlayerManager.Instance._playerHandler._touchInputHandlerScr._bPointerOverBtn = false;
    }
}
