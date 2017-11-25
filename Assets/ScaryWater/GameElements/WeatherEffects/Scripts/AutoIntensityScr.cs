using UnityEngine;
using System.Collections;

public class AutoIntensityScr : MonoBehaviour
{
    float mfSkySpeed = 1f;
    Light mMainLight;
    Material mSkyMat;

    public Gradient _NightDayColor;
    public float _fMaxIntensity = 3f;
    public float _fMinIntensity = 0f;
    public float _fMinPoint = -0.2f;
    public float _fMaxAmbient = 1f;
    public float _fMinAmbient = 0f;
    public float _fMinAmbientPoint = -0.2f;
    public Gradient _NightDayFogColor;
    public AnimationCurve _FogDensityCurve;
    public float _fFogScale = 1f;
    public float _fDayAtmosphereThickness = 0.4f;
    public float _fNightAtmosphereThickness = 0.87f;
    public Vector3 _vDayRotateSpeed;
    public Vector3 _vNightRotateSpeed;
    public bool _bNightTime = false;

    void Start()
    {
        mMainLight = GetComponent<Light>();
        mSkyMat = RenderSettings.skybox;
    }

    void Update()
    {
        UpdateSunIntensity();
        AdjustRotationSpeedOfSkyBox();
    }

    void UpdateSunIntensity()
    {
        float tRange = 1 - _fMinPoint;
        float tDot = Mathf.Clamp01((Vector3.Dot(mMainLight.transform.forward, Vector3.down) - _fMinPoint) / tRange);
        float tIntensity = ((_fMaxIntensity - _fMinIntensity) * tDot) + _fMinIntensity;

        mMainLight.intensity = tIntensity;

        tRange = 1 - _fMinAmbientPoint;
        tDot = Mathf.Clamp01((Vector3.Dot(mMainLight.transform.forward, Vector3.down) - _fMinAmbientPoint) / tRange);
        tIntensity = ((_fMaxAmbient - _fMinAmbient) * tDot) + _fMinAmbient;
        RenderSettings.ambientIntensity = tIntensity;

        mMainLight.color = _NightDayColor.Evaluate(tDot);
        RenderSettings.ambientLight = mMainLight.color;

        RenderSettings.fogColor = _NightDayFogColor.Evaluate(tDot);
        RenderSettings.fogDensity = _FogDensityCurve.Evaluate(tDot) * _fFogScale;

        tIntensity = ((_fDayAtmosphereThickness - _fNightAtmosphereThickness) * tDot) + _fNightAtmosphereThickness;
        mSkyMat.SetFloat("_AtmosphereThickness", tIntensity);

        if (tDot > 0)
        {
            //Debug.Log("DayTime");
            transform.Rotate(_vDayRotateSpeed * Time.deltaTime * mfSkySpeed);
            _bNightTime = false;
        }
            
        else
        {
            //Debug.Log("NightTime");
            transform.Rotate(_vNightRotateSpeed * Time.deltaTime * mfSkySpeed);
            _bNightTime = true;
        }
    }

    void AdjustRotationSpeedOfSkyBox()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            mfSkySpeed *= 0.5f;
        if (Input.GetKeyDown(KeyCode.E))
            mfSkySpeed *= 2f;
    }
}
