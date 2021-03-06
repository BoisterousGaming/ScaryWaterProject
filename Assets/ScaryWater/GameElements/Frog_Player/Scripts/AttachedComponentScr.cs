﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachedComponentScr : MonoBehaviour 
{
    Vector3 mvPosition;
    Vector3 mvRainInitialPos;
    Vector3 mvFarBGInitialPos;
    Transform mfRainTransform;

    public ParticleSystem _CloudLToR;
    public ParticleSystem _CloudRToL;
    public ParticleSystem _Thunder;
    public ParticleSystem _Rain;
    public Transform _FarBG;
    public Transform _PlayerTransform;

    void Start()
    {
        mfRainTransform = _Rain.gameObject.transform;
        mvRainInitialPos = mfRainTransform.position;
        mvFarBGInitialPos = _FarBG.position;
    }

    void LateUpdate()
    {
        SetRainPosition();
        SetFarBGPosition();
    }

    void SetRainPosition()
    {
        mvPosition.x = 0f;
        mvPosition.y = mvRainInitialPos.y;
        mvPosition.z = _PlayerTransform.position.z + mvRainInitialPos.z;
        mfRainTransform.position = mvPosition;
    }

    void SetFarBGPosition()
    {
        mvPosition.x = 0f;
        mvPosition.y = mvFarBGInitialPos.y;
        mvPosition.z = _PlayerTransform.position.z + mvFarBGInitialPos.z;
        _FarBG.position = mvPosition;
    }
}
