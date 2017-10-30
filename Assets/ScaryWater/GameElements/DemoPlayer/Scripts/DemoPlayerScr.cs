using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoPlayerScr : MonoBehaviour 
{
    int miEquipedSkinID;
    float mfRotationSpeed = 10f;

    public Texture[] _arrOfPlayerTexture;

    void Start()
    {
        EquipSkin(0, false);    
    }

    public void EquipSkin(int skinID = 0, bool state = false)
	{
        if (!state)
            miEquipedSkinID = DataManager.GetEquipedSkinID();

        else if (state)
            miEquipedSkinID = skinID;

        for (int i = 0; i < _arrOfPlayerTexture.Length; i++)
        {
            if (miEquipedSkinID == i)
                this.transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material.mainTexture = _arrOfPlayerTexture[i];
        }
	}

	void Update () 
    {
        transform.Rotate(0f, 6.0f * mfRotationSpeed * Time.deltaTime, 0f);
	}
}
