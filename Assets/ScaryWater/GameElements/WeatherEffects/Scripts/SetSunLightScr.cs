using UnityEngine;
using System.Collections;

public class SetSunLightScr : MonoBehaviour
{
    //Material mSkyMat;
    //bool mbLightOn = false;

    //public Renderer _Lightwall;
    //public Renderer _Water;
    //public Transform _tStars;
    //public Transform _tWorldProbe;

    //void Start()
    //{
    //    mSkyMat = RenderSettings.skybox;
    //}

    //void Update()
    //{
    //    _tStars.transform.rotation = transform.rotation;

    //    if (Input.GetKeyDown(KeyCode.T))
    //        mbLightOn = !mbLightOn;

    //    if (mbLightOn)
    //        UpdateFinalColor(5);
    //    else
    //        UpdateFinalColor(0);

    //    Vector3 tVector = Camera.main.transform.position;
    //    _tWorldProbe.transform.position = tVector;

    //    _Water.material.mainTextureOffset = new Vector2(Time.time / 100, 0);
    //    _Water.material.SetTextureOffset("_DetailAlbedoMap", new Vector2(0, Time.time / 80));
    //}

    //void UpdateFinalColor(float fValue)
    //{
    //    Color tFinalColor = Color.white * Mathf.LinearToGammaSpace(fValue);
    //    _Lightwall.material.SetColor("_EmissionColor", tFinalColor);
    //    DynamicGI.SetEmissive(_Lightwall, tFinalColor);
    //}
}
